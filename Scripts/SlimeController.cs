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
            if(player.transform.position.x < transform.position.x + 0.8f && player.transform.position.x > transform.position.x - 0.8f ) 
            {
                transform.localScale = new Vector3(transform.localScale.x,-transform.localScale.y,transform.localScale.x);
                asleep = false;
                GetComponent<Rigidbody2D>().isKinematic = false;
            }
        }
        if(grounded && firstDamageTimer > 0)
        {
            firstDamageTimer -= Time.deltaTime;
        }  
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Ground")
        {
            grounded = true;
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Animator>().Play("SlimeAttack");
        }

        if(coll.gameObject.tag == "Player" && firstDamageTimer > 0)
        {
            coll.gameObject.SendMessage("BarryGotAttacked",30);
        }


    }
    void Hitted(float damage)
    {
        health -= damage;
        hitted = true;
        damageMessagePopUp.GetComponent<TextMeshPro>().text = (damage) + " ";
        health -= (damage);            Instantiate(damageMessagePopUp, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
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