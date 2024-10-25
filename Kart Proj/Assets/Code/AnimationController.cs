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
        if (frontWheels.Count > 0)
        {
            // Apply steering to the FrontWheels parent (Y-axis turning)
            Quaternion steerRotation = Quaternion.Euler(0, 90, 0); // Adjust steering amount (15f)
            // Spin each front wheel based on speed (X-axis for rolling)
            Quaternion spinRotation = Quaternion.Euler(speed * Time.deltaTime * -180f, (steering * 10f), 0);
            foreach (Transform f in frontWheels)
            {
                f.transform.localRotation = steerRotation;
                f.transform.localRotation *= spinRotation;
            }
        }
        
        if (backWheels.Count > 0)
        {
            // Spin each back wheel based on speed (X-axis for rolling)
            Quaternion spinRotation = Quaternion.Euler(speed * Time.deltaTime * -180f, 0, 0);

            foreach (Transform b in frontWheels)
            {
                b.transform.localRotation *= spinRotation;
            }
        }
        
    }
}
