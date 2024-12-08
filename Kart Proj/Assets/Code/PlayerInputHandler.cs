using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] CarSystem carSystem;

    public void OnHorizontal(InputAction.CallbackContext value)
    {
        carSystem.steerInput = value.ReadValue<float>();
    }

    public void OnVertical(InputAction.CallbackContext value)
    {
        carSystem.speedInput = value.ReadValue<float>();
    }

    public void OnDrift(InputAction.CallbackContext value)
    {
        carSystem.isTryingToDrift = value.action.triggered;
    }

    public void OnSpecial(InputAction.CallbackContext value)
    {
        carSystem.usedSpecial = value.action.triggered;
    }

    public void OnRespawn(InputAction.CallbackContext value)
    {
        carSystem.RespawnCar();
    }
}
