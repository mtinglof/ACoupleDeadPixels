using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class CameraControl : MonoBehaviour
{
    private Transform player; 
    public float smoothSpeed = 5.0f; 
    public Vector3 offset = new Vector3(-1.0f, 3.0f, -2.0f); 
    public Vector3 backOffset = new Vector3(0f, 0f, -3.0f); 
    public GameObject[] backgrounds; 
    private GameObject background; 
    private bool playerExists = false; 

    void Awake()
    {
        background = Instantiate(backgrounds[Random.Range(0, backgrounds.Length)], new Vector3(0f, 3f, 0.0f), Quaternion.identity) as GameObject; 
    }

    void Update()
    {
        while(!playerExists)
        {
            player = GameObject.FindGameObjectWithTag("player").transform;
            if(player != null)
            {
                playerExists = true;
            }
        }
    }
    void LateUpdate()
    {
        Vector3 desiredPosition = player.transform.position + offset; 
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed*Time.deltaTime);
        transform.position = smoothPosition;  
        background.transform.position = smoothPosition + backOffset; 
    }
}
