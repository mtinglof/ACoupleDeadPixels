using UnityEngine;
using System; 
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class RoomGen : MonoBehaviour
{
    private List <Vector3> wallPositions = new List<Vector3>();
    private List <Vector3> doorPositions = new List<Vector3>(); 
    public GameObject[] wallTiles; 
    public GameObject[] doorTiles; 
    public GameObject[] windowTiles; 
    public GameObject roomBox; 
    public GameObject floorTile; 
    public GameObject engine; 
    public GameObject engineFuel; 
    public GameObject button; 
    public GameObject comp; 
    public GameObject hal; 
    public GameObject cyroPod; 
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
    private float fuelSize; 
    private float buttonSize; 
    private static int doorCount = 0; 
    private static int roomCount = 0; 
    private static int fuelCount = 1; 
    private bool starter; 
    private bool ender; 
    public GameObject mob; 

    void setStart(float start)
    {
        wallRoomStart = start; 
    }

    public void setHeight(float height)
    {
        wallRoomLevel = height; 
    }

    //List of coordinates to place wall sprites in room, tracks where the last wall is placed. creates parent room object, assigns 
    //position as the middle of the room at player height 
    void InitialiseWallList()
    {
        wallPositions.Clear();
        for (int x = 0; x < roomLength+1; x++)
        {
            wallPositions.Add(new Vector3((wallSize*x)+wallRoomStart, wallRoomLevel, 0f)); 
            wallPositions.Add(new Vector3((wallSize*x)+wallRoomStart, wallRoomLevel+wallHeight, 0f)); 
            wallEnd = (wallSize*x)+wallRoomStart; 
        }
        boardHolder = new GameObject("Room" + roomCount).transform; 
        boardHolder.transform.position = wallPositions[roomLength-1]; 
        GameObject instance = GameObject.Find("Room" + roomCount); 
        instance.gameObject.tag = "room"; 
        roomCount++; 
    }

    void genMobs(int count, Vector3 gen)
    {
        int i = Random.Range(1,(int)Mathf.Sqrt(count)+1); 
        for(int x=0; x<i; x++)
        {
            Instantiate(mob, gen, Quaternion.identity); 
        }
    }

    //Four calls to create the coordinate system for the four door locks 
    void InitialiseDoorList()
    {
        doorPositions.Clear();
        doorPositions.Add(new Vector3(floorRoomStart, floorRoomLevel, 0f)); 
        doorPositions.Add(new Vector3(wallEnd+2.1f, floorRoomLevel, 0f)); 
        doorPositions.Add(new Vector3(floorRoomStart, floorRoomLevel+(wallHeight*2)-.2f, 0f)); 
        doorPositions.Add(new Vector3(wallEnd+2.1f, floorRoomLevel+(wallHeight*2)-.2f, 0f)); 
    }

    //sizes door lock, door metal, floor sprite, and wall sprite for more accurate placing 
    void SpriteSizing()
    {
        roomLength = Random.Range(3, 6); 
        toInstantiate = wallTiles[Random.Range(0, wallTiles.Length)]; 
        wallSize = toInstantiate.GetComponent<Renderer>().bounds.size.x;
        floorTileSize = floorTile.GetComponent<Renderer>().bounds.size.x; 
        wallHeight = toInstantiate.GetComponent<Renderer>().bounds.size.y; 
        floorRoomLevel = wallRoomLevel - wallHeight/2.0f; 
        doorHeight = doorTiles[4].GetComponent<Renderer>().bounds.size.y; 
        floorRoomStart = wallRoomStart-(wallSize/2.0f); 
        fuelSize = engineFuel.GetComponent<Renderer>().bounds.size.y; 
        buttonSize = button.GetComponent<Renderer>().bounds.size.y; 
    }

    //function call that builds room, first routine places all wall titles. Creates parent called "Room" 
    void RoomSetup(bool control)
    {
        GameObject instance; 
        GameObject cryoInstance; 
        int windowSlot = Random.Range(0,2); 
        for(int x = 0; x < wallPositions.Count; x++)
        {
            int windChance = Random.Range(0,4); 
            if(windChance == 0 || control)
            {
                instance = Instantiate(windowTiles[windowSlot], wallPositions[x], Quaternion.identity) as GameObject; 
            }
            else
            {
                instance = Instantiate(toInstantiate, wallPositions[x], Quaternion.identity) as GameObject;
                if(roomCount==1)
                {
                    cryoInstance = Instantiate(cyroPod, wallPositions[x], Quaternion.identity) as GameObject; 
                    cryoInstance.transform.SetParent(boardHolder); 

                }
            } 
            instance.transform.SetParent(boardHolder);    
        }
        
        //routine that places bottom floor tiles  
        float start = doorPositions[0].x + floorTileSize; 
        float end = doorPositions[1].x; 
        //Instantiate(roomBox, new Vector3(start, floorRoomLevel, 0f), Quaternion.identity); 
        toInstantiate = floorTile; 
        while(start<end)
        {
            instance = Instantiate(toInstantiate, new Vector3(start, floorRoomLevel, 0f), Quaternion.identity);  
            instance.transform.SetParent(boardHolder); 
            start = start + floorTileSize; 
        }

        //routine that places top ceiling tiles
        start = doorPositions[0].x + floorTileSize; 
        end = doorPositions[1].x; 
        toInstantiate = floorTile; 
        while(start<end)
        {
            instance = Instantiate(toInstantiate, new Vector3(start, floorRoomLevel+(2*wallHeight), 0f), Quaternion.identity);  
            instance.transform.SetParent(boardHolder); 
            start = start + floorTileSize; 
        }


        //routine that places door metal and creates doorway parent. sets doorway parent position to second metal door gameobject placed 
        Transform doorHolder = new GameObject("Doorway" + doorCount).transform;  
        int index = 1; 
        start = (floorRoomLevel+doorHeight)-floorTileSize/2.0f; 
        end = floorRoomLevel+(wallHeight*2); 
        toInstantiate = doorTiles[4];  
        doorHolder.transform.position = new Vector3(floorRoomStart, start, 0f); 
        while(start<end)
        {
            instance = Instantiate(toInstantiate, new Vector3(floorRoomStart, start, 0f), Quaternion.identity); 
            instance.transform.SetParent(doorHolder); 
            index++; 
            start = floorRoomLevel + (doorHeight*index) - floorTileSize/2.0f; 
        }
        instance = Instantiate(doorTiles[0], doorPositions[0], Quaternion.identity); 
        instance.transform.SetParent(doorHolder); 
        instance = Instantiate(doorTiles[1], doorPositions[1], Quaternion.identity); 
        instance.transform.SetParent(doorHolder); 
        GameObject boardInstance = GameObject.Find("Doorway" + doorCount); 
        if(control)
        {
            boardInstance.gameObject.tag = "doorcontrol"; 
        }
        else if(starter)
        {
            boardInstance.gameObject.tag = "enddoor"; 
        }
        else
        {
            boardInstance.gameObject.tag = "doorway"; 
        }
        boardInstance.AddComponent<BoxCollider2D>(); 
        BoxCollider2D boxInstance = boardInstance.GetComponent<BoxCollider2D>(); 
        boxInstance.size = new Vector2 (1.0f, 10.0f); 
        boxInstance.offset = new Vector2 (0f, 3.75f); 
        doorCount++; 

        doorHolder = new GameObject("Doorway" + doorCount).transform; 
        index = 1; 
        start = (floorRoomLevel+doorHeight)-floorTileSize/2.0f; 
        doorHolder.transform.position = new Vector3(wallEnd+2.1f, start, 0f); 
        end = floorRoomLevel+(wallHeight*2); 
        toInstantiate = doorTiles[4]; 
        while(start<end)
        {
            instance = Instantiate(toInstantiate, new Vector3(wallEnd+2.1f, start, 0f), Quaternion.identity); 
            instance.transform.SetParent(doorHolder); 
            index++; 
            start = floorRoomLevel + (doorHeight*index) - floorTileSize/2.0f;  
        }
        instance = Instantiate(doorTiles[2], doorPositions[2], Quaternion.identity); 
        instance.transform.SetParent(doorHolder); 
        instance = Instantiate(doorTiles[3], doorPositions[3], Quaternion.identity); 
        instance.transform.SetParent(doorHolder); 
        boardInstance = GameObject.Find("Doorway" + doorCount); 
        if(ender)
        {
            boardInstance.gameObject.tag = "enddoor"; 
        }
        else
        {
            boardInstance.gameObject.tag = "doorway"; 
        }
        boardInstance.AddComponent<BoxCollider2D>(); 
        boxInstance = boardInstance.GetComponent<BoxCollider2D>(); 
        boxInstance.size = new Vector2 (1.0f, 10.0f); 
        boxInstance.offset = new Vector2 (0f, 3.75f);
        doorCount++; 
    }

    //buids an engine in the selected room 
    void engineSetup()
    {
        int engineIndex = roomLength; 
        GameObject buttonInstance; 
        GameObject instance; 
        if(roomLength%2 == 0)
        {
            engineIndex = roomLength+1; 
        }
        toInstantiate = engine; 
        Instantiate(toInstantiate, wallPositions[engineIndex], Quaternion.identity);
        toInstantiate = engineFuel; 
        instance = Instantiate(toInstantiate, new Vector3(wallPositions[engineIndex].x, wallPositions[1].y-fuelSize/2.0f-.2f, 0f), Quaternion.identity) as GameObject; 
        instance.name = "fuel" + fuelCount; 
        fuelCount++; 
        buttonInstance = button; 
        Instantiate(buttonInstance, new Vector3(wallPositions[engineIndex].x+wallSize/2.0f, wallPositions[0].y-wallSize/2.0f+buttonSize/2.0f, 0f), Quaternion.identity);
    }

    //builds control room 
    void controlSetup()
    {
        Transform controlHolder = new GameObject("controlRoom").transform;  
        GameObject instance; 
        toInstantiate = comp; 
        float toX = toInstantiate.GetComponent<Renderer>().bounds.size.x/2.0f;
        float toY = toInstantiate.GetComponent<Renderer>().bounds.size.y/2.0f;
        for(int x=0; x<wallPositions.Count; x++)
        {
            int controlRoom = wallPositions.Count-2; 
            for(int i=0; i<4; i++)
            {
                int a = Random.Range(0,3); 
                if(controlRoom == x)
                {
                    instance = Instantiate(toInstantiate, new Vector3(wallPositions[x].x-toX, wallPositions[x].y+toY, 0f), Quaternion.identity) as GameObject; 
                    instance.transform.SetParent(controlHolder); 
                    instance = Instantiate(toInstantiate, new Vector3(wallPositions[x].x+toX, wallPositions[x].y+toY, 0f), Quaternion.identity); 
                    instance.transform.SetParent(controlHolder); 
                    instance = Instantiate(toInstantiate, new Vector3(wallPositions[x].x-toX, wallPositions[x].y-toY, 0f), Quaternion.identity); 
                    instance.transform.SetParent(controlHolder); 
                    instance = Instantiate(toInstantiate, new Vector3(wallPositions[x].x+toX, wallPositions[x].y-toY, 0f), Quaternion.identity); 
                    instance.transform.SetParent(controlHolder); 
                    instance = Instantiate(hal, wallPositions[x], Quaternion.identity); 
                    instance.transform.SetParent(controlHolder); 
                    break; 
                }
                else if(a==0)
                {
                    if(i==0)
                    {
                        Instantiate(toInstantiate, new Vector3(wallPositions[x].x-toX, wallPositions[x].y+toY, 0f), Quaternion.identity); 
                    }
                    else if(i==1)
                    {
                        Instantiate(toInstantiate, new Vector3(wallPositions[x].x+toX, wallPositions[x].y+toY, 0f), Quaternion.identity); 
                    }
                    else if(i==2)
                    {
                        Instantiate(toInstantiate, new Vector3(wallPositions[x].x-toX, wallPositions[x].y-toY, 0f), Quaternion.identity); 
                    }
                    else
                    {
                        Instantiate(toInstantiate, new Vector3(wallPositions[x].x+toX, wallPositions[x].y-toY, 0f), Quaternion.identity); 
                    }
                }
            }
        }
    }

    public float SetupRoom(float start, bool engine, bool control, bool starter, bool ender)
    { 
        this.starter = starter; 
        this.ender = ender; 
        setStart(start); 
        SpriteSizing(); 
        InitialiseWallList();
        InitialiseDoorList(); 
        RoomSetup(control);
        if(engine)
        {
            engineSetup(); 
        }
        if(control)
        {
            controlSetup(); 
        }
        if(roomCount!=1)
        {
            genMobs(roomCount+1, wallPositions[2]); 
        }
        return(start+(wallSize*(roomLength+1)));   
    }
}
