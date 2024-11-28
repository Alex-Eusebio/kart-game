using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TanksRocketFollow : MonoBehaviour
{
    private MissileProjectile missileProjectile;

    private void Start()
    {
        missileProjectile = this.transform.parent.GetComponentInChildren<MissileProjectile>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarSystem>() != null && missileProjectile.target == null)
        {
            if (other.GetComponent<CarSystem>() != missileProjectile.creator)
            {
                missileProjectile.FoundTarget(other.gameObject);
                Destroy(gameObject);
                transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }
}
