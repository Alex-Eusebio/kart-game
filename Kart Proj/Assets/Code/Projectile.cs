using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public SphereCollider projectileArea;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarSystem>() != null /*|| other.GetComponent<AiCarSystem>() != null*/)
        {
            Effect(other);

            Destroy(gameObject);
        }
    }

    protected abstract void Effect(Collider other);
}
