using Google.Protobuf.WellKnownTypes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator characterAnimator;
    [SerializeField]
    private Animator carAnimator;

    [SerializeField]
    private float steering;
    private float smoothTime = 20f;

    private void Update()
    {
        characterAnimator.SetFloat("Steer", steering);
        carAnimator.SetFloat("Steer", steering);
    }

    public void ChangeSteer(float steer)
    {
        steering = Mathf.Lerp(steering, steer, Time.deltaTime*smoothTime);
    }
}
