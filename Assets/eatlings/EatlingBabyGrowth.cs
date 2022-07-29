using UnityEngine;

public class EatlingBabyGrowth : MonoBehaviour
{
    // Public

    public GameObject teen;
    public EatlingSettings eatlingSettings;

    // Private

    private bool _planted;
    private float _growth;
    private bool _fullyGrown;
    private FarmTile _tile;

    private float _waterLevel;
    private float _dryLevel;

    private void Awake()
    {
        teen.SetActive(false);
    }

    private void Update()
    {
        if (_fullyGrown) return;
        if (!_planted) return;

        Drink();
        Grow();
        UpdateDryLevel();
        
        EvolveIfPossible();
    }

    private void Drink()
    {
        _waterLevel = Mathf.Max(0f, _waterLevel -= eatlingSettings.waterConsumptionPerSecond * Time.deltaTime);
    }

    private void Grow()
    {
        if (_waterLevel > 0f)
        {
            _growth += Time.deltaTime;
        }
    }

    private void UpdateDryLevel()
    {
        if (_waterLevel <= 0f)
        {
            _dryLevel += 1f * Time.deltaTime;
        }
        else
        {
            _dryLevel = 0;
        }
        
        if (_dryLevel > eatlingSettings.maxDryTimeBeforeDeath)
        {
            DieFromDrought();
        }
    }

    private void DieFromDrought()
    {
        Debug.Log("EATLING DIED FROM DROUGHT");
        Destroy(gameObject.transform.parent.gameObject);
    }

    private void EvolveIfPossible()
    {
        if (!teen.activeSelf)
        {
            if (_growth > eatlingSettings.timeUntilTeen)
            {
                teen.SetActive(true);
            }
        }
        else if (!_fullyGrown)
        {
            if (_growth > eatlingSettings.timeUntilFullyGrown)
            {
                var eatlingModeController = GetComponentInParent<EatlingModeController>();
                eatlingModeController.SetFullyGrown();
                eatlingModeController.SetFullyGrownPlantedAt(_tile);
                _fullyGrown = true;
            }
        }
    }

    public void Plant()
    {
        var ray = new Ray(transform.position + Vector3.up, Vector3.down);
        var hits = Physics.RaycastAll(ray, 10f);
        foreach (var raycastHit in hits)
        {
            var farmTile = raycastHit.collider.GetComponent<FarmTile>();
            if (farmTile && farmTile.Vacant())
            {
                PlantAt(farmTile);
                return;
            }
        }
    }

    private void PlantAt(FarmTile farmTile)
    {
        _tile = farmTile;
        farmTile.Occupy(gameObject);

        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        GetComponent<Pickupable>().Disable();

        _planted = true;
        transform.position = farmTile.transform.position + Vector3.up * -0.081f;
    }

    public bool IsPlanted()
    {
        return _planted;
    }

    public void Water(float water)
    {
        _waterLevel = Mathf.Min(eatlingSettings.maxWater, _waterLevel + water);
    }
}