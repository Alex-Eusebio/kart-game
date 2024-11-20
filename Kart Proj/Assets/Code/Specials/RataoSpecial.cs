using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RataoSpecial : SpecialAbility
{
    [Header("Values")]
    [SerializeField]
    private float boostStrenght;
    [SerializeField]
    private float boostDuration;

    protected override void Start()
    {
        carSystem = GetComponent<CarSystem>();
        base.Start();
    }

    protected override void ExecuteAbility()
    {
        Boost boost = ScriptableObject.CreateInstance<Boost>();

        boost.Setup(boostStrenght, 0, boostDuration);
        carSystem.boostManager.AddBoost(boost);
    }

    public override void Charge()
    {
        if (resource < maxResource)
        {
            resource++;

            if (resource == maxResource)
                Debug.Log("Ratao Special is Ready!!");
        }
    }

    public void OnBoostCount()
    {
        Charge();
    }
}
