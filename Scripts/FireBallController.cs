using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallScript : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    private GameObject player;
    private Vector2 direction;
    private float timeLife = 2f;
    private float damage = 10;
    private float speed = 0.07f;
    private float maxVel = 1f;
    private bool chasing;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update(){
        timeLife -= Time.deltaTime;

        if(timeLife <= 0){
            Destroy(gameObject);
        }

        /*Vector3 playerPositionFixed = player.transform.position;
        playerPositionFixed.z = -20;
        transform.position = playerPositionFixed;*/

        if(chasing){
            Chasing();
        }
        else{

        }
        
    }

    void FixedUpdate()
    {
        rigidbody2D.velocity = direction * speed;
    }

    public void setDirection(Vector2 dir){
        direction = dir;
    }

    private void Chasing(){

        if (transform.position.x < player.transform.position.x)
        {
            transform.position += Vector3.right * 0.01f;
        }
        else{
            transform.position += Vector3.left * 0.01f;
        }

        if (transform.position.y < player.transform.position.y)
        {
            transform.position += Vector3.up * 0.01f;
        }
        else{
            transform.position += Vector3.down * 0.01f;
        }
    }

    private void OnTriggerEnter2D(Collider2D coll) {

        if(coll.gameObject.tag == "Player"){
            coll.GetComponent<BarryController>().microDamage(damage);
            Destroy(gameObject);
        }
        
    }

    public void Deploy(){
        
    }

    
}
