using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileProjectile : Projectile
{
    private float stunDuration;
    private float speed = 1;
    public GameObject target;

    protected override void Effect(Collider other)
    {
        if (other.GetComponent<CarSystem>() != null /*|| other.GetComponent<AiCarSystem>() != null*/)
        {
            other.GetComponent<CarSystem>().stunDuration = stunDuration;
        }
    }

    public void SetDuration(float stunDuration, float speed)
    {
        this.stunDuration = stunDuration;
        this.speed = speed;
    }

    public void FoundTarget(GameObject target)
    {
        this.target = target;
    }

    protected override void FixedUpdate()
    {
        if (target != null)
        {
            Debug.Log(target);
            Vector3 pos = target.transform.position;
            pos.y += 1.5f;
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, pos, speed);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (target != null)
        {
            if (other.GetComponent<CarSystem>() != null)
            {
                if (other.GetComponent<CarSystem>() != creator)
                {
                    Effect(other);

                    Destroy(gameObject.transform.parent.gameObject);
                }
            }
        }
    }
}
