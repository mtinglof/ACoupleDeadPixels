using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System; 
using Random = UnityEngine.Random;

public class EndaController : MonoBehaviour
{
    private bool sameLevel = false; 
    public GameObject floatingHealth;
    public TextMesh healthText; 
    public GameObject leftDoor; 
    public GameObject rightDoor; 
    private GameObject[] doors; 
    private GameObject player; 
    private float smoothSpeed; 
    private Vector3 oldPosition; 
    public Animator animator; 
    public bool inCombat; 
    private PlayerController playerScript; 
    private int health; 
    private bool alive = true; 
    public Sprite[] dead; 
    public SpriteRenderer sprity; 
    public BoxCollider2D collider; 

    // Start is called before the first frame update
    void Awake()
    {
        getDoors(); 
        player = GameObject.FindGameObjectWithTag("player"); 
        playerScript = player.GetComponent<PlayerController>(); 
        health = 100; 
        smoothSpeed = Random.Range(5,13)*.1f; 
        healthText = floatingHealth.GetComponent<TextMesh>(); 
        floatingHealth.GetComponent<MeshRenderer> ().sortingLayerName = "entity";
        healthText.text = health.ToString(); 
    }

    void checkLevel()
    {
        if(player.transform.position.y+2>transform.position.y && player.transform.position.y-1<transform.position.y)
        {
            sameLevel = true;  
        }
        else
        {
            sameLevel = false; 
        }
    }

    void openRoom()
    {
        if((leftDoor.GetComponent<BoxCollider2D>().isTrigger == true || rightDoor.GetComponent<BoxCollider2D>().isTrigger == true) && sameLevel && !inCombat)
        {
            oldPosition = transform.position; 
            transform.position = Vector3.Lerp(transform.position, player.transform.position, smoothSpeed*Time.deltaTime);
            animator.SetFloat("speed", 1); 
            if(oldPosition.x > transform.position.x)
            {
                animator.SetBool("left", true); 
            }
            else
            {
                animator.SetBool("left", false); 
            }
        }
        else
        {
            animator.SetFloat("speed", 0); 
        }
    }

    void getDoors()
    {
        doors = GameObject.FindGameObjectsWithTag("doorway"); 
        for(int x =0; x<doors.Length; x++)
        {
            if(doors[x].transform.position.y+2>transform.position.y && doors[x].transform.position.y-1<transform.position.y) 
            {
                if(doors[x].transform.position.x < rightDoor.transform.position.x && doors[x].transform.position.x > transform.position.x)
                {
                    rightDoor = doors[x]; 
                }
                if(doors[x].transform.position.x > leftDoor.transform.position.x && doors[x].transform.position.x < transform.position.x)
                {
                    leftDoor = doors[x]; 
                }
            }
        }
    }

    public void setCombat(bool combat)
    {
        inCombat = combat; 
        if(inCombat && alive)
        {
            animator.SetBool("bite", true);
        }
        else
        {
            animator.SetBool("bite", false); 
        }
    }

    public void damage(int damage)
    {
        health = health - damage; 
        animator.SetFloat("health", health); 
        healthText.text = health.ToString(); 
        if(health<0)
        {
            healthText.text = " "; 
            die(); 
        }
    }

    public bool getAlive()
    {
        return(alive); 
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds (3f);
        UnityEngine.Object.Destroy(gameObject); 
    }
    void die()
    {
        alive = false; 
        collider.size = new Vector2(collider.size.x, 4); 
        collider.offset = new Vector2(collider.offset.x, 0);  
        animator.enabled = false; 
        if(animator.GetBool("left"))
        {
            sprity.sprite = dead[0]; 
        }
        else
        {
            sprity.sprite = dead[1]; 
        }
        StartCoroutine(Wait()); 
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.IgnoreLayerCollision(10,12); 
        Physics2D.IgnoreLayerCollision(10,10); 
        checkLevel(); 
        openRoom(); 
    }
}
