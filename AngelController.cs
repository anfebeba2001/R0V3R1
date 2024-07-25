using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelController : MonoBehaviour
{
    public GameObject tab;
    public GameObject generalMenu;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            tab.SetActive(true);
            generalMenu.SetActive(false);
        }
    }
}
