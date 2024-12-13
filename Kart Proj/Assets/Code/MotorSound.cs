using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorSound : MonoBehaviour
{
    AudioSource audioSource;
    CarSystem carSystem;

    [SerializeField]
    float minSpeed;
    [SerializeField]
    float maxSpeed;
    [SerializeField]
    float minPitch;
    [SerializeField]
    float maxPitch;
    [SerializeField]
    float pitchFromCar;

    private void Start()
    {
        audioSource = transform.parent.GetComponentInChildren<AudioSource>();
        carSystem = transform.parent.GetComponentInChildren<CarSystem>();

        this.gameObject.transform.localPosition = FindAnyObjectByType<AudioManager>().transform.localPosition;
    }

    private void Update()
    {
        EngineSound();
    }

    void EngineSound()
    {
        float speed = carSystem.currentSpeed + carSystem.bonusSpeed;

        if (speed < minSpeed)
        {
            audioSource.pitch = minPitch;
        } else if (speed > minSpeed && speed < maxSpeed)
        {
            audioSource.pitch = speed/50f;
        } else
        {
            audioSource.pitch = maxPitch;
        }
    }
}
