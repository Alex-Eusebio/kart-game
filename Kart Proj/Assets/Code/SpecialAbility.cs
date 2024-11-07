using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpecialAbility : ISpecial
{
    protected float resource;
    protected float maxResource;

    public SpecialAbility(float cooldown)
    {
        maxResource = cooldown;
        resource = 0;
    }

    public virtual void Charge(float val)
    {
        resource += val;

        if (resource > maxResource)
        {
            resource = maxResource;
        }
    }

    public virtual bool IsAvailable()
    {
        return resource == maxResource;
    }

    public void Activate()
    {
        if (IsAvailable())
        {
            ExecuteAbility();
            resource = 0;
        }
    }

    protected abstract void ExecuteAbility(); // The actual ability logic
}

