using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UndeadController : MonoBehaviour
{
    private float maxHealth;
    private float health;
    private GameObject blood;
    private GameObject damageMessagePopUp;
    private bool hitted;
    public bool taunted;

    public bool walking;
    public bool attacking;
    public float idleTimer;

    public float minLimitPos;
    public float maxLimitPos;
    private GameObject playerDetected;
    private GameObject tears;
    
    private float normalRadius = 5;
    private float tauntedRadius = 5;
    private int amountOfTearsToDrop = 387;


    // Start is called before the first frame update
    void Start()
    {
        tears = GetComponent<EnemyController>().getTears();
        maxHealth = 200;
        health = maxHealth;
        blood = GetComponent<EnemyController>().getBlood();
        damageMessagePopUp = GetComponent<EnemyController>().getDamageMessagePopUp();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!walking)
        {
            if(idleTimer > 0)
            {
                idleTimer -= Time.deltaTime;
            }
            else
            {           
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                walking = true;
            }
        }
        
        hitted = GetComponent<EnemyController>().getHitted();
        if(hitted)
        {
            GetComponent<EnemyController>().cancelHitted();
            Hitted(GetComponent<EnemyController>().getDamageReceived());
        }
        if(health <= 0)
        {
            GetComponent<Animator>().Play("UndeadDead");
        }
        else
        {
            if(attacking)
            {
                GetComponent<Animator>().Play("UndeadShield");
            }
            else
            {       
                
                if(walking)
                {
                    
                        if(((transform.position.x >= maxLimitPos ) && transform.localScale.x > 0) ||((transform.position.x <= minLimitPos ) && transform.localScale.x < 0))
                        {
                            walking = false;
                            idleTimer = 4.5f; 
                            GetComponent<Animator>().Play("UndeadIdle");
                        }
                        else
                        {   
                            GetComponent<Animator>().Play("UndeadWalking");                        
                            if(transform.localScale.x > 0)
                            {
                                transform.position += Vector3.right*0.03f;
                            }                
                            else{
                                transform.position += Vector3.left*0.03f;
                            }
                        }
                    
                }                
            }
            taunted = GetComponentInParent<EnemyController>().getTaunted();
            playerDetected = GetComponentInParent<EnemyController>().getPlayerDetected();
            if (playerDetected != null)
            {
                if(playerDetected.transform.position.y > transform.position.y - 1f && playerDetected.transform.position.y < transform.position.y + 1f)
                {
                    Debug.Log("Está a mi altura");
                    if(playerDetected.transform.position.x <  maxLimitPos && playerDetected.transform.position.x > minLimitPos)
                    {
                        Debug.Log("Está a en mi rango");
                        if(transform.localScale.x > 0)
                        {
                            if(playerDetected.transform.position.x >  transform.position.x)
                            {
                                Debug.Log("Está Delante de mi");
                                attacking = true;                                
                                if(transform.position.x < maxLimitPos)
                                    transform.position += Vector3.right*0.12f;
                            }
                        }
                        else
                        {
                            if(playerDetected.transform.position.x <  transform.position.x)
                            {
                                Debug.Log("Está detrás de mi2");
                                attacking = true;                                
                                if(transform.position.x > minLimitPos)
                                    transform.position += Vector3.left*0.12f;
                            }                            
                        }
                    }
                    
                }
                
                            else                            
                                attacking = false;  
            }
        }
    }

    private void Hitted(float damage)
    {
        if((playerDetected.transform.position.x >  transform.position.x && transform.localScale.x < 0) || (playerDetected.transform.position.x <  transform.position.x && transform.localScale.x > 0) )
        {
            health -= damage;
            hitted = true;
            damageMessagePopUp.GetComponent<TextMeshPro>().text = (damage) + " ";
            health -= (damage);
            Instantiate(damageMessagePopUp, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            Instantiate(blood, transform.position + new Vector3(0, 0f, 0), Quaternion.identity);
        }
    }
    void finishDeath()
    {
        tears.GetComponent<TearsController>().currentValue = 0;
       tears.GetComponent<TearsController>().finalValue = amountOfTearsToDrop;
       Instantiate(tears, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        Destroy(gameObject);
        Destroy(gameObject);
    }
    void finishAttack()
    {
        
    }
    void setRadius(GameObject enemy)
    {
        enemy.GetComponent<EnemyDetector>().setNormalRadius(normalRadius);
        enemy.GetComponent<EnemyDetector>().setTauntedRadius(tauntedRadius);
    }
    void OnCollisionStay2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            
            if (coll.gameObject.transform.position.x > transform.position.x)
                coll.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 3, ForceMode2D.Impulse);
            else
                coll.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 3, ForceMode2D.Impulse);
            coll.gameObject.SendMessage("BarryGotAttacked",30);
            
  
        }

    }
}
