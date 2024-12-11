using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] CarSystem carSystem;
    [SerializeField] public bool playerHasControll = false;

    public void OnHorizontal(InputAction.CallbackContext value)
    {
        if (playerHasControll)
            carSystem.steerInput = value.ReadValue<float>();
    }

    public void OnVertical(InputAction.CallbackContext value)
    {
        if (playerHasControll)
            carSystem.speedInput = value.ReadValue<float>();
    }

    public void OnDrift(InputAction.CallbackContext value)
    {
        if (playerHasControll)
            carSystem.isTryingToDrift = value.action.triggered;
    }

    public void OnSpecial(InputAction.CallbackContext value)
    {
        if (playerHasControll)
            carSystem.usedSpecial = value.action.triggered;
    }

    public void OnRespawn(InputAction.CallbackContext value)
    {
        if (playerHasControll)
            carSystem.RespawnCar();
    }
}
