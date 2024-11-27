using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileProjectile : Projectile
{
    private float stunDuration;

    protected override void Effect(Collider other)
    {
        if (other.GetComponent<CarSystem>() != null /*|| other.GetComponent<AiCarSystem>() != null*/)
        {
            other.GetComponent<CarSystem>().stunDuration = stunDuration;
        }
    }

    public void SetDuration(float stunDuration, float explosionDuration)
    {
        this.stunDuration = stunDuration;
    }
}
