using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileProjectile : Projectile
{
    public float stunDuration;
    protected override void Effect(Collider other)
    {
        other.GetComponent<CarSystem>().stunDuration = stunDuration;
    }
}
