using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using UnityEngine.Audio; 

public class PlayerController : MonoBehaviour
{
    public Collider2D interaction; 
    private Interaction interactionScript; 
    public Animator animator; 
    private float horizontalMove = 0f; 
    public float horizontalLag = .15f; 
    private bool teleport = false; 
    private Vector3 partnerPosition; 
    public int fixedEngines = 0; 
    public bool coordinates = false; 
    private bool exited = true; 
    private int health; 
    public GameObject leftShot; 
    public GameObject rightShot; 
    public GameObject laser; 
    private GameObject HUD; 
    public AudioClip gunSound; 
    public AudioSource soundSource; 

    void Start()
    {
        interactionScript = interaction.GetComponent<Interaction>(); 
        setHealth(); 
        HUD = GameObject.Find("HUD"); 
    }

    void setHealth()
    {
        health = 100;
    }

    void doSomething()
    {
        interaction = interactionScript.getInteraction(); 
        if(interaction.tag == "doorway")
        {
            if(interaction.GetComponent<BoxCollider2D>().isTrigger == true)
            {
                foreach (Renderer sprite in interaction.GetComponentsInChildren<Renderer>())
                {
                    sprite.enabled = true;
                }
                interaction.GetComponent<BoxCollider2D>().isTrigger = false; 
            }
            else
            {
                foreach (Renderer sprite in interaction.GetComponentsInChildren<Renderer>())
                {
                    sprite.enabled = false;
                }
                interaction.GetComponent<BoxCollider2D>().isTrigger = true;  
            }
        }
        if(interaction.tag == "elevator")
        {
            teleport = true; 
            partnerPosition = interactionScript.getPartner(); 
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false; 
        }
        
        if(interaction.tag == "button")
        { 
            if(interactionScript.getFuel())
            {
                fixedEngines++; 
                HUD.GetComponent<HUDControl>().setEngines(fixedEngines); 
            }
        }

        if(interaction.tag == "doorcontrol")
        {
            if(fixedEngines > 2)
            {
                if(interaction.GetComponent<BoxCollider2D>().isTrigger == true)
                {
                    foreach (Renderer sprite in interaction.GetComponentsInChildren<Renderer>())
                    {
                        sprite.enabled = true;
                    }
                    interaction.GetComponent<BoxCollider2D>().isTrigger = false; 
                }
                else
                {
                    foreach (Renderer sprite in interaction.GetComponentsInChildren<Renderer>())
                    {
                        sprite.enabled = false;
                    }
                    interaction.GetComponent<BoxCollider2D>().isTrigger = true;    
                }
            }
            else
            {
                HUD.GetComponent<HUDControl>().tooEarly(); 
            }
        }

        if(interaction.tag == "hal")
        {
            coordinates = true; 
            HUD.GetComponent<HUDControl>().blastOff(); 
        }

        if(interaction.tag == "pod" && coordinates)
        {
            HUD.GetComponent<HUDControl>().youWin(); 
        }
    }

    public void interactionExit(bool state)
    {
        exited = state; 
    }

    public void damage(int damage)
    {
        health = health - damage; 
        HUD.GetComponent<HUDControl>().setHealth(health); 
    }

    // Update is called once per frame
    void Update()
    {
        //routine that works with player annimation. controls horizontal movement 
        if(!teleport)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal"); 
            if(horizontalMove<0 && horizontalMove != 0)
            {
                animator.SetBool("left", true); 
            }
            if(horizontalMove>0 && horizontalMove != 0)
            {
                animator.SetBool("left", false); 
            }
            animator.SetFloat("speed", horizontalMove); 
        }
        

        if(animator.GetBool("left") && Input.GetButtonDown("Fire1"))
        {
            soundSource.clip = gunSound; 
            soundSource.Play(); 
            animator.SetTrigger("fire2"); 
            Instantiate(laser, leftShot.transform.position, Quaternion.identity).GetComponent<LaserControl>().goLeft(false);
        }
        if(!animator.GetBool("left") && Input.GetButtonDown("Fire1"))
        {
            soundSource.clip = gunSound; 
            soundSource.Play();
            animator.SetTrigger("fire1"); 
            Instantiate(laser, rightShot.transform.position, Quaternion.identity).GetComponent<LaserControl>().goLeft(true);  
        }

        if(Input.GetAxisRaw("Interact") > 0 && Input.GetButtonDown("Interact"))
        {
            if(!exited)
            {
                doSomething();
            } 
        }

        if(teleport)
        {
            Vector3 smoothPosition = Vector3.Lerp(transform.position, partnerPosition, 1);
            transform.position = smoothPosition;  
            if(transform.position == partnerPosition)
            {
                teleport = false; 
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                gameObject.GetComponent<BoxCollider2D>().enabled = true; 
            }
        }
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x + (horizontalMove*horizontalLag), transform.position.y, transform.position.z);
    }
}
