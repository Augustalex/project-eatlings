using UnityEngine;

public class GrownEatlingPlanted : MonoBehaviour
{
    private FarmTile _tile;

    public void SetPlanted(FarmTile tile)
    {
        _tile = tile;
    }

    void Awake()
    {
        GetComponent<Pickupable>().WasPickedUp += Unplant;
    }

    private void Unplant()
    {
        if (_tile)
        {
            _tile.Eject();
        }
    }
}