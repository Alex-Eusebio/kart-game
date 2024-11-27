using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder;

public class BoostManagerAI : BoostManager
{
    AICarSystem carSystemAi;

    protected override void Start()
    {
        allBenSpecials = FindObjectsOfType<BenSpecial>();

        carSystem = GetComponent<AICarSystem>();
        carSystemAi = GetComponent<AICarSystem>();
    }

    public override void AddBoost(Boost boost)
    {
        carSystemAi.agent.AddReward(0.01f * boost.duration);
        base.AddBoost(boost);
    }
}
