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
    private float puddleDuration;

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
        GameObject projectile = MonoBehaviour.Instantiate(throwablePrefab, throwSpawnPoint.position, Quaternion.identity);
        projectile.GetComponent<HiveProjectile>().SetSlow(slowDuration, slowStrenght, puddleDuration);
        projectile.GetComponent<HiveProjectile>().creator = carSystem;
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

        CheckIfReady();
    }

    protected override void SfxActivate()
    {
        AudioManager.Instance.PlaySfx("zumzumSpecialActivate");
    }

    protected override void SfxReady()
    {
        AudioManager.Instance.PlaySfx("zumzumSpecialReady");
    }
}
