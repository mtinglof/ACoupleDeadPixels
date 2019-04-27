using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserControl : MonoBehaviour
{
    private int goingLeft; 
    private float horizontalMove = 1f; 
    private EndaController endaScript; 

    public void goLeft(bool left)
    {
        if(left)
        {
            goingLeft = 1; 
        }
        else
        {
            goingLeft = -1; 
        }
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if(other.tag=="enemy")
        {
            int damage = Random.Range(0,15); 
            other.GetComponent<EndaController>().damage(damage); 
            Destroy(gameObject); 
        }
        if((other.GetComponent<Collider2D>().isTrigger == false && other.tag == "doorway") || other.tag == "enddoor" || other.tag == "doorcontrol")
        {
            Destroy(gameObject); 
        }
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x + (horizontalMove*goingLeft), transform.position.y, transform.position.z);
    }
}
