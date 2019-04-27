using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class TitleScript : MonoBehaviour
{
    public GameObject background; 
    private List<int> floating = new List<int>(); 
    void Awake()
    {
        Instantiate(background, new Vector3(105f, 41f, 0.0f), Quaternion.identity); 
    }

    void Update()
    {
        
    }
    void LateUpdate()
    {
        
    }
}
