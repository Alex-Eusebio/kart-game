using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoosterPad : MonoBehaviour, IChangeSpeed
{
    [SerializeField]
    float speedChange;
    [SerializeField]
    float maxTimer;
    float timer;

    public float ChangeSpeed(float speed, CarSystem car)
    {
        timer += Time.deltaTime;
        if (timer >= maxTimer)
        {
            if (car is not AICarSystem)
                AudioManager.Instance.PlaySfx("padBoost");
            timer = 0;
            return speedChange;
        }
        return 0;
    }
}
