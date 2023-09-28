using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private TrackCheckpoints trackCheckpoints;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CarController>(out CarController carController))
        {
            trackCheckpoints.CarThroughCheckpoint(this);
        }
    }

    public void SetTrackCheckpoints(TrackCheckpoints trackCheckpoints)
    {
        this.trackCheckpoints = trackCheckpoints;
    }
}
