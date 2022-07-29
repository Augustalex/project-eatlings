using System.Linq;
using UnityEngine;

public class ItemUseTargetSystem : MonoBehaviour
{
    // Public

    public GameObject targetHighlight;

    public enum TargetType
    {
        HealthyPlantedEatlings
    }

    public enum SortType
    {
        WaterNeed
    }

    public TargetType[] targetTypes = new TargetType[] { };
    public SortType sortType = SortType.WaterNeed;

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

    public void NullTarget()
    {
        targetHighlight.SetActive(false);
        _currentTarget = null;
    }

    // Private

    private void Awake()
    {
        targetHighlight.SetActive(false);
    }

    private GameObject FindNextTarget()
    {
        var hits = Physics.OverlapSphere(transform.position, 2f);
        if (hits.Length == 0) return null;

        return hits
            .Select(CheckTargetTypes)
            .Where(h => h != null)
            .OrderBy(hit =>
            {
                if (sortType == SortType.WaterNeed)
                {
                    return hit.GetComponent<EatlingBabyGrowth>().WaterLevel();
                }
                else
                {
                    return 0f;
                }
            })
            .FirstOrDefault();
    }

    private GameObject CheckTargetTypes(Collider hit)
    {
        foreach (var targetType in targetTypes)
        {
            if (targetType == TargetType.HealthyPlantedEatlings)
            {
                var target = CheckHitForHealthyPlantedEatling(hit);
                if (target) return target;
            }
        }

        return null;
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
}