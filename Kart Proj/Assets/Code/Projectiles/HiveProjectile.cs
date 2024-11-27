using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveProjectile : Projectile
{
    [SerializeField]
    private MeshRenderer mesh;
    private float slowStrenght;
    private float slowDuration;
    public SphereCollider puddleArea;
    [SerializeField]
    private GameObject puddleSprite;
    private float puddleDuration;

    protected override void FixedUpdate()
    {
        if (puddleArea.enabled)
        {
            puddleDuration -= Time.deltaTime;

            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            if (puddleDuration <= 0)
            {
                Destroy(gameObject);
            }
            return;
        }

        base.FixedUpdate();
    }

    protected override void Effect(Collider other)
    {
        if (other.GetComponent<CarSystem>() != creator)
        {
            Boost slow = ScriptableObject.CreateInstance<Boost>();
            slow.Setup(slowStrenght, 0, slowDuration);

            other.GetComponent<CarSystem>().boostManager.AddBoost(slow);

        } else
        {
            Boost slow = ScriptableObject.CreateInstance<Boost>();
            slow.Setup(-slowStrenght, 0, slowDuration);

            other.GetComponent<CarSystem>().boostManager.AddBoost(slow);
        }
    }

    public void SetSlow(float slowDuration, float slowStrenght, float puddleDuration)
    {
        this.slowDuration = slowDuration;
        this.slowStrenght = slowStrenght;
        this.puddleDuration = puddleDuration;
        
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (puddleArea.enabled)
        {
            Effect(other);
            return;
        }

        TriggerExplosion();
        base.OnTriggerEnter(other);
    }

    private void TriggerExplosion()
    {
        mesh.enabled = false;
        projectileArea.enabled = false;
        puddleArea.enabled = true;
        puddleSprite.SetActive(true);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
