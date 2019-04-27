using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    private Collider2D interaction; 
    private ElevatorTrack elevatorScript; 
    private ButtonControl buttonScript; 
    public GameObject player; 
    private PlayerController playerScript; 

    void Awake()
    {
        playerScript = player.GetComponent<PlayerController>(); 
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        playerScript.interactionExit(false); 
        interaction = other; 
    }

    void OnTriggerExit2D (Collider2D other)
    {
        playerScript.interactionExit(true); 
    }

    public Collider2D getInteraction()
    {
        return(interaction); 
    }

    public Vector3 getPartner()
    {
        elevatorScript = interaction.GetComponent<ElevatorTrack>(); 
        return(elevatorScript.getPartner()); 
    }

    public bool getFuel()
    {
        buttonScript = interaction.GetComponent<ButtonControl>(); 
        return(buttonScript.fixFuel()); 
    }
}
