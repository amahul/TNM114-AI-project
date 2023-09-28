using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{
    private List<Checkpoint> checkpointList;
    private int nextCheckpointIndex;

    private void Awake()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");

        checkpointList = new List<Checkpoint>();
        foreach(Transform checkpointSingleTransform in checkpointsTransform)
        {
            Checkpoint checkpoint = checkpointSingleTransform.GetComponent<Checkpoint>();

           checkpoint.SetTrackCheckpoints(this);
           checkpointList.Add(checkpoint);
           

        }

        nextCheckpointIndex = 0;
    }

    public void CarThroughCheckpoint(Checkpoint checkpoint)
    {
        Debug.Log(nextCheckpointIndex);
        if(checkpointList.IndexOf(checkpoint) == nextCheckpointIndex)
        {
            //Correct checkpoint index
           // Debug.Log("Correct checkpoint");
            nextCheckpointIndex++;
        }
        else
        {
            //Debug.Log("Wrong checkpoint");
        }
    }

    public void resetCheckpointIndex()
    {
        nextCheckpointIndex = 0;
    }
}
