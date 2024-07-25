using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool onRoom;
    public Vector3 positionToGo;
    private bool used;
    public GameObject roomDoor;
    private static Vector3 positionToReturn;
    private GameObject buffsHelper;
   

    
    // Start is called before the first frame update
    void Start()
    {
        buffsHelper = GameObject.FindGameObjectWithTag("BuffsHelper");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    internal void transportBarry(GameObject gameObject)
    {
        if(onRoom || (!used && !onRoom))
        {
            if(!onRoom)
            {
                used = true;
                gameObject.transform.position = positionToGo;
                positionToReturn = transform.position;
                buffsHelper.GetComponent<BuffsOnBarryHelper>().AddBuff();
            }
            else
            {
                gameObject.transform.position = positionToReturn;
            }
        }
        
    }

}
