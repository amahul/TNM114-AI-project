using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;


public class CarAgent : Agent
{
    // [SerializeField] private Checkpoint trackCheckpoints;
    // [SerializeField] private Transform spawnPosition;

    [SerializeField] private TrackCheckpoints trackCheckpoints;
    [SerializeField] private Transform spawnPosition;

    private CarController carController;
    private Rigidbody rb;

    private void Awake()
    {
        carController = GetComponent<CarController>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        trackCheckpoints.OnCarCorrectCheckpoint += TrackCheckpoints_OnCarCorrectCheckpoint;
        trackCheckpoints.OnCarWrongCheckpoint += TrackCheckpoints_OnCarWrongCheckpoint;
    }

    private void TrackCheckpoints_OnCarWrongCheckpoint(object sender, TrackCheckpoints.CarCheckpointEventArgs e)
    //private void TrackCheckpoints_OnCarWrongCheckpoint(object sender, EventArgs e)
    {
        if (e.carTransform == transform)
        {
            Debug.Log("-1 Wrong");
            AddReward(-0.5f);
            EndEpisode();
        }
    }

    private void TrackCheckpoints_OnCarCorrectCheckpoint(object sender, TrackCheckpoints.CarCheckpointEventArgs e)
    //private void TrackCheckpoints_OnCarCorrectCheckpoint(object sender, EventArgs e)
    {
        if (e.carTransform == transform)
        {
            Debug.Log("+1 Correct");
            AddReward(1f);
        }
    }

    public override void OnEpisodeBegin()
    {
        MaxStep = 1200; //hade han i car videon
        trackCheckpoints.resetCheckpointIndex();
        transform.position = spawnPosition.position + new Vector3(UnityEngine.Random.Range(-3f, 3f), 0, UnityEngine.Random.Range(-3f, +3f));
        transform.forward = spawnPosition.forward;

    }

    public void Update()
    {
        // Car flipped over
        if (transform.up.y < -0.5f)
        {
            // Debug.Log("Negative reward, flipped.");
            AddReward(-0.5f);
            EndEpisode();
        }

        Vector3 velocity = rb.velocity;
        //Debug.Log("Velocity");
        //Debug.Log(velocity);

        //if (velocity.x > 2)
        //{
        //    Debug.Log("Object is moving forward!");
        //    AddReward(+0.1f);
        //}
    }
    public override void CollectObservations(VectorSensor sensor)
    {

        // sensor.AddObservation(0.5f);
        Vector3 checkpointForward = trackCheckpoints.GetNextTransform(transform).transform.forward;
        float directionDot = Vector3.Dot(transform.forward, checkpointForward);
        sensor.AddObservation(directionDot);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        //Debug.Log("Mini neg reward");
        //AddReward(-0.01f); // give agent negative reward for every step
        // Debug.Log(StepCount);

        float forwardAmount = 0f;
        float turnAmount = 0f;

        switch (actions.DiscreteActions[0])
        {

            case 0: forwardAmount = 0f; break;
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
        if (Input.GetKey(KeyCode.DownArrow)) forwardAction = 2;

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
            Debug.Log("COLLISION ENTER");
            // Minus reward
            AddReward(-0.5f);
            EndEpisode();
            // Debug.Log("Negative reward");
        }
        //if (collision.gameObject.tag == "Checkpoint")
        //{
        //    // positive reward
        //    AddReward(+1f);
        //    Debug.Log("Positive reward");
        //}

    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "Wall")
        {
            Debug.Log("COLLISION STAY");
            // Minus reward
            AddReward(-0.01f);
            // Debug.Log("Negative reward");
        }
    }


    //private void OnCollisionStay(Collision collision)
    //{
    //    Debug.Log("ON Collision stay");
    //    if (collision.gameObject.tag == "Wall")
    //    {
    //        // Minus reward
    //        AddReward(-0.1f);
    //        Debug.Log("Negative reward");
    //    }
    //}





}
