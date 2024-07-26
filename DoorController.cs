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
    public static float travelingCoolDown;
   

    
    // Start is called before the first frame update
    void Start()
    {
        buffsHelper = GameObject.FindGameObjectWithTag("BuffsHelper");
        positionToGo = roomDoor.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(travelingCoolDown > 0)
        {
            travelingCoolDown -= Time.deltaTime;
        }
    }
    internal void transportBarry(GameObject barry)
    {
        if(onRoom || (!used && !onRoom))
        {
            if(!onRoom)
            {
                used = true;
                barry.transform.position = positionToGo;
                positionToReturn = transform.position;
                buffsHelper.GetComponent<BuffsOnBarryHelper>().AddBuff();
                travelingCoolDown = 2f;
            }
            else
            {
                if(travelingCoolDown <= 0)
                {
                    barry.transform.position = positionToReturn;
                }
                
            }
        }
        
    }

}
