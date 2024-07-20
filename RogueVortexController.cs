using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueVortexController : MonoBehaviour
{
    private bool enemyAttracted;
    private GameObject player;
    private float microDamageTimer;
    private float dragSpeed = 0.01f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(enemyAttracted)
        {
            player.GetComponent<Rigidbody2D>().gravityScale = 0;
            if(microDamageTimer <= 0)
            {
                player.SendMessage("microDamage", 5);
                microDamageTimer = 0.2f;
            }
            
            if(player.transform.position.x < transform.position.x)
            {
                player.transform.position += new Vector3(1,0,0)*dragSpeed*2;
            }
            else
            {
                player.transform.position += new Vector3(-1,0,0)*dragSpeed*2;
            }

            if(player.transform.position.y < transform.position.y)
            {
                player.transform.position += new Vector3(0,1,0)*dragSpeed*2;
            }
            else
            {
                player.transform.position += new Vector3(0,-1,0)*dragSpeed*4;
            }
        }
         if (microDamageTimer > 0)
        {
            microDamageTimer -= Time.deltaTime;
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            enemyAttracted = true;
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            player.GetComponent<Rigidbody2D>().gravityScale = 1;
            enemyAttracted = false;
        }
    }
    void Destroy()
    {
        Destroy(gameObject);
    }
}
