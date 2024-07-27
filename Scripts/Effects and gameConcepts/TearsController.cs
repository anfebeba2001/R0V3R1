using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public class TearsController : MonoBehaviour
{
    public GameObject text;
    private GameObject player;
    private float goingUpTimer;
    public int currentValue;
    public int finalValue;
    private float justShowingTimer;

    // Start is called before the first frame update
    void Start()
    {
        currentValue = 0;
        text.GetComponent<TextMeshPro>().text = "" + currentValue;
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<BarryController>().addTears(finalValue);
        goingUpTimer = 3f;
        justShowingTimer = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(goingUpTimer > 0)
        {
            goingUpMethod();
        }
        if(currentValue < finalValue)
        {
            currentValue += 1;
            if(currentValue < finalValue - 1500)
            {
                currentValue += 15;
            }
            if(currentValue < finalValue - 150)
            {
                currentValue += 7;
            }
            text.GetComponent<TextMeshPro>().text = "" + currentValue;
        }
        else
        {
            
            if(justShowingTimer > 0f)
            {
                justShowingTimer -= Time.deltaTime;
            }
            else
            {
                
                Destroy(gameObject);
            }
        }

        
        
    }

    private void goingUpMethod()
    {
        goingUpTimer -= Time.deltaTime;
        text.transform.position += new UnityEngine.Vector3(0.005f,0.003f,0);
        text.transform.localScale += new UnityEngine.Vector3(0.0015f,0.0015f,0);
    }
    
}
