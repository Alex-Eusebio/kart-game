using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZumzumSpecial : SpecialAbility
{
    [Header("Values")]
    [SerializeField]
    private float throwingStrenght;
    [SerializeField]
    private float throwingBonusSpeedPer;
    [SerializeField]
    private float slowStrenght;
    [SerializeField]
    private float slowDuration;
    [SerializeField]
    private float cooldown;
    private float inCooldown;
    [SerializeField]
    private float puddleDuration;

    [Header("GameObjects")]
    [SerializeField]
    private GameObject throwablePrefab;
    [SerializeField]
    Transform throwSpawnPoint;

    private void Awake()
    {
        maxResource = 0;
    }

    public override bool IsAvailable()
    {
        return inCooldown <= 0;
    }

    protected override void ExecuteAbility()
    {
        Throw();
        inCooldown = cooldown;
    }

    void Throw()
    {
        GameObject projectile = MonoBehaviour.Instantiate(throwablePrefab, throwSpawnPoint.position, Quaternion.identity);
        projectile.GetComponent<HiveProjectile>().SetSlow(slowDuration, slowStrenght, puddleDuration);
        projectile.GetComponent<HiveProjectile>().creator = carSystem;
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = throwSpawnPoint.forward * (throwingStrenght + (carSystem.currentSpeed + carSystem.bonusSpeed) * throwingBonusSpeedPer);
        }
    }

    public override void Charge()
    {
        if (inCooldown > 0)
        {
            inCooldown -= Time.deltaTime;
        }
    }
}
