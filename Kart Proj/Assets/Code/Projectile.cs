using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public SphereCollider projectileArea;
    public bool stuns = false;
    public float stunDuration;
    public float slowStrenght;
    public float slowDuration;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarSystem>() != null /*|| other.GetComponent<AiCarSystem>() != null*/)
        {
            if (stuns)
            {
                other.GetComponent<CarSystem>().stunDuration = stunDuration;
            } else
            {
                Boost slow = ScriptableObject.CreateInstance<Boost>();
                slow.Setup(slowStrenght, 0, slowDuration);

                other.GetComponent<CarSystem>().boostManager.AddBoost(slow);
            }

            Destroy(gameObject);
        }
    }
}
