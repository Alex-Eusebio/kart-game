using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileProjectile : Projectile
{
    private float stunDuration;
    private float speed = 1;
    private float rewardLifeTime;
    public GameObject target;
    private float startLifeTime;

    protected override void Effect(Collider other)
    {
        if (other.transform.parent.GetComponentInChildren<CarSystem>() != null /*|| other.GetComponent<AiCarSystem>() != null*/)
        {
            other.transform.parent.GetComponentInChildren<CarSystem>().stunDuration = stunDuration;

            if (emergencyTimer < startLifeTime * rewardLifeTime)
            {
                creator.GetComponent<TanksSpecial>().RocketReward();
            }
        }
    }

    public void SetDuration(float stunDuration, float speed, float lifeTime, float rewardLifeTime)
    {
        this.stunDuration = stunDuration;
        this.speed = speed;
        emergencyTimer = lifeTime;
        startLifeTime = lifeTime;
        this.rewardLifeTime = rewardLifeTime;
    }

    public void FoundTarget(GameObject target)
    {
        this.target = target;
    }

    protected override void FixedUpdate()
    {
        emergencyTimer -= Time.deltaTime;

        if (emergencyTimer <= 0)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }

        if (target != null)
        {
            Vector3 pos = target.transform.position;
            pos.y += 2.5f;
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, pos, speed);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (target != null)
        {
            Debug.Log(other.gameObject);
            if (other.transform.parent.GetComponentInChildren<CarSystem>() && other.transform.parent.GetComponentInChildren<CarSystem>() != creator)
            { 
                Effect(other);

                Destroy(gameObject.transform.parent.gameObject);
            }
        }
    }
}
