using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoostManager : MonoBehaviour
{
    [SerializeField]
    List<Boost> boosts = new List<Boost>();
    CarSystem carSystem;

    private void Start()
    {
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

        Debug.Log(carSystem.currentSpeed + bonusSpeed);
        if (carSystem.currentSpeed + bonusSpeed > 0)
            carSystem.bonusSpeed = bonusSpeed;
        else
            carSystem.bonusSpeed = 0;

        carSystem.bonusSteer = bonusSteer;
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
    }

    private void RemoveBoost(Boost boost)
    {
        boosts.Remove(boost); 
    }
}
