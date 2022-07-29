using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    // Public
    
    public GameObject forrest;
    public GameObject farm;
    
    // Private
    
    private static GameSceneManager _instance;

    // Public
    
    public static GameSceneManager Get()
    {
        return _instance;
    }

    public void ChangeSceneToFarm()
    {
        forrest.SetActive(false);        
        farm.SetActive(true);
    }

    public void ChangeSceneToForrest()
    {
        farm.SetActive(false);
        
        forrest.SetActive(true);
    }
    
    // Private
    
    void Awake()
    {
        _instance = this;
        
        ChangeSceneToFarm();
    }
}
