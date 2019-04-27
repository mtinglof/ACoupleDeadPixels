using UnityEngine;
using System; 
using System.Collections;
using System.Collections.Generic; 
using Random = UnityEngine.Random;  

public class BoardManager : MonoBehaviour{

    public GameObject roomObj; 
    private RoomGen roomScript; 
    public GameObject hallObj; 
    private HallGen hallScript; 
    public GameObject eleObj; 
    private ElevatorGen elevatorScript; 
    private List<Vector3> elevatorPosition = new List<Vector3>(); 
    private List<int> elevatorList = new List<int>(); 
    private List<int> engineList = new List<int>(); 
    private int numRoom; 
    private int levels = 5;
    private float height = 2.5f;  
    private int engineCount = 3; 
    private int controlLevel; 
    
    void setupEngines()
    {
        engineList.Clear(); 
        for(int x=0; x<engineCount; x++)
        {
            int a = Random.Range(1,levels); 
            while(engineList.Contains(a))
            {
                a = Random.Range(1,levels); 
            }
            engineList.Add(a); 
        }
    }

    public void SetupScene()
    {
        setupEngines(); 
        controlLevel = Random.Range(1,levels); 
        roomScript = roomObj.GetComponent<RoomGen>(); 
        hallScript = hallObj.GetComponent<HallGen>(); 
        elevatorScript = eleObj.GetComponent<ElevatorGen>(); 
        bool start = false; 
        float end = 0.0f;
        int a; 
        int b; 
        int c = -1; 
        for(int j=0; j < levels; j++)
        {
            roomScript.setHeight(height); 
            hallScript.setHeight(height); 
            numRoom = Random.Range(2, 4);  
            a =  Random.Range(0,numRoom);
            elevatorList.Add(a); 
            b = a; 
            while(a==b)
            {
                b = Random.Range(0,numRoom); 
            }
            elevatorList.Add(b); 
            end = 0.0f; 
            if(engineList.Contains(j))
            {
                c = Random.Range(0,numRoom);
            }
            else
            {
                c = -1; 
            }
            for(int x=0; x < numRoom; x++)
            {
                start = false;  
                if(x==0)
                {
                    start = true; 
                }
                if(x==c)
                {
                    end = roomScript.SetupRoom(end, true, false, start, false);
                }
                else
                {
                    end = roomScript.SetupRoom(end, false, false, start, false);
                }
                if(elevatorList.Contains(x))
                {
                    end = hallScript.SetupRoom(end); 
                    elevatorPosition.Add(hallScript.getElevatorPosition()); 
                }
                else
                {
                    end = hallScript.SetupRoom(end); 
                }
            }
            if(controlLevel == j)
            {
                roomScript.SetupRoom(end, false, true, start, true); 
            }
            else
            {
                roomScript.SetupRoom(end, false, false, start, true); 
            }
            height = height + 10; 
            elevatorList.Clear(); 
        }
        elevatorScript.setElevatorPosition(elevatorPosition); 
        elevatorScript.placeElevators(); 
    }
}
