using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System.Dynamic;
//using UnityEngine.Rendering.PostProcessing;
//using Cinemachine;

public class AICarSystem : CarSystem
{
    public KartAgent agent;

    public void GetAiInputs(float speed, float steer, bool isDrift)
    {
        speedInput = speed;
        steerInput = steer;
        isTryingToDrift = isDrift;
        //usedSpecial = Input.GetButton("Fire1");
    }

    protected override void GetInputs()
    {
        
    }
}
