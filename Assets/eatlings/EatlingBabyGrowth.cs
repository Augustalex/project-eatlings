using UnityEngine;

public class EatlingBabyGrowth : MonoBehaviour
{
    // Public
    public GameObject teen;
    public float timeUntilTeen = 10;
    public float timeUntilFullyGrown = 30;

    // Private

    private bool _planted;
    private float _growth;
    private bool _fullyGrown;
    private FarmTile _tile;

    private void Awake()
    {
        teen.SetActive(false);
    }

    private void Update()
    {
        if (_fullyGrown) return;

        if (_planted)
        {
            _growth += Time.deltaTime;

            if (!teen.activeSelf)
            {
                if (_growth > timeUntilTeen)
                {
                    teen.SetActive(true);
                }
            }
            else if (!_fullyGrown)
            {
                if (_growth > timeUntilFullyGrown)
                {
                    var eatlingModeController = GetComponentInParent<EatlingModeController>();
                    eatlingModeController.SetFullyGrown();
                    eatlingModeController.SetFullyGrownPlantedAt(_tile);
                    _fullyGrown = true;
                }
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
}