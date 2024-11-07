using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpecial
{
    void Charge(float val);
    void Activate(); // Method to activate the ability
    bool IsAvailable(); // Method to check if ability can be used (e.g., cooldown)
}

