using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour{
    public static GameManager instance=null; 
    public BoardManager boardScript; 
    public GameObject player; 
    public AudioClip backgroundSound; 
    public AudioSource backgroundSource; 
    private bool firstBuild = true; 

    void Awake()
    {
        if(instance == null)
            instance = this; 
        else if(instance != this)
            Destroy(gameObject); 
        boardScript = GetComponent<BoardManager>(); 
        backgroundSource.clip = backgroundSound; 
        backgroundSource.Play(); 
    }

    void InitGame()
    {
        boardScript.SetupScene(); 
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1 && firstBuild)
        {
            InitGame(); 
            firstBuild = false; 
        }
    }
}