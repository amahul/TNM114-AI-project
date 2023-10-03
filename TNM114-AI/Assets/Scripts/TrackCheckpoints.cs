using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{
    public event EventHandler<CarCheckpointEventArgs> OnCarCorrectCheckpoint;
    public event EventHandler<CarCheckpointEventArgs> OnCarWrongCheckpoint;

    public class CarCheckpointEventArgs : EventArgs
    {

        public Transform carTransform;
    }

    [SerializeField] private List<Transform> carTransformList; // to store multiple cars driving on track
    private List<Checkpoint> checkpointList;
    private List<int> nextCheckpointIndexList;

    private void Awake()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");

        checkpointList = new List<Checkpoint>();
        foreach (Transform checkpointSingleTransform in checkpointsTransform)
        {
            Checkpoint checkpoint = checkpointSingleTransform.GetComponent<Checkpoint>();

            checkpoint.SetTrackCheckpoints(this);
            checkpointList.Add(checkpoint);


        }

        nextCheckpointIndexList = new List<int>();
        foreach (Transform carTransfrom in carTransformList)
        {
            nextCheckpointIndexList.Add(0);
        }
    }

    public void CarThroughCheckpoint(Checkpoint checkpoint, Transform carTransform)
    {
        int nextCheckpointIndex = nextCheckpointIndexList[carTransformList.IndexOf(carTransform)];
        // Debug.Log(nextCheckpointIndex);
        if (checkpointList.IndexOf(checkpoint) == nextCheckpointIndex)
        {
            //Correct checkpoint index
            Debug.Log("Correct checkpoint");
            // nextCheckpointIndex++;
            //  Be able to do multiple laps (or multiple tracks? Is this needed?)
            nextCheckpointIndexList[carTransformList.IndexOf(carTransform)] = (nextCheckpointIndex + 1) % checkpointList.Count;
            OnCarCorrectCheckpoint?.Invoke(this, new CarCheckpointEventArgs { carTransform = carTransform });
            // Called null conditional operator
            // Same as: if(OnCarCorrectCheckpoint != null) OnCarCorrectCheckpoint(this, EventArgs.Empty)

        }
        else
        {
            Debug.Log("Wrong checkpoint");
            OnCarWrongCheckpoint?.Invoke(this, new CarCheckpointEventArgs { carTransform = carTransform });
            //  Checkpoint correctCheckpoint = checkpointList[nextCheckpointIndex];
        }
    }



    public void resetCheckpointIndex()
    {
        foreach (Transform carTransform in carTransformList)
        {
            nextCheckpointIndexList.Add(0);
        }
    }

    //public int getNextTransform(Transform transformArg)
    //{
    //    return nextCheckpointIndexList[carTransformList.IndexOf(transformArg)];
    //}
}
