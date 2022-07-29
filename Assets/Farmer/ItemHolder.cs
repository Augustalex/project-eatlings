using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    // Public
    public GameObject pivot;
    public GameObject rotationPivot;

    public event Action DidHoldItem;
    public event Action DidDropItem;

    // Private
    private GameObject _itemGO;
    private Pickupable _item;

    // Public

    public void Pickup(Pickupable item)
    {
        _itemGO = item.gameObject;
        _item = item;

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
                Debug.Log(pivot.transform.rotation.eulerAngles);
                _itemGO.transform.rotation = Quaternion.Euler(0f, pivot.transform.rotation.eulerAngles.y, 0f);
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
            Debug.Log("TILE POINT: " + pivotLevelPosition);
            return pivotLevelPosition;
        }
        else
        {
            Debug.Log("PIVOT POINT: " + pivot.transform.position);
            return pivot.transform.position;
        }
    }

    private GameObject NearestTile()
    {
        var closestDistance = Mathf.Infinity;
        GameObject closestObject = null;
        foreach (var hit in HighlightHits())
        {
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
}