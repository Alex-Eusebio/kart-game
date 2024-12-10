using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System.Dynamic;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;
//using UnityEngine.Rendering.PostProcessing;
//using Cinemachine;

public class CarSystem : MonoBehaviour
{
    public Transform kartModel;
    public Transform kartNormal;
    public Rigidbody sphere;
    [HideInInspector]
    public BoostManager boostManager;
    [SerializeField]
    public SpecialAbility special;

    [Header("Car Stats")]
    public float speed;
    public float bonusSpeed;
    public float currentSpeed;
    float rotate, currentRotate;
    public float bonusSteer;
    public float driftPower;
    public int driftMode = 0; 
    public bool drifting;
    public bool isGrounded;
    public bool onRoad;
    public bool isStunned;
    public float stunDuration;

    Color c;
    int driftDirection;

    [Header("Inputs")]
    public float steerInput;
    public float speedInput;
    public bool isTryingToDrift = false;
    protected bool wasTryingToDrift = false;
    public bool usedSpecial = false;

    [Header("Parameters")]
    public float maxSpeed = 30f;
    public float backwardsSpeed = -15f;
    public float acceleration = 1f;
    public float minDriftPower;
    public float driftPowerPerLvl;
    public float driftBoostPerLvl;
    public float driftBoostDurationPerLvl = 0.7f;
    public float driftSpeedDebuff = 8f;
    public float driftPassiveSteeringMulti = 2f;
    public float steering = 80f;
    public float steeringDriftMulti = 1f;
    public float steeringDebuff = 4f;
    public float gravity = 10f;
    public LayerMask layerMask;
    public LayerMask groundMask;

    [Header("Ignore")]
    public bool ignoreStuns = false;
    public bool ignoreEnemySlows = false;
    public bool ignoreRoadSlows = false;

    [Header("Animator")]
    public AnimationController animControll;

    [Header("Grounded Stats")]
    public float bonusGravity;
    public float gravityScale;
    public float airBorneTime;
    public float airBorneTimer;

    [Header("Particles")]
    public List<ParticleSystem> driftParticles = new List<ParticleSystem>();
    public List<ParticleSystem> sparkParticles = new List<ParticleSystem>();
    public List<ParticleSystem> tubeParticles = new List<ParticleSystem>();
    public Transform wheelParticles;
    public Transform flashParticles;
    public Transform tubeSmokeParticles;
    public Color[] turboColors;

    float distToGround;
    private Quaternion initialRotation;

    void Start()
    {
        boostManager = GetComponent<BoostManager>();

        if (wheelParticles != null && flashParticles != null)
        {
            for (int i = 0; i < wheelParticles.GetChild(0).childCount; i++)
            {
                driftParticles.Add(wheelParticles.GetChild(0).GetChild(i).GetComponent<ParticleSystem>());
            }

            for (int i = 0; i < wheelParticles.GetChild(1).childCount; i++)
            {
                driftParticles.Add(wheelParticles.GetChild(1).GetChild(i).GetComponent<ParticleSystem>());
            }

            foreach (ParticleSystem p in flashParticles.GetComponentsInChildren<ParticleSystem>())
            {
                sparkParticles.Add(p);
            }

            foreach (ParticleSystem p in tubeSmokeParticles.GetComponentsInChildren<ParticleSystem>())
            {
                tubeParticles.Add(p);
            }
        }
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

                if (speedChange > 0)
                    onRoad = true;
                else
                    onRoad = false;

                if (ignoreRoadSlows)
                {
                    if (speedChange > 0)
                    {
                        currentSpeed += speedChange;
                    }
                } else
                {
                    if (currentSpeed + speedChange > 0)
                        currentSpeed += speedChange;
                }
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

        float steer = 0;

        if (!isStunned || ignoreStuns)
        {
            // Handle steering
            steer = steerInput;

            // Start drifting
            if (isTryingToDrift && !drifting && steer != 0)
            {
                StartDrifting(steer);
            }

            // Stop drifting when the button is released
            if (!isTryingToDrift && drifting)
            {
                StopDrifting();
            }

            // Activate special
            if (usedSpecial && special != null)
            {
                special.Activate();
            }
        }

        //Animations    

        //a) Kart
        if (!drifting)
        {
            kartModel.localEulerAngles = Vector3.Lerp(kartModel.localEulerAngles, new Vector3(0, 90 + (steer * 15), kartModel.localEulerAngles.z), .2f);
        }
        else
        {
            float control = (driftDirection == 1) ? ExtensionMethods.Remap(steer, -1, 1, .5f, 2) : ExtensionMethods.Remap(steer, -1, 1, 2, .5f);
            kartModel.parent.localRotation = Quaternion.Euler(0, Mathf.LerpAngle(kartModel.parent.localEulerAngles.y, (control * 15) * driftDirection, .2f), 0);
        }

        if (animControll)
        {
            //b) Wheels
            animControll.UpdateWheelsRotation(currentSpeed+bonusSpeed);

            //c) Steering Wheel

            if (!drifting)
                animControll.ChangeSteer(steer);
            else
                animControll.ChangeSteer(driftDirection);
        }
    }

    public void RespawnCar()
    {
        if (sphere.GetComponent<LapManager>().lastCheckpoint)
        {
            sphere.position = sphere.GetComponent<LapManager>().lastCheckpoint.transform.position;
            transform.rotation = sphere.GetComponent<LapManager>().lastCheckpoint.transform.rotation;
            currentSpeed = 0;
        }
    }

    // Start drifting
    private void StartDrifting(float steer)
    {
        drifting = true;
        driftDirection = steer > 0 ? 1 : -1;
        wasTryingToDrift = true; // Update the previous state

        // Example: Play effects or animations for drifting
        kartModel.parent.DOComplete();
        kartModel.parent.DOPunchPosition(transform.up * .2f, .3f, 5, 1);
    }

    // Stop drifting
    private void StopDrifting()
    {
        drifting = false; // Stop the drifting state
        wasTryingToDrift = false; // Reset the previous state
        Boost(); // Apply the boost mechanic
    }

    private void FixedUpdate()
    {
        if (ignoreStuns)
        {
            stunDuration = 0;
        }

        if (stunDuration > 0)
        {
            isStunned = true;
            animControll.UpdateStun(isStunned);
            stunDuration -= Time.deltaTime;
        }

        if (stunDuration <= 0)
        {
            isStunned = false;
            animControll.UpdateStun(isStunned);
        }

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

        float steer = 0;

        if (!isStunned || ignoreStuns)
        {
            steer = steerInput;

            //Accelerate
            if (speedInput > 0)
                speed = maxSpeed;
            else if (speedInput < 0)
            {
                speed = backwardsSpeed;
            }
        }

        //Steer
        if (steer != 0 && !drifting)
        {
            int dir = steer > 0 ? 1 : -1;
            float amount = Mathf.Abs(steer);
            float debuffValue = Mathf.Lerp(0, steeringDebuff, amount);
            Steer(dir, amount);
            if (speed > steeringDebuff + 2)
                speed -= debuffValue;
        }
        
        if (drifting)
        {
            if (speed > 5)
                speed -= driftSpeedDebuff;

            float control = (driftDirection == 1) ? ExtensionMethods.Remap(steer, -1, 1, 0.2f, 2) : ExtensionMethods.Remap(steer, -1, 1, 2, 0.2f);
            float powerControl = (driftDirection == 1) ? ExtensionMethods.Remap(steer, -1, 1, .2f, 1) : ExtensionMethods.Remap(steer, -1, 1, 1, .2f);
            Steer(driftDirection*driftPassiveSteeringMulti, control, steeringDriftMulti);
            driftPower += powerControl;

            ColorDrift();
        }

        currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * acceleration); speed = 0f;

        //Forward Acceleration
        if (!drifting)
            sphere.AddForce(-kartModel.transform.right * (currentSpeed+bonusSpeed), ForceMode.Acceleration);
        else
            sphere.AddForce(transform.forward * (currentSpeed+bonusSpeed), ForceMode.Acceleration);

        //Gravity
        sphere.AddForce(Vector3.down * (gravity + bonusGravity), ForceMode.Acceleration);

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

        if (special && !special.hasSpecialCharge)
            special.Charge();
    }

    public int GetDriftLevel()
    {
        if (driftPower >= minDriftPower && driftPower < driftPowerPerLvl)
        {
            return 1;
        } else if (driftPower >= driftPowerPerLvl && driftPower < driftPowerPerLvl * 2)
        {
            return 2;
        }
        else if (driftPower >= driftPowerPerLvl * 2)
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
            Boost boost = ScriptableObject.CreateInstance<Boost>();

            boost.Setup(driftBoostPerLvl*driftMode, 0, driftBoostDurationPerLvl*driftMode);
            boostManager.AddBoost(boost);

            if (tubeParticles.Count > 0)
            {
                foreach (ParticleSystem particle in tubeParticles)
                {
                    particle.Play();
                }
            }
        }

        driftPower = 0;
        driftMode = 0;

        if (driftParticles.Count > 0) 
            foreach (ParticleSystem p in driftParticles)
            {
                p.Stop();
                p.Clear();
            }

        kartModel.parent.DOLocalRotate(Vector3.zero, .5f).SetEase(Ease.OutBack);

    }

    public void Steer(float direction, float amount, float steeringMulti = 1)
    {
        rotate = ((steering*steeringMulti) * direction) * amount;
    }

    public void ColorDrift()
    {
        int temp = driftMode;
        driftMode = GetDriftLevel();

        if (turboColors.Length > 0)
            c = turboColors[driftMode];

        if (driftMode > 0 && (temp != driftMode))
            PlayFlashParticle(c);

        if (driftParticles.Count > 0)
        {
            foreach (ParticleSystem p in driftParticles)
            {
                var pmain = p.main;
                pmain.startColor = c;
                p.Play();
            }
        }
    }

    void PlayFlashParticle(Color c)
    {
        if (sparkParticles.Count > 0)
            foreach (ParticleSystem p in sparkParticles)
            {
                var pmain = p.main;
                pmain.startColor = c;
                p.Play();
            }
    }
}
