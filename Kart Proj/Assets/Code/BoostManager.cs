using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;

public class BoostManager : MonoBehaviour
{
    [SerializeField]
    List<Boost> boosts = new List<Boost>();
    CarSystem carSystem;
    BenSpecial[] allBenSpecials;

    private void Start()
    {
        allBenSpecials = FindObjectsOfType<BenSpecial>();

        carSystem = GetComponent<CarSystem>();
    }

    private void FixedUpdate()
    {
        ManageBoosts();
        CheckBonusStats();
    }

    private void CheckBonusStats()
    {
        float bonusSpeed = 0;
        float bonusSteer = 0;
        foreach (Boost boost in boosts)
        {
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

        carSystem.animControll.UpdateBonusSpeed(bonusSpeed);
    }

    void ManageBoosts()
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

    public void AddBoost(Boost boost)
    {
        boosts.Add(boost);

        if (allBenSpecials.Count() > 0)
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

    private void RemoveBoost(Boost boost)
    {
        boosts.Remove(boost); 
    }
}
