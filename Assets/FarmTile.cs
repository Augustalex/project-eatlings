using UnityEngine;

public class FarmTile : MonoBehaviour
{
    public GameObject hole;

    private GameObject _occupant;

    void Awake()
    {
        hole.SetActive(false);
    }

    public void Occupy(GameObject o)
    {
        _occupant = o;

        hole.SetActive(true);
    }

    public bool Vacant()
    {
        return _occupant == null;
    }

    public void Eject()
    {
        _occupant = null;
        hole.SetActive(false);
    }
}