using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RataoSpecial : SpecialAbility
{
    [Header("Values")]
    [SerializeField]
    private float chargePerTick;
    [SerializeField]
    private float boostStrenght;
    [SerializeField]
    private float steeringBuff;
    [SerializeField]
    private float boostDuration;
    [SerializeField]
    private float jumpStrenght;
    [SerializeField]
    private RataoSpecialCharger specialCharger;
    [SerializeField]
    public CarSystem target;

    private void Start()
    {
        specialCharger.SetSpecial(this);
    }

    protected override void ExecuteAbility()
    {
        Boost boost = ScriptableObject.CreateInstance<Boost>();

        boost.Setup(boostStrenght, steeringBuff, boostDuration);
        carSystem.boostManager.AddBoost(boost);

        carSystem.sphere.AddForce(Vector3.up * jumpStrenght, ForceMode.Impulse);
    }

    public override void Charge()
    {
        if (resource < maxResource)
        {
            resource += chargePerTick;
            AudioManager.Instance.PlaySfx("rataoSpecialFeedback");

            if (target && target.maxSpeed < carSystem.maxSpeed)
            {
                Boost boost = ScriptableObject.CreateInstance<Boost>();

                float speedDifference = (target.maxSpeed - carSystem.maxSpeed) + 5;

                if (speedDifference < 0) {
                    boost.Setup(speedDifference, 0, 0.04f);
                    carSystem.boostManager.AddBoost(boost);
                }
            }
        }

        CheckIfReady();
    }

    public CarSystem GetCar()
    {
        return carSystem;
    }

    protected override void SfxActivate()
    {
        AudioManager.Instance.PlaySfx("rataoSpecialActivate");
    }

    protected override void SfxReady()
    {
        AudioManager.Instance.PlaySfx("rataoSpecialReady");
    }
}
