using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTile : MonoBehaviour
{
    public GameObject hole;
    
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
}
