using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EatlingBabyGrowth : MonoBehaviour
{
    // Public

    public GameObject teen;
    public EatlingSettings eatlingSettings;

    public event Action NeedsWater;
    public event Action GotWater;
    public event Action Died;

    // Private

    private bool _planted;
    private float _growth;
    private bool _fullyGrown;
    private FarmTile _tile;

    private float _waterLevel;
    private float _dryLevel;
    private bool _dead;
    private Pickupable _pickupConfig;

    // Public
    public bool IsPlanted()
    {
        return _planted;
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

    public void Water(float water)
    {
        _waterLevel = Mathf.Min(eatlingSettings.maxWater, _waterLevel + water);
    }

    public bool IsDead()
    {
        return _dead;
    }

    public float WaterLevel()
    {
        return Mathf.Max(0f, _waterLevel);
    }

    // Private


    private void Awake()
    {
        _pickupConfig = GetComponent<Pickupable>();

        teen.SetActive(false);
    }

    private void Update()
    {
        if (_dead) return;
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
            if (_dryLevel == 0f) NeedsWater?.Invoke();

            _dryLevel += 1f * Time.deltaTime;
        }
        else
        {
            if (_dryLevel != 0f) GotWater?.Invoke();

            _dryLevel = 0f;
        }

        if (_dryLevel > eatlingSettings.maxDryTimeBeforeDeath)
        {
            DieFromDrought();
        }
    }

    private void DieFromDrought()
    {
        Debug.Log("EATLING DIED FROM DROUGHT");

        _dead = true;
        SetAsReadyToPickUp();

        var currentRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(currentRotation.x + 20f, currentRotation.y,
            currentRotation.z + (Random.value < .5f ? -20f : 20f));

        Died?.Invoke();
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

    private void PlantAt(FarmTile farmTile)
    {
        _tile = farmTile;
        farmTile.Occupy(gameObject);

        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        SetAsPlanted();

        transform.position = farmTile.transform.position + Vector3.up * -0.081f;
    }

    private void SetAsReadyToPickUp()
    {
        _planted = false;
        _pickupConfig.Enable();
    }

    private void SetAsPlanted()
    {
        _pickupConfig.Disable();
        _planted = true;
    }
}