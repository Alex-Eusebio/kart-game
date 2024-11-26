using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;

public class BoostManagerAI : BoostManager
{
    AICarSystem carSystemAi;

    protected override void Start()
    {
        allBenSpecials = FindObjectsOfType<BenSpecial>();

        carSystemAi = GetComponent<AICarSystem>();
    }

    protected override void CheckBonusStats()
    {
        float bonusSpeed = 0;
        float bonusSteer = 0;
        foreach (Boost boost in boosts)
        {
            bonusSpeed += boost.bonusSpeed;
            bonusSteer += boost.bonusSteering;
        }

        if (carSystemAi.currentSpeed + bonusSpeed > 0)
            carSystemAi.bonusSpeed = bonusSpeed;
        else
        {
            carSystemAi.bonusSpeed = 0;
            bonusSpeed = 0;
        }

        carSystemAi.bonusSteer = bonusSteer;

        carSystemAi.animControll.UpdateBonusSpeed(bonusSpeed);
    }

}
