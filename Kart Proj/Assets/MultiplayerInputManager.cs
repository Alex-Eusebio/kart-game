using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerInputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput[] cars;

    void Start()
    {
        cars = FindObjectsOfType<PlayerInput>();

        // Get all input devices
        var devices = InputSystem.devices;

        for (int i = 0; i < cars.Length && i < devices.Count; i++)
        {
            var playerInput = cars[i].GetComponent<PlayerInput>(); // Get the PlayerInput component

            if (playerInput != null) // Ensure playerInput is not null
            {
                // Check if the device is either a Gamepad or Joystick
                if (devices[i] is Gamepad || devices[i] is Joystick)
                {
                    Debug.Log($"{devices[i].displayName} assigned to {cars[i].name} ({devices[i].GetType()})");
                    // Switch control scheme to "Controler" (check if this name matches your Input Actions)
                    playerInput.SwitchCurrentControlScheme("Controler", devices[i]);
                }
                else if (devices[i] is Keyboard)
                {
                    Debug.Log($"{devices[i].displayName} assigned to {cars[i].name} ({devices[i].GetType()})");
                    // Switch control scheme to "Keyboard"
                    playerInput.SwitchCurrentControlScheme("Keyboard", devices[i]);
                }
            }
        }

        // Assign keyboard to the first car if needed
        if (Keyboard.current != null && cars.Length > 0)
        {
            var playerInput = cars[0];
            if (playerInput != null)
            {
                playerInput.SwitchCurrentControlScheme(Keyboard.current);
            }
        }


    }
}
