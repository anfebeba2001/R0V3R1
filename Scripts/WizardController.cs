using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;

        if(direction.x >= 0){
            Debug.Log("Derecha");
            transform.localScale = new Vector3(-1,1,1);
        }
        else{
            Debug.Log("Izquierda");
            transform.localScale = new Vector3(1,1,1);
        }

        float distance = Mathf.Abs(player.transform.position.x - transform.position.x);
    }
}
