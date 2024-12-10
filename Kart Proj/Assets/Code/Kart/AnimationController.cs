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
    private Animator wheelsAnimator; // Novo Animator para as rodas

    [Header("Car Parts")]
    [SerializeField]
    private List<Transform> frontWheels;
    [SerializeField]
    private List<Transform> backWheels;

    [SerializeField]
    private float steering;
    [SerializeField]
    private float bonusSpeed;
    [SerializeField]
    private bool isOnSpecial = false;
    [SerializeField]
    private bool isStunned = false;

    float wheelSpinAngle = 0f;
    private float smoothTime = 20f;

    private void Update()
    {
        if (characterAnimator)
        {
            characterAnimator.SetBool("IsOnSpecial", isOnSpecial);
            characterAnimator.SetBool("IsStunned", isStunned);
            characterAnimator.SetFloat("Steer", steering);
            characterAnimator.SetFloat("BonusSpeed", bonusSpeed);
        }

        if (carAnimator)
        {
            carAnimator.SetBool("IsOnSpecial", isOnSpecial);
            carAnimator.SetBool("IsStunned", isStunned);
            carAnimator.SetFloat("Steer", steering);
            carAnimator.SetFloat("BonusSpeed", bonusSpeed);
        }

        if (wheelsAnimator) // Atualiza os parâmetros do wheelsAnimator
        {
            wheelsAnimator.SetFloat("Steer", steering);
            wheelsAnimator.SetFloat("BonusSpeed", bonusSpeed);
            wheelsAnimator.SetBool("IsOnSpecial", isOnSpecial);
            wheelsAnimator.SetBool("IsStunned", isStunned);
        }
    }

    public void ChangeSteer(float steer)
    {
        steering = Mathf.Lerp(steering, steer, Time.deltaTime * smoothTime);
    }

    public void UpdateSpecial(bool isOnSpecial)
    {
        this.isOnSpecial = isOnSpecial;
    }

    public void UpdateBonusSpeed(float bonusSpeed)
    {
        float temp = 6f;
        this.bonusSpeed = Mathf.SmoothDamp(this.bonusSpeed, bonusSpeed, ref temp, 0.1f);
    }

    public void UpdateStun(bool isStunned)
    {
        this.isStunned = isStunned;
    }

    public void UpdateWheelsRotation(float speed)
    {
        wheelSpinAngle += speed * Time.deltaTime * -180f;

        if (frontWheels.Count > 0)
        {
            foreach (Transform f in frontWheels)
            {
                f.localEulerAngles = new Vector3(wheelSpinAngle, (steering * 10f), 0);
            }
        }

        if (backWheels.Count > 0)
        {
            foreach (Transform b in backWheels)
            {
                b.localEulerAngles = new Vector3(wheelSpinAngle, 0, 0);
            }
        }
    }
}
