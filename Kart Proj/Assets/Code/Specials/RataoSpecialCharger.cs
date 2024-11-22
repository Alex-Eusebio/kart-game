using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RataoSpecialCharger : MonoBehaviour
{
    private RataoSpecial special;

    public void SetSpecial(RataoSpecial special)
    {
        this.special = special;
    }

    private bool Ignore(Collider other)
    {
        return other.gameObject == special.GetCar().sphere.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Ignore(other))
            return;

        GameObject gb = null;

        if (other.transform.parent.GetComponentInChildren<CarSystem>() != null)
            gb = other.transform.parent.GetComponentInChildren<CarSystem>().gameObject;

        if (gb != null)
        {
            special.target = gb.GetComponent<CarSystem>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Ignore(other))
            return;

        GameObject gb = null;

        if (other.transform.parent.GetComponentInChildren<CarSystem>() != null)
            gb = other.transform.parent.GetComponentInChildren<CarSystem>().gameObject;

        if (gb.GetComponent<CarSystem>() != null)
        {
            special.target = gb.GetComponent<CarSystem>();
            special.Charge();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Ignore(other))
            return; 
        
        GameObject gb = null;

        if (other.transform.parent.GetComponentInChildren<CarSystem>() != null)
            gb = other.transform.parent.GetComponentInChildren<CarSystem>().gameObject;

        if (gb.GetComponent<CarSystem>() != null)
        {
            special.target = null;
        }
    }
}
