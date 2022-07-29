using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayersManager : MonoBehaviour
{
    private static GamePlayersManager _instance;
    private int _playerCount;

    void Awake()
    {
        _instance = this;
    }

    public static GamePlayersManager Get()
    {
        return _instance;
    }

    public void OnPlayerJoined()
    {
        _playerCount += 1;
    }

    public int PlayerCount()
    {
        return _playerCount;
    }
}
