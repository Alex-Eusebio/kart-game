using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileProjectile : Projectile
{
    [SerializeField]
    private MeshRenderer mesh;
    private float stunDuration;
    public SphereCollider explosionArea;
    [SerializeField]
    private GameObject explosionSprite;
    private float explosionDuration;

    protected override void FixedUpdate()
    {
        if (explosionArea.enabled)
        {
            explosionDuration -= Time.deltaTime;

            if (explosionDuration <= 0)
            {
                Destroy(gameObject);
            }
            return;
        }

        base.FixedUpdate();
    }

    protected override void Effect(Collider other)
    {
        if (other.GetComponent<CarSystem>() != null /*|| other.GetComponent<AiCarSystem>() != null*/)
        {
            other.GetComponent<CarSystem>().stunDuration = stunDuration;
        }
    }

    private void TriggerExplosion()
    {
        mesh.enabled = false;
        projectileArea.enabled = false;
        explosionArea.enabled = true;
        explosionSprite.SetActive(true);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (explosionArea.enabled)
        {
            Effect(other);
            return;
        }

        TriggerExplosion();
        base.OnTriggerEnter(other);
    }

    public void SetDuration(float stunDuration, float explosionDuration)
    {
        this.stunDuration = stunDuration;
        this.explosionDuration = explosionDuration;
    }
}
