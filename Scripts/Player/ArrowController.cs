using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private float damage;
    private float timeLife = 0.3f;
    private Vector2 direction;
    private float speed = 10.0f;
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timeLife -= Time.deltaTime;

        if(timeLife <= 0){
            Destroy(gameObject);
        }
    }

    private void FixedUpdate() {
        rigidBody2D.velocity = direction * speed;
    }

    public void SetDirection(Vector2 dir){
        direction = dir;
    }

    public void setDamage(float dmg){
        damage = dmg;
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag != "Player" && (coll.gameObject.tag == "Enemy" || coll.gameObject.tag == "Skeleton"))
        {
            coll.gameObject.SendMessage("HittedByBow", damage);
            Destroy(gameObject);
        }
    }
}
