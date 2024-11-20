using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public SphereCollider projectileArea;
    private float emergencyTimer = 12f;
    public CarSystem creator;

    protected virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if (other.GetComponent<CarSystem>() != null /*|| other.GetComponent<AiCarSystem>() != null*/)
        {
            if (other.GetComponent<CarSystem>() != creator)
            {
                Effect(other);

                Destroy(gameObject);
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        emergencyTimer -= Time.deltaTime;

        if (emergencyTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected abstract void Effect(Collider other);
}
