using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterPad : MonoBehaviour, IChangeSpeed
{
    [SerializeField]
    float speedChange;

    public float ChangeSpeed(float speed)
    {
        return speed + speedChange;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("YOO");
        CarSystem carSystem = other.transform.parent.GetComponentInChildren<CarSystem>();
        carSystem.speed = ChangeSpeed(carSystem.speed);
        
    }
}
