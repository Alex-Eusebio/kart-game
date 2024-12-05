using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RataoSpecialCharger : MonoBehaviour
{
    private RataoSpecial special;
    private bool ready = false;

    public void SetSpecial(RataoSpecial special)
    {
        this.special = special;
    }

    private bool Ignore(Collider other)
    {
        return other.gameObject == special.GetCar().sphere.gameObject;
    }

    private void Start()
    {
        ready = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ready)
        {
            if (Ignore(other))
                return;

            GameObject gb = null;

            if (other.transform.parent && other.transform.parent.GetComponentInChildren<CarSystem>() != null)
                gb = other.transform.parent.GetComponentInChildren<CarSystem>().gameObject;

            if (gb != null)
            {
                special.target = gb.GetComponent<CarSystem>();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (ready)
        {
            if (Ignore(other))
                return;

            if (other.transform.parent && other.transform.parent.GetComponentInChildren<CarSystem>() != null)
            {
                special.target = other.transform.parent.GetComponentInChildren<CarSystem>();
                special.Charge();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Ignore(other))
            return; 
        
        if (other.transform.parent && other.transform.parent.GetComponentInChildren<CarSystem>() != null)
            special.target = null;
    }
}
