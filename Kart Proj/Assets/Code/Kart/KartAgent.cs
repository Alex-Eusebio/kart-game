using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;

public class KartAgent : Agent
{
    private AICarSystem carSystem;
    public CheckpointManager checkpointManager;

    public override void Initialize()
    {
        carSystem = GetComponent<AICarSystem>();  
        checkpointManager = carSystem.sphere.gameObject.GetComponent<CheckpointManager>();
    }

    public override void OnEpisodeBegin()
    {
        checkpointManager.ResetCheckpoints();
        carSystem.Respawn();
    }

    #region
    //collect extra information that isn't picked up by the RaycastSensors
    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 diff = checkpointManager.nextCheckPointToReach.transform.position - transform.position;

        sensor.AddObservation(diff/20f);

        if (checkpointManager.nextCheckPointToReach.isOnTurn && carSystem.isTryingToDrift)
            AddReward(0.01f);
        else if (!checkpointManager.nextCheckPointToReach.isOnTurn && carSystem.isTryingToDrift)
            AddReward(-0.08f);
            
        if (!carSystem.onRoad && !carSystem.ignoreRoadSlows)
            AddReward(-0.07f);

        AddReward(-0.001f);
    }

    //Processing the actions recieved
    public override void OnActionReceived(ActionBuffers actions)
    {
        var input = actions.ContinuousActions;

        if (input[1] < 0)
            AddReward(Mathf.Lerp(-0.01f, -0.05f, input[1]));

        carSystem.GetAiInputs(input[1], input[0], Mathf.FloorToInt(input[2]) != 0);
    }

    //For manual testing with human input, the actionsOut defined here will be sent to OnActionsRecieved
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var action = actionsOut.ContinuousActions;

        action[0] = Input.GetAxis("Horizontal"); //Steering
        action[1] = Input.GetAxis("Vertical"); // Acceralating
        action[2] = Convert.ToSingle(Input.GetButton("Jump")); // Drifting
    }
    #endregion
}
