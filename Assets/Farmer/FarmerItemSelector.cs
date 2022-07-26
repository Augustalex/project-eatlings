using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(FarmerPivotAccess))]
[RequireComponent(typeof(FarmerState))]
public class FarmerItemSelector : MonoBehaviour
{
    // Private
    private Rigidbody _rigidbody;
    private Pickupable _selectedItem;
    private Highlightable _highlighted;
    private FarmerPivotAccess _farmerPivotAccess;
    private FarmerState _farmerState;

    // Gizmos
    private Vector3 _gizmoLastHighlightedPosition;
    private Vector3[] _gizmoHighlighted = new[] {Vector3.zero, Vector3.zero, Vector3.zero};
    private GameObject _itemToHighlight;

    // Public

    public bool HasSelectedItem()
    {
        return _selectedItem != null;
    }

    public Pickupable SelectedItem()
    {
        return _selectedItem;
    }

    // Private

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _farmerPivotAccess = GetComponent<FarmerPivotAccess>();
        _farmerState = GetComponent<FarmerState>();
    }

    void Update()
    {
        UpdatedItemToBeHighlighted();

        if (_farmerState.CanHighlightItem() && _itemToHighlight != null)
        {
            var pickupable = _itemToHighlight.GetComponent<Pickupable>();
            if (pickupable && pickupable.CanPickUp())
            {
                _selectedItem = pickupable;
            }

            var highlightable = _itemToHighlight.GetComponent<Highlightable>();
            if (highlightable)
            {
                ResetHighlightedObject();

                _highlighted = highlightable;
                highlightable.Highlight();
            }
        }
        else
        {
            ResetHighlightedObject();

            _highlighted = null;
            _selectedItem = null;
        }
    }

    private void ResetHighlightedObject()
    {
        if (_highlighted)
        {
            _highlighted.StopHighlight();
        }
    }

    private void UpdatedItemToBeHighlighted()
    {
        _itemToHighlight = MostLikelyItemAtHighlightPosition();
    }

    private GameObject MostLikelyItemAtHighlightPosition()
    {
        var closestDistance = Mathf.Infinity;
        GameObject closestObject = null;
        foreach (var hit in HighlightHits())
        {
            var hitAttachedRigidbody = hit.attachedRigidbody;
            if (!hitAttachedRigidbody) continue;

            var hitRoot = hitAttachedRigidbody.gameObject;

            var gridPosition = FarmGridUtils.OrientToGrid(hitRoot.transform.position);
            var playerGridPosition = FarmGridUtils.OrientToGrid(_rigidbody.position);
            var distance = Vector2.Distance(gridPosition, playerGridPosition);
            if (distance < closestDistance)
            {
                closestObject = hitRoot;
                closestDistance = distance;
            }
        }

        return closestObject;
    }

    private IEnumerable<Collider> HighlightHits()
    {
        var index = 0;
        return HighlightPositions().SelectMany((h) =>
        {
            _gizmoHighlighted[index++] = h;
            return Physics.OverlapBox(h, HighlightHitBoxHalfExtends());
        });
    }

    private Vector3 HighlightHitBoxHalfExtends()
    {
        return new Vector3(
            .4f,
            2f,
            .4f
        );
    }

    private Vector3 HighlightPosition()
    {
        var position = _rigidbody.position;
        var nextPosition = position + _farmerPivotAccess.pivot.transform.forward;
        var gridAlignedPosition = FarmGridUtils.GridAlign(nextPosition);

        _gizmoLastHighlightedPosition = gridAlignedPosition;

        return gridAlignedPosition;
    }

    private Vector3[] HighlightPositions()
    {
        var forward = _farmerPivotAccess.pivot.transform.forward;
        var sideForward = forward * 1.25f;
        var sideVector = _farmerPivotAccess.pivot.transform.right * .3f;
        return new[]
        {
            FarmGridUtils.GridAlign(_rigidbody.position + forward),
            FarmGridUtils.GridAlign(_rigidbody.position + sideForward + sideVector),
            FarmGridUtils.GridAlign(_rigidbody.position + sideForward - sideVector)
        };
    }

    private void OnDrawGizmos()
    {
        foreach (Vector3 pos in _gizmoHighlighted)
        {
            Gizmos.DrawCube(pos, HighlightHitBoxHalfExtends() * 2f);
        }

        // Gizmos.DrawCube(_gizmoLastHighlightedPosition, Vector3.one * .3f);
    }
}