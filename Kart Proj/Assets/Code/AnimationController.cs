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

    [Header("Car Parts")]
    [SerializeField]
    private List<Transform> frontWheels;
    [SerializeField]
    private List<Transform> backWheels;
    [SerializeField]
    private Transform steeringWheel;

    [SerializeField]
    private float steering;
    private float smoothTime = 20f;

    float wheelSpinAngle = 0f;

    private void Update()
    {
        if (characterAnimator)
            characterAnimator.SetFloat("Steer", steering);

        if (carAnimator)
            carAnimator.SetFloat("Steer", steering);

        //steeringWheel.localEulerAngles = new Vector3(-25, 90, ((steer * 45)));
    }

    public void ChangeSteer(float steer)
    {
        steering = Mathf.Lerp(steering, steer, Time.deltaTime*smoothTime);
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
