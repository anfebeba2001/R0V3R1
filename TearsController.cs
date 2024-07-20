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
    private bool goingUp;
    private float goingUpTimer;
    private bool justShowing;
    private int currentValue;
    public int finalValue;
    private float justShowingTimer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        goingUp = true;
        goingUpTimer = 3f;
        justShowing = false;
        text.transform.position = transform.position;
        text.transform.localScale = new UnityEngine.Vector3(0,0,0);
        justShowingTimer = 3;
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
            currentValue += 2;
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
            if(!justShowing)
            {
                justShowing = true;
            }
        }
        if(justShowing && justShowingTimer > 0f)
        {
            justShowingTimer -= Time.deltaTime;
        }
        else
        {
           player.GetComponent<BarryController>().addTears(finalValue);
           Destroy(gameObject);
        }
    }

    private void goingUpMethod()
    {
        goingUpTimer -= Time.deltaTime;
        text.transform.position += new UnityEngine.Vector3(0.005f,0.003f,0);
        text.transform.localScale += new UnityEngine.Vector3(0.0015f,0.0015f,0);
    }
    
}
