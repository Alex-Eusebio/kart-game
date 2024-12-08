using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System.Dynamic;
//using UnityEngine.Rendering.PostProcessing;
//using Cinemachine;

public class AICarSystem : CarSystem
{
    public KartAgent agent;
    SpawnPointManager spawnPointManager;
    Vector3 rotatio;
    private void Awake()
    {
        rotatio = gameObject.transform.eulerAngles;
        if (!spawnPointManager)
            spawnPointManager = FindObjectOfType<SpawnPointManager>();
    }

    public void GetAiInputs(float speed, float steer, bool isDrift)
    {
        speedInput = speed;
        steerInput = steer;
        isTryingToDrift = isDrift;
        //usedSpecial = Input.GetButton("Fire1");
    }

    public void Respawn()
    {
        Vector3 pos = spawnPointManager.SelectRandomSpawnpoint();
        sphere.MovePosition(pos);
        transform.position = pos - new Vector3(0, 0.4f, 0);
        transform.rotation = Quaternion.identity;
        transform.eulerAngles = rotatio;
        driftPower = 0;
    }
}
