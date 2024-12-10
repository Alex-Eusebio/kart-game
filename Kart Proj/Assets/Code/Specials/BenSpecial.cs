using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BenSpecial : SpecialAbility
{
    [Header("Values")]
    [SerializeField]
    private float boostStrenghtPerLvl;
    [SerializeField]
    private float boostStrenghtCap;
    [SerializeField]
    private float steeringBuffPerLvl;
    [SerializeField]
    private float steeringBuffCap;
    [SerializeField]
    private float boostDurationPerLvl;

    public GameObject crystal;

    private bool Ignore(Collider other)
    {
        return other.gameObject == carSystem.sphere.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BenCrystal>())
        {
            if (other.gameObject.GetComponent<BenCrystal>().owner == carSystem)
            {
                Charge();
                Destroy(other.gameObject);
            }
        }
    }

    protected override void ExecuteAbility()
    {
        Boost boost = ScriptableObject.CreateInstance<Boost>();

        float boostStrenght = boostStrenghtPerLvl * resource;

        if (boostStrenght > boostStrenghtCap)
            boostStrenght = boostStrenghtCap;

        float steeringBuff = steeringBuffPerLvl * resource;

        if (steeringBuff > steeringBuffCap)
            steeringBuff = steeringBuffCap;

        float boostDuration = boostDurationPerLvl * resource;

        boost.Setup(boostStrenght, steeringBuff, boostDuration);
        carSystem.boostManager.AddBoost(boost);
    }

    public override void Activate()
    {
        if (resource > 0)
        {
            ExecuteAbility();
            resource = 0;
        }
    }

    public override void Charge()
    {
        if (resource < maxResource)
        {
            resource++;

            CheckIfReady();
        }
    }

    protected override void SfxActivate()
    {
        AudioManager.Instance.PlaySfx("benSpecialActivate");
    }

    protected override void SfxReady()
    {
        AudioManager.Instance.PlaySfx("benSpecialReady");
    }
}
