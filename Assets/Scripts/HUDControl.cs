using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System; 

public class HUDControl : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI enginesText;
    public TextMeshProUGUI youDied;  
    private int health = 100; 
    private int enginesCount = 0; 
    public TextMeshProUGUI warningText; 
    public TextMeshProUGUI tooEarlyText; 
    public bool countDown = false; 
    private float startTime; 

    void Start()
    {
        healthText.text = "Health\n" + health.ToString(); 
        enginesText.text = "Engines\n" + enginesCount.ToString(); 
    }

    public void setHealth(int health)
    {
        healthText.text = "Health " + health.ToString(); 
        if(health<1)
        {
            died(); 
        }
    }

    public void setEngines(int engines)
    {
        enginesText.text = "Engines\n" + engines.ToString(); 
    }     

    void died()
    {
        Time.timeScale = 0; 
        youDied.text = "YOU\nDIED\nGAME OVER"; 
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds (4f);
        tooEarlyText.text = " "; 
    }

    private IEnumerator Waiter()
    {
        yield return new WaitForSeconds(4f); 
        countDown = true; 
        startTime = Time.time;
        warningText.text = " ";  
    }

    public void blastOff()
    {
        warningText.text = "Get back to your Cryo Sleep Pod!"; 
        StartCoroutine(Waiter()); 
    }

    public void tooEarly()
    {
        tooEarlyText.text = "You still need to start " + (3 - enginesCount).ToString() + " engines!"; 
        StartCoroutine(Wait());
    }

    public void youWin()
    {
        Time.timeScale = 0; 
        youDied.text = "YOU\nWIN\n"; 
    }

    void Update()
    {
        if(countDown)
        {
            warningText.text = "Time to Jump\n" + (60.00f - (Time.time - startTime)).ToString(); 
            if((60-(Time.time-startTime)) < 0)
            {
                died(); 
            }
        }
    }

}
