using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    private float maxHealth;
    private float health;
    private GameObject blood;
    private GameObject tears;
    private GameObject damageMessagePopUp;
    private GameObject player;
    private bool asleep;
    private bool grounded;
    private bool hitted;
    private float firstDamageTimer;
    public float bottomLimitFallDown;
    private float fixedBarryCoolDownAttack;

    void Start()
    {
        firstDamageTimer = 1f;
        maxHealth = 200;
        health = maxHealth;
        tears = GetComponent<EnemyController>().getTears();
        blood = GetComponent<EnemyController>().getBlood();
        damageMessagePopUp = GetComponent<EnemyController>().getDamageMessagePopUp();
        asleep = true;
        grounded = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        hitted = GetComponent<EnemyController>().getHitted();
        if(hitted)
        {
            GetComponent<EnemyController>().cancelHitted();
            Hitted(GetComponent<EnemyController>().getDamageReceived());
        }
        if(health <= 0)
        {
            GetComponent<Animator>().Play("SlimeDead");
        }
        if(asleep)
        {
            if(player.transform.position.x < transform.position.x + 0.8f &&
               player.transform.position.x > transform.position.x - 0.8f &&
               player.transform.position.y > transform.position.y - bottomLimitFallDown &&
               player.transform.position.y < transform.position.y ) 
            {
                transform.localScale = new Vector3(transform.localScale.x,-transform.localScale.y,transform.localScale.x);
                asleep = false;
                GetComponent<Rigidbody2D>().gravityScale = 3f;
                GetComponent<Rigidbody2D>().isKinematic = false;
                
            }
        }
        if(grounded && firstDamageTimer > 0)
        {
            firstDamageTimer -= Time.deltaTime;
        }  
        if(!asleep && !grounded)
        {
            if(fixedBarryCoolDownAttack > 0)
            {
                fixedBarryCoolDownAttack -= Time.deltaTime;
                GetComponent<CircleCollider2D>().isTrigger = true;
            }
            else
            {
                GetComponent<CircleCollider2D>().isTrigger = false;
            }
        }
        
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        
    }
    void OnCollisionStay2D(Collision2D coll)
    {
        
        
        if(coll.gameObject.tag == "Ground")
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<CircleCollider2D>().isTrigger = true;
            grounded = true;

            GetComponent<Animator>().Play("SlimeAttack");
        }

        if(coll.gameObject.tag == "Player"  && firstDamageTimer > 0)
        {
            
            if (coll.gameObject.transform.position.x > transform.position.x)
                coll.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right, ForceMode2D.Impulse);
            else
                coll.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left, ForceMode2D.Impulse);
            coll.gameObject.SendMessage("BarryGotAttacked",30);
            
            fixedBarryCoolDownAttack = 0.02f;
        }


    }
    void Hitted(float damage)
    {
        health -= damage;
        hitted = true;
        damageMessagePopUp.GetComponent<TextMeshPro>().text = (damage) + " ";
        health -= (damage);
        Instantiate(damageMessagePopUp, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        Instantiate(blood, transform.position + new Vector3(0, 0f, 0), Quaternion.identity);
    }
    void HittedByBow(float damage)
    {
        Hitted(damage);
    }
    void Die()
    {
       Destroy(gameObject);
       tears.GetComponent<TearsController>().currentValue = 0;
       tears.GetComponent<TearsController>().finalValue = 44;
       Instantiate(tears, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
    }
}