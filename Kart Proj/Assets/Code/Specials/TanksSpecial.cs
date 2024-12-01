using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanksSpecial : SpecialAbility
{
    [Header("Values")]
    [SerializeField]
    private float throwingStrenght;
    [SerializeField]
    private float throwingBonusSpeedPer;
    [SerializeField]
    private float stunDuration;
    [SerializeField]
    private float rocketSpeed;

    [Header("GameObjects")]
    [SerializeField]
    private GameObject throwablePrefab;
    [SerializeField]
    Transform throwSpawnPoint;

    public override bool IsAvailable()
    {
        return resource >= maxResource;
    }

    protected override void ExecuteAbility()
    {
        Throw();
    }

    void Throw()
    {
        carSystem.animControll.UpdateSpecial(true);
        GameObject projectile = MonoBehaviour.Instantiate(throwablePrefab, throwSpawnPoint.position, carSystem.gameObject.transform.rotation);
        projectile.GetComponentInChildren<MissileProjectile>().creator = carSystem;
        projectile.GetComponentInChildren<MissileProjectile>().SetDuration(stunDuration, rocketSpeed);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = throwSpawnPoint.forward * (throwingStrenght + (carSystem.currentSpeed + carSystem.bonusSpeed) * throwingBonusSpeedPer);
        }
        carSystem.animControll.UpdateSpecial(false);
    }

    public override void Charge()
    {
        if (resource < maxResource)
        {
            resource += Time.deltaTime;
        }
    }
}
