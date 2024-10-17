using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator characterAnimator;
    [SerializeField]
    private Animator carAnimator;

    public float steering;

    private void Update()
    {
        characterAnimator.SetFloat("Steer", steering);
        carAnimator.SetFloat("Steer", steering);
    }
}
