using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirAttack3CollisionDetector : MonoBehaviour
{

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D coll) {
        if((coll.gameObject.tag == "Enemy" || coll.gameObject.tag == "Skeleton" || coll.gameObject.tag == "Ground") && !player.GetComponent<BarryController>().getGrounded() && player.GetComponent<BarryController>().getThirdAttack()){
            StartCoroutine(player.GetComponent<BarryController>().ThirdAirAttackEnd());
        }
    }
}
