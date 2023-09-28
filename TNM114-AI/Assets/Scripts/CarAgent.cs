using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CarAgent : Agent
{
   // [SerializeField] private Checkpoint trackCheckpoints;
   // [SerializeField] private Transform spawnPosition;

    private CarController carController;
    [SerializeField] TrackCheckpoints trackCheckpoint;
    [SerializeField] private Transform spawnPosition;

    private void Awake()
    {
        carController = GetComponent<CarController>();
    }
    public override void OnEpisodeBegin()
    {
        MaxStep = 1000;
        trackCheckpoint.resetCheckpointIndex();
        transform.position = spawnPosition.position + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, +3f));
        transform.forward = spawnPosition.forward;
    }

    public void Update()
    {
        // Car flipped over
        if(transform.up.y < -0.5f)
        {
            AddReward(-1f);
            EndEpisode();
        }
    }
    //public override void CollectObservations(VectorSensor sensor)
    //{

    //    sensor.AddObservation(0.5f);
    //}
    public override void OnActionReceived(ActionBuffers actions)
    {
        AddReward(-0.01f); // give agent negative reward for every step
       // Debug.Log(StepCount);
        float forwardAmount = 0f;
        float turnAmount = 0f;
        
        switch (actions.DiscreteActions[0])
        {
            
            case 0: forwardAmount = 0f;  break;
            case 1: forwardAmount = +1f; break;
            case 2: forwardAmount = -1f; break;
        }
        switch (actions.DiscreteActions[1])
        {
            case 0: turnAmount = 0f; break;
            case 1: turnAmount = +1f; break;
            case 2: turnAmount = -1f; break;
        }
        carController.SetInputs(forwardAmount, turnAmount);
        //Debug.Log(actions.ContinuousActions[0]);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int forwardAction = 0;
        if (Input.GetKey(KeyCode.UpArrow)) forwardAction = 1;
        if(Input.GetKey(KeyCode.DownArrow)) forwardAction = 2;

        int turnAction = 0;
        if (Input.GetKey(KeyCode.RightArrow)) turnAction = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) turnAction = 2;

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = forwardAction;
        discreteActions[1] = turnAction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            // Minus reward
            AddReward(-0.5f);
            //Debug.Log("Negative reward");
        }
        if (collision.gameObject.tag == "Checkpoint")
        {
            // positive reward
            AddReward(+1f);
            //Debug.Log("Positive reward");
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            // Minus reward
            AddReward(-0.1f);
           // Debug.Log("Negative reward");
        }
    }



    

}
