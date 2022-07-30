using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemUseTargetSystem : MonoBehaviour
{
    // Public

    public GameObject targetHighlight;

    public enum TargetSystem
    {
        ForwardToTheSides,
        UniformSphere
    }

    public enum TargetType
    {
        HealthyPlantedEatlings,
        VacantTile,
        Anywhere
    }

    public enum SortType
    {
        WaterNeed,
        Closest
    }

    public TargetSystem targetSystem = TargetSystem.UniformSphere;
    public TargetType[] targetTypes = new TargetType[] { };
    public SortType sortType = SortType.WaterNeed;

    public bool groundHighlight = true;

    // Private 
    private GameObject _currentTarget;

    private GameObject pivot;

    // Public

    public GameObject GetCurrentTarget()
    {
        return _currentTarget;
    }

    // public Vector3 ApplicableDropPosition()
    // {
    //     // Drops always happen in forward-sides direction
    //     var target = FindNextTargetUsing(TargetSystem.ForwardToTheSides, new TargetType[] {TargetType.Anywhere},
    //         SortType.Closest);
    //     return target;
    // }

    public void TargetNext()
    {
        var next = FindNextTarget();
        _currentTarget = next;
        if (next)
        {
            targetHighlight.SetActive(true);

            var newPosition = next.transform.position + Vector3.up * 2f;
            targetHighlight.transform.position = new Vector3(newPosition.x,
                groundHighlight ? 0f : newPosition.y, newPosition.z);
        }
        else
        {
            targetHighlight.SetActive(false);
        }
    }

    public void NullTarget()
    {
        targetHighlight.SetActive(false);
        _currentTarget = null;
    }

    // Private

    private void Awake()
    {
        pivot = gameObject;

        targetHighlight.transform.SetParent(null);
        targetHighlight.SetActive(false);
    }

    private GameObject FindNextTarget()
    {
        return FindNextTargetUsing(targetSystem, targetTypes, sortType);
    }

    private GameObject FindNextTargetUsing(TargetSystem theTargetSystem, TargetType[] theTargetTypes,
        SortType theSortType)
    {
        var hits = theTargetSystem == TargetSystem.UniformSphere
            ? Physics.OverlapSphere(transform.position, 2f)
            : HighlightHits();

        return hits
            .Select(h => CheckTargetTypes(h, theTargetTypes))
            .Where(h => h != null)
            .OrderBy(hit =>
            {
                if (theSortType == SortType.WaterNeed)
                {
                    return hit.GetComponent<EatlingBabyGrowth>().WaterLevel();
                }
                else if (theSortType == SortType.Closest)
                {
                    return Vector3.Distance(transform.position, hit.transform.position);
                }
                else
                {
                    return 0f;
                }
            })
            .FirstOrDefault();
    }

    private GameObject CheckTargetTypes(Collider hit, TargetType[] theTargetTypes)
    {
        foreach (var targetType in theTargetTypes)
        {
            if (targetType == TargetType.HealthyPlantedEatlings)
            {
                var target = CheckHitForHealthyPlantedEatling(hit);
                if (target) return target;
            }
            else if (targetType == TargetType.VacantTile)
            {
                var target = CheckHitForVacantTile(hit);
                if (target) return target;
            }
        }

        return null;
    }

    private GameObject CheckHitForVacantTile(Collider hit)
    {
        var tile = hit.GetComponent<FarmTile>();
        if (tile == null) return null;

        return tile.Vacant() ? tile.gameObject : null;
    }

    private GameObject CheckHitForHealthyPlantedEatling(Collider hit)
    {
        var tile = hit.GetComponent<FarmTile>();
        if (tile == null) return null;
        if (tile.Vacant()) return null;

        var occupant = tile.GetOccupant();
        var babyGrowth = occupant.GetComponent<EatlingBabyGrowth>();
        if (!babyGrowth) return null;
        if (babyGrowth.IsDead()) return null;

        return occupant;
    }

    private IEnumerable<Collider> HighlightHits()
    {
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
        var forward = pivot.transform.forward * .5f;
        var sideForward = forward * .3f;
        var sideVector = pivot.transform.right * .3f;
        var transformPosition = transform.position;
        return new[]
        {
            FarmGridUtils.GridAlign(transformPosition + forward),
            FarmGridUtils.GridAlign(transformPosition + sideForward + sideVector),
            FarmGridUtils.GridAlign(transformPosition + sideForward - sideVector)
        };
    }
}