using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : ScriptableObject
{
    [SerializeField]
    float duration;
    public float bonusSpeed;
    public float bonusSteering;

    public bool IsExpired => duration <= 0;

    // Update is called once per frame
    public void UpdateDuration(float deltaTime)
    {
        duration -= deltaTime;
    }

    public void Setup(float bonusSpeed, float bonusSteering, float duration)
    {
        this.bonusSpeed = bonusSpeed;
        this.bonusSteering = bonusSteering;
        this.duration = duration;
    }
}
