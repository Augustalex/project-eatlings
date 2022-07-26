using System;
using UnityEngine;

[RequireComponent(typeof(FarmerPivotAccess))]
public class FarmerItemSelector : MonoBehaviour
{
    // Private
    private Rigidbody _rigidbody;
    private Pickupable _selectedItem;
    private Highlightable _highlighted;
    private FarmerPivotAccess _farmerPivotAccess;
    private Vector3 _gizmoLastHighlightedPosition;

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
    }

    void Update()
    {
        var highlightedItem = MostLikelyItemAtHighlightPosition();
        if (highlightedItem)
        {
            var pickupable = highlightedItem.GetComponent<Pickupable>();
            if (pickupable)
            {
                _selectedItem = pickupable;
            }

            var highlightable = highlightedItem.GetComponent<Highlightable>();
            if (highlightable)
            {
                _highlighted = highlightable;
                highlightable.Highlight();
            }
        }
        else
        {
            if (_highlighted)
            {
                _highlighted.StopHighlight();
            }

            _highlighted = null;
            _selectedItem = null;
        }
    }

    private GameObject MostLikelyItemAtHighlightPosition()
    {
        var hits = Physics.OverlapBox(HighlightPosition(), HighlightHitBoxHalfExtends());
        var closestDistance = Mathf.Infinity;
        GameObject closestObject = null;
        foreach (var hit in hits)
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

        Debug.Log("CLOSEST: " + closestObject + ", closest distance: " + closestDistance);

        return closestObject;
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(_gizmoLastHighlightedPosition, Vector3.one * .3f);
    }
}