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
    private bool lastTimeCheck = false;

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

        CheckIfReady();
    }

    public virtual bool IsAvailable()
    {
        return resource >= maxResource;
    }

    public virtual void Activate()
    {
        if (IsAvailable())
        {
            SfxActivate();
            ExecuteAbility();
            resource = 0;
        } else
        {
            SfxNotReady();
        }
    }

    protected void CheckIfReady()
    {
        bool isready = lastTimeCheck;
        lastTimeCheck = IsAvailable();

        if (isready == false && lastTimeCheck == true )
            SfxReady();
    }

    private void SfxNotReady()
    {
        AudioManager.Instance.PlaySfx("specialNotReady");
    }

    protected abstract void ExecuteAbility(); // The actual ability logic
    protected abstract void SfxActivate();
    protected abstract void SfxReady();
}

