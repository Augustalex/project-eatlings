using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FarmerInput : MonoBehaviour
{
    private PlayerInput _playerInput;
    private bool _noPlayer = true;
    private FarmerMovement _farmerMovement;

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();

        ReloadPlayerAssets();
        SetupEvents();
    }

    private void ReloadPlayerAssets()
    {
        _farmerMovement = GetComponentInChildren<FarmerMovement>();
        _noPlayer = false;
    }

    private void SetupEvents()
    {
        SetupAction(_playerInput.currentActionMap["Move"], (c) =>
        {
            if (_noPlayer) return;
            var rawMovementVector = c.ReadValue<Vector2>();
            _farmerMovement.SetMovementVector(rawMovementVector);
        });
    }

    public void TearDownEvents()
    {
        // TearDownAction(_playerInput.currentActionMap["Fire"], _shipGun.Fire);
    }

    private void SetupAction(InputAction action, Action<InputAction.CallbackContext> callback)
    {
        action.started += callback;
        action.performed += callback;
        action.canceled += callback;
    }

    private void TearDownAction(InputAction action, Action<InputAction.CallbackContext> callback)
    {
        action.started -= callback;
        action.performed -= callback;
        action.canceled -= callback;
    }
}