using UnityEngine;

public class FarmerItemSelector : MonoBehaviour
{
    // Private
    private Rigidbody _rigidbody;
    private Pickupable _selectedItem;

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
        }
    }

    private GameObject MostLikelyItemAtHighlightPosition()
    {
        var hits = Physics.OverlapBox(HighlightPosition(), Vector3.one * .4f);
        var closestDistance = Mathf.Infinity;
        GameObject closestObject = null;
        foreach (var hit in hits)
        {
            var gridPosition = FarmGridUtils.OrientToGrid(hit.transform.position);
            var playerGridPosition = FarmGridUtils.OrientToGrid(_rigidbody.position);
            var distance = Vector2.Distance(gridPosition, playerGridPosition);
            if (distance < closestDistance)
            {
                closestObject = hit.gameObject;
                closestDistance = distance;
            }
        }

        return closestObject;
    }

    private Vector3 HighlightPosition()
    {
        var position = _rigidbody.position;
        var nextPosition = position + _rigidbody.transform.forward;
        var gridAlignedPosition = FarmGridUtils.GridAlign(nextPosition);

        return gridAlignedPosition;
    }
}