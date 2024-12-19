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
    private bool affectedOwner = false;
    private List<Collider> enemyHit = new List<Collider>(); 

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
        if (other.GetComponent<CarSystem>() != null) {
            if (other.GetComponent<CarSystem>() != creator)
            {
                if (!enemyHit.Contains(other))
                {
                    Boost slow = ScriptableObject.CreateInstance<Boost>();
                    slow.Setup(slowStrenght, 0, slowDuration);

                    other.GetComponent<CarSystem>().boostManager.AddBoost(slow);
                    AudioManager.Instance.PlaySfx("zumzumSpecialHitEnemy");
                    enemyHit.Add(other);
                }
            }
            else
            {
                if (!affectedOwner)
                {
                    Boost slow = ScriptableObject.CreateInstance<Boost>();
                    slow.Setup(-slowStrenght, 0, slowDuration);

                    other.GetComponent<CarSystem>().boostManager.AddBoost(slow);
                    AudioManager.Instance.PlaySfx("zumzumSpecialHitSelf");
                    affectedOwner = true;
                }
            }
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
        AudioManager.Instance.PlaySfx("zumzumSpecialLand");
    }
}
