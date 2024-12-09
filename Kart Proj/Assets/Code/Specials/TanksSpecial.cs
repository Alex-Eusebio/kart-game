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
    [SerializeField]
    private float rocketLifeTimeReward;
    [SerializeField]
    private float rocketLifeTime;
    [SerializeField]
    private float boostGained;
    [SerializeField]
    private float boostDuration;

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
        projectile.GetComponentInChildren<MissileProjectile>().SetDuration(stunDuration, rocketSpeed, rocketLifeTime, rocketLifeTimeReward);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = throwSpawnPoint.forward * (throwingStrenght + (carSystem.currentSpeed + carSystem.bonusSpeed) * throwingBonusSpeedPer);
        }
        carSystem.animControll.UpdateSpecial(false);
    }

    public void RocketReward()
    {
        Boost boost = ScriptableObject.CreateInstance<Boost>();

        boost.Setup(boostGained, 0, boostDuration);
        carSystem.boostManager.AddBoost(boost);
    }

    public override void Charge()
    {
        if (resource < maxResource)
        {
            resource += Time.deltaTime;
        }
    }
}
