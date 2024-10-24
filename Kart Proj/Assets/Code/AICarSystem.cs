using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//using UnityEngine.Rendering.PostProcessing;
//using Cinemachine;

public class AICarSystem : MonoBehaviour
{
    //private PostProcessVolume postVolume;
    //private PostProcessProfile postProfile;

    public Transform kartModel;
    public Transform kartNormal;
    public Rigidbody sphere;
    SpawnPointManager spawnPointManager;

    //public List<ParticleSystem> primaryParticles = new List<ParticleSystem>();
    //public List<ParticleSystem> secondaryParticles = new List<ParticleSystem>();


    [Header("Car Stats")]
    public float speed;
    public float currentSpeed;
    public float currentSteer;
    float rotate, currentRotate;
    public float driftPower;
    public float minDriftPower;
    public float driftPowerPerLvl;
    public float driftBoostPerLvl;
    public int driftMode = 0;

    bool first, second, third;
    Color c;
    int driftDirection;

    [Header("Bools")]
    public bool drifting;
    public bool isTryingToDrift;
    public bool isGrounded;
    public bool onRoad;

    [Header("Parameters")]

    public float maxSpeed = 30f;
    public float backwardsSpeed = -15f;
    public float acceleration = 1f;
    public float driftSpeedDebuff = 8f;
    public float driftPassiveSteeringMulti = 2f;
    public float steering = 80f;
    public float steeringDriftMulti = 1f;
    public float gravity = 10f;
    public LayerMask layerMask;
    public LayerMask groundMask;

    [Header("Model Parts")]

    public Transform frontWheels;
    public Transform backWheels;
    public Transform steeringWheel;


    [Header("Grounded Stats")]
    public float bonusGravity;
    public float gravityScale;
    public float airBorneTime;
    public float airBorneTimer;

    /*[Header("Particles")]
    public Transform wheelParticles;
    public Transform flashParticles;
    public Color[] turboColors;*/

    float distToGround;

    private void Awake()
    {
        if (!spawnPointManager)
            spawnPointManager = FindObjectOfType<SpawnPointManager>();
    }

    void Start()
    {
        /*postVolume = Camera.main.GetComponent<PostProcessVolume>();
        postProfile = postVolume.profile;*/

        /*for (int i = 0; i < wheelParticles.GetChild(0).childCount; i++)
        {
            primaryParticles.Add(wheelParticles.GetChild(0).GetChild(i).GetComponent<ParticleSystem>());
        }

        for (int i = 0; i < wheelParticles.GetChild(1).childCount; i++)
        {
            primaryParticles.Add(wheelParticles.GetChild(1).GetChild(i).GetComponent<ParticleSystem>());
        }

        foreach (ParticleSystem p in flashParticles.GetComponentsInChildren<ParticleSystem>())
        {
            secondaryParticles.Add(p);
        }*/
    }

    public bool IsGrounded()
    {
        RaycastHit hit;
        float rayLength = 1.1f; // Adjust based on your character's size
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayLength))
        {
            if (hit.collider.gameObject.GetComponent<IChangeSpeed>() != null)
            {
                float speedChange = hit.collider.gameObject.GetComponent<IChangeSpeed>().ChangeSpeed(currentSpeed);
                currentSpeed += speedChange;
                if (speedChange > 0)
                    onRoad = true;
                else 
                    onRoad = false;
            } else
            {
                onRoad = true;
            }
            return true;
        }
        return false;
    }

    void Update()
    {
        //Follow Collider
        transform.position = sphere.transform.position - new Vector3(0, 0.75f, 0);

        //Drift
        if (isTryingToDrift && !drifting && currentSteer != 0)
        {
            drifting = true;
            driftDirection = currentSteer > 0 ? 1 : -1;

            /*foreach (ParticleSystem p in primaryParticles)
            {
                p.startColor = Color.clear;
                p.Play();
            }*/

            kartModel.parent.DOComplete();
            kartModel.parent.DOPunchPosition(transform.up * .2f, .3f, 5, 1);

        }

        if (!isTryingToDrift && drifting)
        {
            Boost();
        }

        //Animations    

        //a) Kart
        if (!drifting)
        {
            kartModel.localEulerAngles = Vector3.Lerp(kartModel.localEulerAngles, new Vector3(0, 90 + (currentSteer * 15), kartModel.localEulerAngles.z), .2f);
        }
        else
        {
            float control = (driftDirection == 1) ? ExtensionMethods.Remap(currentSteer, -1, 1, .5f, 2) : ExtensionMethods.Remap(currentSteer, -1, 1, 2, .5f);
            kartModel.parent.localRotation = Quaternion.Euler(0, Mathf.LerpAngle(kartModel.parent.localEulerAngles.y, (control * 15) * driftDirection, .2f), 0);
        }

        //b) Wheels
        frontWheels.localEulerAngles = new Vector3((currentSteer * 15), frontWheels.localEulerAngles.y, 0);
        frontWheels.localEulerAngles += new Vector3(0, 0, sphere.velocity.magnitude / 2);
        backWheels.localEulerAngles += new Vector3(sphere.velocity.magnitude / 2, 0, 0);

        //c) Steering Wheel
        steeringWheel.localEulerAngles = new Vector3(-25, 90, ((currentSteer * 45)));

    }

    public void ApplyAcceleration(float input)
    {
        if (input > 0)
            speed = maxSpeed;
        else if (input < 0)
            speed = backwardsSpeed;
        
        currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * acceleration); speed = 0f;
        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f); rotate = 0f;
    }

    public void Steer(float steeringSignal)
    {
        currentSteer = steeringSignal;
        if (currentSteer != 0 && !drifting)
        {
            int dir = currentSteer > 0 ? 1 : -1;
            float amount = Mathf.Abs(currentSteer);
            Steer(dir, amount);
        }

        /*int steerDirection = steeringSignal > 0 ? 1 : -1;
        float steeringStrength = Mathf.Abs(steeringSignal);
        rotate = (steering *steerDirection) * steeringStrength;*/
    }

    public void Drift(float input)
    {
        if (input == 1 && currentSteer > 0 || currentSteer < 0)
            isTryingToDrift = true;
        else
            isTryingToDrift = false;
    }

    public void AnimateKart(float input)
    {
        kartModel.localEulerAngles = Vector3.Lerp(kartModel.localEulerAngles, new Vector3(0, 90 + (input * 15), kartModel.localEulerAngles.z), .2f);

        frontWheels.localEulerAngles = new Vector3(0, (input * 15), frontWheels.localEulerAngles.z);
        frontWheels.localEulerAngles += new Vector3(0, 0, sphere.velocity.magnitude / 2);
        backWheels.localEulerAngles += new Vector3(0, 0, sphere.velocity.magnitude / 2);

        steeringWheel.localEulerAngles = new Vector3(-25, 90, ((input * 45)));
    }

    private void FixedUpdate()
    {
        if (!IsGrounded())
        {
            airBorneTimer += Time.deltaTime;

            if (airBorneTimer > airBorneTime)
            {
                bonusGravity += gravityScale * Time.deltaTime;
            }
        } else
        {
            bonusGravity = 0;
            airBorneTimer = 0;
        }

        if (drifting)
        {
            if (speed > 5)
                speed -= driftSpeedDebuff;

            float control = (driftDirection == 1) ? ExtensionMethods.Remap(currentSteer, -1, 1, 0.2f, 2) : ExtensionMethods.Remap(currentSteer, -1, 1, 2, 0.2f);
            float powerControl = (driftDirection == 1) ? ExtensionMethods.Remap(currentSteer, -1, 1, .2f, 1) : ExtensionMethods.Remap(currentSteer, -1, 1, 1, .2f);
            Steer(driftDirection*driftPassiveSteeringMulti, control, steeringDriftMulti);
            driftPower += powerControl;

            ColorDrift();
        }

        currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * acceleration); speed = 0f;

        //Forward Acceleration
        if (!drifting)
            sphere.AddForce(-kartModel.transform.right * currentSpeed, ForceMode.Acceleration);
        else
            sphere.AddForce(transform.forward * currentSpeed, ForceMode.Acceleration);

        //Gravity
        sphere.AddForce(Vector3.down * (gravity+bonusGravity), ForceMode.Acceleration);

        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f); rotate = 0f;

        //Steering
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + currentRotate, 0), Time.deltaTime * 5f);

        RaycastHit hitOn;
        RaycastHit hitNear;

        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out hitOn, 1.1f, layerMask);
        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out hitNear, 2.0f, layerMask);

        //Normal Rotation
        kartNormal.up = Vector3.Lerp(kartNormal.up, hitNear.normal, Time.deltaTime * 8.0f);
        kartNormal.Rotate(0, transform.eulerAngles.y, 0);
    }

    public int GetDriftLevel()
    {
        if (driftPower > minDriftPower && driftPower < driftPowerPerLvl)
        {
            return 1;
        } else if (driftPower > driftPowerPerLvl && driftPower < driftPowerPerLvl * 2)
        {
            return 2;
        }
        else if (driftPower > driftPowerPerLvl * 2)
        {
            return 3;
        } else
        {
            return 0;
        }
    }

    public void Boost()
    {
        drifting = false;

        if (driftMode > 0)
        {
            currentSpeed += driftBoostPerLvl * GetDriftLevel();

            Debug.Log($"{this.gameObject.name} BOOST! {GetDriftLevel()}");
            GetComponent<KartAgent>().AddReward(0.02f * GetDriftLevel());
            //kartModel.Find("Tube001").GetComponentInChildren<ParticleSystem>().Play();
            //kartModel.Find("Tube002").GetComponentInChildren<ParticleSystem>().Play();
        }

        driftPower = 0;
        driftMode = 0;
        first = false; second = false; third = false;

        /*foreach (ParticleSystem p in primaryParticles)
        {
            p.startColor = Color.clear;
            p.Stop();
        }*/

        kartModel.parent.DOLocalRotate(Vector3.zero, .5f).SetEase(Ease.OutBack);

    }

    public void Steer(float direction, float amount, float steeringMulti = 1)
    {
        rotate = ((steering*steeringMulti) * direction) * amount;
    }

    public void ColorDrift()
    {
        if (!first)
            c = Color.clear;

        if (driftPower > 50 && driftPower < 100 - 1 && !first)
        {
            first = true;
            //c = turboColors[0];
            driftMode = 1;

            PlayFlashParticle(c);
        }

        if (driftPower > 100 && driftPower < 150 - 1 && !second)
        {
            second = true;
            //c = turboColors[1];
            driftMode = 2;

            PlayFlashParticle(c);
        }

        if (driftPower > 150 && !third)
        {
            third = true;
            //c = turboColors[2];
            driftMode = 3;

            PlayFlashParticle(c);
        }

        /*foreach (ParticleSystem p in primaryParticles)
        {
            var pmain = p.main;
            pmain.startColor = c;
        }

        foreach (ParticleSystem p in secondaryParticles)
        {
            var pmain = p.main;
            pmain.startColor = c;
        }*/
    }

    void PlayFlashParticle(Color c)
    {
        //GameObject.Find("CM vcam1").GetComponent<CinemachineImpulseSource>().GenerateImpulse();

        /*foreach (ParticleSystem p in secondaryParticles)
        {
            var pmain = p.main;
            pmain.startColor = c;
            p.Play();
        }*/
    }

    public void Respawn()
    {
        Vector3 pos = spawnPointManager.SelectRandomSpawnpoint();
        sphere.MovePosition(pos);
        transform.position = pos - new Vector3(0, 0.4f, 0);
        transform.rotation = Quaternion.identity;
        transform.eulerAngles = new Vector3(0, -90, 0);
    }

    private void Speed(float x)
    {
        currentSpeed = x;
    }

    void ChromaticAmount(float x)
    {
        //postProfile.GetSetting<ChromaticAberration>().intensity.value = x;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(transform.position + transform.up, transform.position - (transform.up * 2));
    //}
}
