using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpecialAbility : MonoBehaviour, ISpecial
{
    public float resource;
    public float maxResource;
    [SerializeField]
    public bool hasSpecialCharge = false;

    protected private CarSystem carSystem;

    public SpecialAbility()
    {
        resource = 0;
    }

    private void Awake()
    {
        carSystem = GetComponent<CarSystem>();
    }

    public virtual void Charge()
    {
        if (resource > maxResource)
        {
            resource = maxResource;
        }
    }

    public virtual bool IsAvailable()
    {
        return resource >= maxResource;
    }

    public virtual void Activate()
    {
        if (IsAvailable())
        {
            ExecuteAbility();
            resource = 0;
        }
    }

    protected abstract void ExecuteAbility(); // The actual ability logic
}

