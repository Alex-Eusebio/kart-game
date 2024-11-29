using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public SphereCollider projectileArea;
    [SerializeField]
    protected float emergencyTimer = 12f;
    public CarSystem creator;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarSystem>() != null || other.GetComponent<AICarSystem>() != null)
        { 
            if (other.GetComponent<CarSystem>() != creator || other.GetComponent<AICarSystem>() != creator)
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
