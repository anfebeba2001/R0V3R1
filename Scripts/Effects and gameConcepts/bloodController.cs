using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bloodController : MonoBehaviour
{
    void Start()
    {
        GetComponent<Animator>().Play(Random.Range(1, 9) + "");
    }

    // Update is called once per frame
    void Update()
    {
       
        

    }
    void exit()
    {
        Destroy(gameObject);

    }

}