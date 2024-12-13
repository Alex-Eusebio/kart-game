using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    [SerializeField]
    float curHighLap = 0;
    [SerializeField]
    bool modeA = true;
    [SerializeField]
    private Animator anim;

    public void HandleGateLogic(int lap)
    {
        if (lap > curHighLap && lap != curHighLap)
        {
            curHighLap = lap; 
            
            if (modeA)
            {
                anim.SetTrigger("B");
                modeA = false;
            }
            else
            {
                anim.SetTrigger("A");
                modeA = true;
            }
        }
    }
}
