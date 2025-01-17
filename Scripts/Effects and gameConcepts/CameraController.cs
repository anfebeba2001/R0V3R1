using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject mainCamera;
    private GameObject player;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(mainCamera.GetComponent<Camera>().orthographicSize < 3.8f)
        {
            mainCamera.GetComponent<Camera>().orthographicSize += 0.0025f;
        }
        if(mainCamera.GetComponent<Camera>().orthographicSize < 2f)
        {
            mainCamera.GetComponent<Camera>().orthographicSize += 0.025f;
        }
        
        Vector3 playerPositionFixed = player.transform.position;
        playerPositionFixed.z = -20;
        mainCamera.transform.position = playerPositionFixed;
        mainCamera.transform.position += new Vector3(0,1,0)*1.5f;
        

    }
}
