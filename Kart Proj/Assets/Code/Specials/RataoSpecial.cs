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

    protected override void Start()
    {
        carSystem = GetComponent<CarSystem>();
        specialCharger.SetSpecial(this);
        base.Start();
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

            if (target && target.maxSpeed < carSystem.maxSpeed)
            {
                Boost boost = ScriptableObject.CreateInstance<Boost>();

                boost.Setup(target.maxSpeed - carSystem.maxSpeed, 0, 0.05f);
                carSystem.boostManager.AddBoost(boost);
            }

            if (resource == maxResource)
                Debug.Log("Ratao Special is Ready!!");
        }
    }

    public CarSystem GetCar()
    {
        return carSystem;
    }
}
