using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControl : MonoBehaviour
{
    public GameObject fuel; 
    public FuelControl fuelScript; 
    private static int fuelCount = 1; 

    void Awake()
    {
        fuel = GameObject.Find("fuel" + fuelCount); 
        fuelCount++; 
        fuelScript = fuel.GetComponent<FuelControl>(); 
    }

    public bool fixFuel()
    {
        return(fuelScript.fixFuel()); 
    }
}
