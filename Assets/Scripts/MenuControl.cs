using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MenuControl : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame()
    {
        SceneManager.LoadScene(1); 
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }

    public void Instructions()
    {
        SceneManager.LoadScene(2); 
    }

    public void Back()
    {
        SceneManager.LoadScene(0); 
    }

    public void Credits()
    {
        SceneManager.LoadScene(3); 
    }
}
