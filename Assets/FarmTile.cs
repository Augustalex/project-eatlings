using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTile : MonoBehaviour
{
    public GameObject hole;
    public GameObject plantableHighlight;

    private GameObject _occupant;

    void Awake()
    {
        hole.SetActive(false);
    }

    void Update()
    {
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

    public void HighlightPlantable()
    {
        plantableHighlight.SetActive(true);
    }

    public void DisableHighlightPlantable()
    {
        plantableHighlight.SetActive(false);
    }
}