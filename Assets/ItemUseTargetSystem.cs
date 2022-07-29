using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUseTargetSystem : MonoBehaviour
{
    // Public

    public GameObject targetHighlight;

    public enum TargetType
    {
        PlantedTile
    }

    public TargetType[] targetTypes = new TargetType[] { };

    // Private 
    private GameObject _currentTarget;

    // Public

    public GameObject GetCurrentTarget()
    {
        return _currentTarget;
    }

    public void TargetNext()
    {
        var next = FindNextTarget();
        _currentTarget = next;
        if (next)
        {
            targetHighlight.SetActive(true);
            targetHighlight.transform.position = next.transform.position + Vector3.up * 2f;
        }
        else
        {
            targetHighlight.SetActive(false);
        }
    }

    // Private

    private GameObject FindNextTarget()
    {
        var hits = Physics.OverlapSphere(transform.position, 2f);

        foreach (var hit in hits)
        {
            foreach (var targetType in targetTypes)
            {
                if (targetType == TargetType.PlantedTile)
                {
                    var target = CheckHitForPlantedTile(hit);
                    if (target) return target;
                }
            }
        }

        return null;
    }

    private GameObject CheckHitForPlantedTile(Collider hit)
    {
        var tile = hit.GetComponent<FarmTile>();
        if (tile == null) return null;
        if (tile.Vacant()) return null;

        return tile.GetOccupant();
    }
}