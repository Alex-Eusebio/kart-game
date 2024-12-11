using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;

public class BoostManager : MonoBehaviour
{
    [SerializeField]
    protected List<Boost> boosts = new List<Boost>();
    protected CarSystem carSystem;
    protected BenSpecial[] allBenSpecials;
    
    protected virtual void Start()
    {
        allBenSpecials = FindObjectsOfType<BenSpecial>();

        carSystem = GetComponent<CarSystem>();
    }

    protected void FixedUpdate()
    {
        ManageBoosts();
        CheckBonusStats();
    }

    protected void CheckBonusStats()
    {
        float bonusSpeed = 0;
        float bonusSteer = 0;
        foreach (Boost boost in boosts)
        {
            if (carSystem.ignoreEnemySlows && boost.bonusSpeed < 0)
                bonusSpeed += 0;
            else
                bonusSpeed += boost.bonusSpeed;
            
            bonusSteer += boost.bonusSteering;
        }

        if (carSystem.currentSpeed + bonusSpeed > 0)
            carSystem.bonusSpeed = bonusSpeed;
        else
        {
            carSystem.bonusSpeed = 0;
            bonusSpeed = 0;
        }

        carSystem.bonusSteer = bonusSteer;

        if (carSystem.transform.parent.GetComponentInChildren<AICarSystem>())
        {
            if (bonusSpeed > 0)
                carSystem.transform.parent.GetComponentInChildren<DebugCanvas>().EnableSpeedLines();
            else
                carSystem.transform.parent.GetComponentInChildren<DebugCanvas>().DisableSpeedLines();
        }

        carSystem.animControll.UpdateBonusSpeed(bonusSpeed);
    }

    protected void ManageBoosts()
    {
        foreach (Boost boost in boosts.ToList())
        {
            boost.UpdateDuration(Time.deltaTime);

            if (boost.IsExpired)
            {
                RemoveBoost(boost);
            }
        }
    }

    public virtual void AddBoost(Boost boost)
    {
        boosts.Add(boost);

        if (allBenSpecials.Count() > 0 && boost.bonusSpeed > 0)
        {
            foreach (BenSpecial special in allBenSpecials)
            {
                if (gameObject.GetComponent<BenSpecial>() != special)
                {
                    GameObject crystal = MonoBehaviour.Instantiate(special.crystal, this.gameObject.transform.position, Quaternion.identity);
                    crystal.GetComponent<BenCrystal>().owner = special.GetComponent<CarSystem>();
                    crystal.gameObject.name = crystal.gameObject.name + " " + special.transform.parent.name;
                }
            }
        }
    }

    protected void RemoveBoost(Boost boost)
    {
        boosts.Remove(boost); 
    }
}
