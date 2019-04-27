using UnityEngine;
using System; 
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class HallGen : MonoBehaviour
{
    private List <Vector3> wallPositions = new List<Vector3>();
    private List <Vector3> doorPositions = new List<Vector3>(); 
    public GameObject[] wallTiles; 
    public GameObject[] doorTiles; 
    public GameObject[] windowTiles; 
    public GameObject[] electronics; 
    public GameObject floorTile; 
    private GameObject toInstantiate;  
    private Transform boardHolder; 
    private int roomLength; 
    private float wallRoomStart; 
    private float wallRoomLevel; 
    private float floorRoomStart;  
    private float floorRoomLevel; 
    private float floorTileSize; 
    private float wallSize; 
    private float wallHeight; 
    private float doorHeight; 
    private float wallEnd; 
    private Vector3 elevatorPosition; 
    private static int hallCount = 0; 

    //set start position of hallway
    void setStart(float start)
    {
        wallRoomStart = start; 
    }

    //set height for the hallway
    public void setHeight(float height)
    {
        wallRoomLevel = height; 
    }

    //List of coordinates to place wall sprites in room, tracks where the last wall is placed. Creates parent classes that holds the room 
    //tags the parent, and places the parent in the middle of the hallway
    void InitialiseWallList()
    {
        wallPositions.Clear();
        int elevatorIndex = roomLength/2; 
        for (int x = 0; x < roomLength+1; x++)
        {
            wallPositions.Add(new Vector3((wallSize*x)+wallRoomStart, wallRoomLevel, 0f));  
            wallEnd = (wallSize*x)+wallRoomStart; 
            if(elevatorIndex == x)
            {
                elevatorPosition = wallPositions[x]; 
            }
        }
        boardHolder = new GameObject("Hall" + hallCount).transform; 
        boardHolder.transform.position = wallPositions[roomLength/2]; 
        GameObject instance = GameObject.Find("Hall" + hallCount); 
        instance.gameObject.tag = "hall"; 
        hallCount++; 
    }

    //Four calls to create the coordinate system for the four door locks 
    void InitialiseDoorList()
    {
        doorPositions.Clear();
        doorPositions.Add(new Vector3(floorRoomStart, floorRoomLevel, 0f)); 
        doorPositions.Add(new Vector3(wallEnd+2.1f, floorRoomLevel, 0f)); 
        doorPositions.Add(new Vector3(floorRoomStart, floorRoomLevel+wallHeight, 0f)); 
        doorPositions.Add(new Vector3(wallEnd+2.1f, floorRoomLevel+wallHeight, 0f)); 
    }

    //sizes door lock, door metal, floor sprite, and wall sprite for more accurate placing 
    void SpriteSizing()
    {
        roomLength = Random.Range(4, 7); 
        toInstantiate = wallTiles[Random.Range(0, wallTiles.Length)]; 
        wallSize = toInstantiate.GetComponent<Renderer>().bounds.size.x;
        floorTileSize = floorTile.GetComponent<Renderer>().bounds.size.x; 
        wallHeight = toInstantiate.GetComponent<Renderer>().bounds.size.y; 
        floorRoomLevel = wallRoomLevel - wallHeight/2.0f; 
        doorHeight = doorTiles[4].GetComponent<Renderer>().bounds.size.y; 
        floorRoomStart = wallRoomStart-(wallSize/2.0f); 
    }

    //function call that builds hallways, creates parent called "Room" 
    void RoomSetup()
    {
        GameObject instance; 
        int windowSlot = Random.Range(0,2); 
        for(int x = 0; x < wallPositions.Count; x++)
        {
            int windChance = Random.Range(0,2); 
            if(windChance == 0)
            {
                instance = Instantiate(windowTiles[windowSlot], wallPositions[x], Quaternion.identity) as GameObject; 
            }
            else
            {
                instance = Instantiate(toInstantiate, wallPositions[x], Quaternion.identity) as GameObject;
            }
            instance.transform.SetParent(boardHolder);
            int a = Random.Range(0,2); 
            if(a==0)
            {
                instance = Instantiate(electronics[electronics.Length-1], new Vector3(wallPositions[x].x, wallPositions[x].y+(wallHeight), 0f), Quaternion.identity) as GameObject; 
            }
            else
            {
                instance = Instantiate(electronics[Random.Range(0,electronics.Length)], new Vector3(wallPositions[x].x, wallPositions[x].y+(wallHeight), 0f), Quaternion.identity) as GameObject; 

            }
            instance.transform.SetParent(boardHolder); 
        }
        
        //routine that places bottom floor tiles  
        float start = doorPositions[0].x; 
        float end = doorPositions[1].x + floorTileSize; 
        toInstantiate = floorTile; 
        while(start<end)
        {
            instance = Instantiate(toInstantiate, new Vector3(start, floorRoomLevel, 0f), Quaternion.identity);  
            instance.transform.SetParent(boardHolder); 
            start = start + floorTileSize; 
        }

    //routine that places top ceiling tiles
        start = doorPositions[0].x; 
        end = doorPositions[1].x + floorTileSize; 
        toInstantiate = floorTile; 
        while(start<end)
        {
            instance = Instantiate(toInstantiate, new Vector3(start, floorRoomLevel+(wallHeight), 0f), Quaternion.identity);  
            instance.transform.SetParent(boardHolder); 
            start = start + floorTileSize; 
        }
    }

    public Vector3 getElevatorPosition()
    {
        return(elevatorPosition); 
    }

    public float SetupRoom(float start)
    { 
        setStart(start); 
        SpriteSizing(); 
        InitialiseWallList();
        InitialiseDoorList(); 
        RoomSetup();
        return(start+(wallSize*(1+roomLength)));   
    }
}
