using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    // Public
    public GameObject pivot;

    public event Action DidHoldItem;
    public event Action DidDropItem;

    // Private
    private GameObject _itemGO;
    private Pickupable _item;
    private ItemUseTargetSystem _itemTargetSystem;

    // Public

    public void Pickup(Pickupable item)
    {
        _itemGO = item.gameObject;
        _item = item;
        _itemTargetSystem = item.GetComponent<ItemUseTargetSystem>();

        item.PickedUp();

        DidHoldItem?.Invoke();
    }

    public bool HoldingItem()
    {
        return _itemGO != null;
    }

    // Private

    void Update()
    {
        if (HoldingItem())
        {
            if (_item.gridAligned)
            {
                _itemGO.transform.position = NearestApplicablePosition();
            }
            else
            {
                _itemGO.transform.position = pivot.transform.position;
                _itemGO.transform.rotation = Quaternion.Euler(0f, pivot.transform.rotation.eulerAngles.y, 0f);
            }

            if (_itemTargetSystem != null)
            {
                _itemTargetSystem.TargetNext();
            }
        }
    }

    public void Drop()
    {
        var pickupable = _itemGO.GetComponent<Pickupable>();
        pickupable.Dropped();

        var newPosition = NearestApplicablePosition();

        var itemRigidbody = _itemGO.GetComponent<Rigidbody>();

        var eatlingRoot = _itemGO.GetComponent<EatlingBabyGrowth>();
        if (eatlingRoot)
        {
            Destroy(itemRigidbody);
            eatlingRoot.transform.position = newPosition;
            eatlingRoot.Plant();
        }
        else
        {
            itemRigidbody.MovePosition(newPosition);
            itemRigidbody.AddForce(Vector3.up * .1f, ForceMode.Impulse);
        }

        DidDropItem?.Invoke();

        _itemGO = null;
        _item = null;

        if (_itemTargetSystem) _itemTargetSystem.NullTarget();
        _itemTargetSystem = null;
    }

    private Vector3 NearestApplicablePosition()
    {
        var nearestTile = NearestTile();
        if (nearestTile)
        {
            var position = nearestTile.transform.position;
            var pivotLevelPosition = new Vector3(
                position.x,
                pivot.transform.position.y,
                position.z
            );
            return pivotLevelPosition;
        }
        else
        {
            return pivot.transform.position;
        }
    }

    private GameObject NearestTile()
    {
        var closestDistance = Mathf.Infinity;
        GameObject closestObject = null;
        foreach (var hit in HighlightHits())
        {
            if (hit.CompareTag("Player")) continue;
            
            var tile = hit.gameObject.GetComponentInParent<FarmTile>();
            if (tile)
            {
                var gridPosition = FarmGridUtils.OrientToGrid(hit.gameObject.transform.position);
                var playerGridPosition = FarmGridUtils.OrientToGrid(pivot.transform.position);
                var distance = Vector2.Distance(gridPosition, playerGridPosition);
                if (distance < closestDistance)
                {
                    closestObject = hit.gameObject;
                    closestDistance = distance;
                }
            }
        }

        return closestObject;
    }

    private IEnumerable<Collider> HighlightHits()
    {
        var index = 0;
        return HighlightPositions().SelectMany((h) => { return Physics.OverlapBox(h, HighlightHitBoxHalfExtends()); });
    }

    private Vector3 HighlightHitBoxHalfExtends()
    {
        return new Vector3(
            .4f,
            2f,
            .4f
        );
    }

    private Vector3[] HighlightPositions()
    {
        var forward = pivot.transform.forward;
        var sideForward = forward * .5f;
        var sideVector = pivot.transform.right * .3f;
        var transformPosition = transform.position;
        return new[]
        {
            FarmGridUtils.GridAlign(transformPosition),
            FarmGridUtils.GridAlign(transformPosition + forward),
            FarmGridUtils.GridAlign(transformPosition + sideForward + sideVector),
            FarmGridUtils.GridAlign(transformPosition + sideForward - sideVector)
        };
    }

    public void Use()
    {
        // TODO: Can we use some interface or something here to make this more Polymorphic? :) Or maybe simple is better?
        var waterCan = _itemGO.GetComponent<WateringCan>();
        if (waterCan)
        {
            var target = _itemTargetSystem.GetCurrentTarget();
            if (target)
            {
                waterCan.Water(target);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var pond = other.GetComponent<Pond>();
        if (pond)
        {
            pond.TryApplyItem(_itemGO);
        }
    }
}