using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrack : MonoBehaviour
{
    public GameObject partner; 
    private ElevatorTrack elevatorScript; 

    public void setPartner(GameObject partner)
    {
        this.partner = partner; 
        elevatorScript = this.partner.GetComponent<ElevatorTrack>(); 
    }
    public Vector3 getPartner()
    {
        return(elevatorScript.myPosition()); 
    }

    public Vector3 myPosition()
    {
        return(transform.position); 
    }
}
