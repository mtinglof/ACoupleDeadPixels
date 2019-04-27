using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftBite : MonoBehaviour
{
    public GameObject enda; 
    private EndaController endaScript; 
    private bool combat = false; 
    public GameObject player; 
    private PlayerController playerScript; 

    void Awake()
    {
        player = GameObject.FindWithTag("player"); 
        endaScript = enda.GetComponent<EndaController>(); 
        playerScript = player.GetComponent<PlayerController>(); 
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if(other.tag == "player" && endaScript.getAlive())
        {
            endaScript.setCombat(true); 
            combat = true;  
            StartCoroutine(bite());
        }
        
    }

    void OnTriggerExit2D (Collider2D other)
    {
        if(other.tag == "player")
        {
            endaScript.setCombat(false); 
            combat = false; 
        }
    }

    IEnumerator bite()
    {
        for(;;)
        {
            if(combat)
            {
                int damage = Random.Range(0,6); 
                playerScript.damage(damage); 
            }
            yield return new WaitForSeconds(1.0f); 
        }
    }
}
