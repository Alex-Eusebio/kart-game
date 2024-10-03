using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class KartAgent : Agent
{
    private CarSystem carSystem;

    public override void Initialize()
    {
        carSystem = GetComponent<CarSystem>();  
    }

    public override void OnEpisodeBegin()
    {
        //add respawn
        //add checkpointmanager
    }

    #region
    //collect extra information that isn't picked up by the RaycastSensors
    public override void CollectObservations(VectorSensor sensor)
    {
        
    }

    //Processing the actions recieved
    public override void OnActionReceived(ActionBuffers actions)
    {
        var input = actions.ContinuousActions;

        //carSystem.ApplyAccelaration <- edit carSystem? maybe
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
