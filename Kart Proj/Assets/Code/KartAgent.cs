using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class KartAgent : Agent
{
    private CarSystem carSystem;
    public CheckpointManager checkpointManager;

    public override void Initialize()
    {
        carSystem = GetComponent<CarSystem>();  
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

        if (!carSystem.onRoad)
            AddReward(-0.0005f);

        AddReward(-0.001f);
    }

    //Processing the actions recieved
    public override void OnActionReceived(ActionBuffers actions)
    {
        var input = actions.ContinuousActions;

        carSystem.ApplyAcceleration(input[1]);

        carSystem.Steer(input[0]);

        //if (input[2] == 1 && input[0] > 0 || input[0] < 0)
            //carSystem.Drift(input[0]);
    }

    //For manual testing with human input, the actionsOut defined here will be sent to OnActionsRecieved
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var action = actionsOut.ContinuousActions;

        action[0] = Input.GetAxis("Horizontal"); //Steering
        action[1] = Input.GetAxis("Vertical"); // Acceralating
    }
    #endregion
}
