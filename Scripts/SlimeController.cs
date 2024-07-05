using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    // Start is called before the first frame update
    private float health = 50;
    private bool taunted;
    private bool searching;
    private GameObject playerDetected;
    private float speed = 5;
    private bool attacking;
    private Color normalColor = new Color(1,1,1,1);
    private Color attackingColor = new Color(1.0f, 0.0f, 0.0f, 0.8f);

    private Color searchingColor = new Color(1.0f, 0.0f, 0.0f, 0.18f);
    private Vector3 upAttackingPos;
    private float goingUpTimer;
    private float goingDownTimer;
    private bool goingUp;
    private bool goingDown;
    private float goingDownLimit = 0.06f;
    private float goingUpLimit = 2.5f;
    private Vector3 fixedPlayerPosition;
    private float goingUpVelocity = 0.0075f;
    private float goingDownVelocity = 0.24f;
    private float normalRadius = 0.9f;
    private float tauntedRadius = 1.5f;
    private float damage = 300;
    private float maxVel = 0.3f;
    private bool dying;

    void Start()
    {
        dying = false;
        goingDownTimer = 500;
        goingUpTimer = 500;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            dying = true;
        }
        if(dying)
        {
            GetComponent<Animator>().SetBool("Dying",true);
            GetComponent<Animator>().Play("SlimeDead");
        }
        taunted = GetComponentInParent<EnemyController>().getTaunted();
        if(!taunted && !dying)
        {
            GetComponent<SpriteRenderer>().color = normalColor;
        }
        if (taunted && !searching && !attacking)
        {
            searching = true;
            playerDetected = GetComponentInParent<EnemyController>().getPlayerDetected();
        }
        if (taunted && searching && !attacking )
        {
            GetComponent<SpriteRenderer>().color = searchingColor;
            if (playerDetected.transform.position.x > transform.position.x && GetComponent<Rigidbody2D>().velocity.x < maxVel)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * speed, ForceMode2D.Force);
            }
            else if (playerDetected.transform.position.x < transform.position.x &&  GetComponent<Rigidbody2D>().velocity.x > -maxVel)
            {

                GetComponent<Rigidbody2D>().AddForce(Vector2.left * speed, ForceMode2D.Force);
            }
        }
        if (attacking)
        {        
            
            searching = false;
            GetComponent<SpriteRenderer>().color = attackingColor;
            playerDetected.GetComponent<CapsuleCollider2D>().isTrigger = true;
            transform.localScale = new UnityEngine.Vector3(3, 7.5f, 1);
            GetComponent<Rigidbody2D>().isKinematic = true;
            playerDetected.GetComponent<Rigidbody2D>().isKinematic = true;
            fixedPlayerPosition = transform.position;
            fixedPlayerPosition.y -= 0.25f;
            playerDetected.transform.position = fixedPlayerPosition;
            transform.position = upAttackingPos;


            if (goingUpTimer < goingUpLimit)
            {
                goingUpTimer += Time.deltaTime;
                upAttackingPos.y += goingUpVelocity; 
                goingUp = true;
            }
            else 
            {
                if(goingUp)
                {
                    goingDownTimer = 0;
                      goingUp = false;
                      goingDown = true;
                }
              
            }

            if (goingDownTimer < goingDownLimit && goingDown)
            {
                goingDownTimer += Time.deltaTime;
                upAttackingPos.y -= goingDownVelocity;
            }
            else if(goingDown && !(goingDownTimer < goingDownLimit))
            {
                attacking = false;
                searching = true;
                goingDown = false;
                GetComponent<SpriteRenderer>().color = searchingColor;
                playerDetected.GetComponent<CapsuleCollider2D>().isTrigger = false;
                transform.localScale = new UnityEngine.Vector3(2, 2f, 1);
                GetComponent<Rigidbody2D>().isKinematic = false;
                fixedPlayerPosition = playerDetected.transform.position;
                fixedPlayerPosition.x -= 1.5f;
                playerDetected.transform.position = fixedPlayerPosition;
                playerDetected.SendMessage("BarryGotAttacked",damage);
                
                playerDetected.GetComponent<Rigidbody2D>().isKinematic = false;
                playerDetected.GetComponent<Rigidbody2D>().AddForce(new Vector2(-5,5), ForceMode2D.Impulse);
            }

            
        }
    }
    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            goingUpTimer = 0;
            coll.gameObject.SendMessage("freeze", 3.3f);
            upAttackingPos = playerDetected.transform.position;  
            attacking = true;
            searching = false;
        }
       if (coll.gameObject.tag == "Ground")
       {
            goingDownTimer = 500;
       }
            
    }
    void setRadius(GameObject enemy)
    {
        enemy.GetComponent<EnemyDetector>().setNormalRadius(normalRadius);
        enemy.GetComponent<EnemyDetector>().setTauntedRadius(tauntedRadius);
    }
    void Hitted(float damage)
    {
        health -= damage;
        if(playerDetected.transform.position.x < transform.position.x)
        {
             GetComponent<Rigidbody2D>().AddForce(Vector2.right * 7, ForceMode2D.Impulse);
        }
        else
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.left * 7, ForceMode2D.Impulse);
        }
    }
    void Die()
    {
        Destroy(this.gameObject);
    }
}
