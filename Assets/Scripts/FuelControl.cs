using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelControl : MonoBehaviour
{
    public Animator animator; 
    private bool done = false; 

    public bool fixFuel()
    {
        if(!done)
        {
            animator.SetBool("fixed", true); 
            done = true; 
            return(true); 
        }
        else
        {
            return(false); 
        }
    }
}
