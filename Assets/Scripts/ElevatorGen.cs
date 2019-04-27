using UnityEngine;
using System; 
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ElevatorGen : MonoBehaviour
{
    private List <Vector3> elevatorPositions = new List<Vector3>();
    private List <int> elevatorOrder; 
    public GameObject[] elevators;
    private int elevatorCount = 5; 
    public GameObject[] first; 

    public void setElevatorPosition(List<Vector3> positions)
    {
        elevatorPositions = positions; 
    }

    void setOrder()
    {
        elevatorOrder = new List<int>(){0, 2, 4, 6, 8, 7, 9, 1, 3, 5}; 
    }

    public void placeElevators()
    {
        setOrder(); 
        GameObject elevator; 
        GameObject elevatorPartner = first[0]; 
        ElevatorTrack elevatorScript; 
        ElevatorTrack partnerScript; 
        bool partner = false; 
        for(int x=0; x<elevatorPositions.Count; x++)
        {
            elevator = Instantiate(elevators[x], elevatorPositions[elevatorOrder[x]], Quaternion.identity) as GameObject;
            elevatorScript = elevator.GetComponent<ElevatorTrack>(); 
            partnerScript = elevatorPartner.GetComponent<ElevatorTrack>(); 
            if(partner)
            {
                partnerScript.setPartner(elevator); 
                elevatorScript.setPartner(elevatorPartner); 
                partner = false; 
            }
            else
            {
                elevatorPartner = elevator;
                partner = true;  
            }
        }
    }

}
