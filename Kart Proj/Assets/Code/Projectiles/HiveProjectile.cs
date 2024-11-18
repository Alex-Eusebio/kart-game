using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveProjectile : Projectile
{
    private float slowStrenght;
    private float slowDuration;
    protected override void Effect(Collider other)
    {
        Boost slow = ScriptableObject.CreateInstance<Boost>();
        slow.Setup(slowStrenght, 0, slowDuration);

        other.GetComponent<CarSystem>().boostManager.AddBoost(slow);
    }

    public void SetSlow(float slowDuration, float slowStrenght)
    {
        this.slowDuration = slowDuration;
        this.slowStrenght = slowStrenght;
    }
}
