using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxSpecial : SpecialAbility
{
    [Header("Values")]
    [SerializeField]
    private float boostStrenght;
    [SerializeField]
    private float maxBattery;
    [SerializeField]
    private float chargingCap = 10;
    [SerializeField]
    private float driftPowerConvertPer = 0.2f;

    private void Awake()
    {
        maxResource = maxBattery;
    }

    protected override void ExecuteAbility()
    {
        carSystem.currentSpeed += boostStrenght; 
    }

    public override void Charge()
    {
        if (carSystem.drifting)
        {
            float charge = carSystem.driftPower * driftPowerConvertPer;

            if (charge > chargingCap)
            {
                charge = chargingCap;
            }

            resource += charge;
        }

        if (resource > maxResource)
        {
            resource = maxResource;
            Debug.Log("Max Special is Ready!!");
        }
    }
}
