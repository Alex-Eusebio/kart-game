using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    float time;
    public int maxLaps = 3;

    private void Start()
    {
        LapManager[] carts = FindObjectsOfType<LapManager>();

        foreach (LapManager cart in carts)
        {
            cart.SetMaxLaps(maxLaps);
        }
    }

    private void Update()
    {
        time += Time.deltaTime*1000;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<LapManager>() != null)
        {
            other.GetComponent<LapManager>().Lap(time);
        }
    }
}
