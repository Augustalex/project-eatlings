using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlayersManager : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup _cinemachineTargetGroup;
    
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

    public void OnPlayerJoined(PlayerInput player)
    {
        _playerCount += 1;
        Debug.Log(player);
        _cinemachineTargetGroup.AddMember(player.transform.GetChild(0).transform, 1f, 0f);
    }

    public int PlayerCount()
    {
        return _playerCount;
    }
}
