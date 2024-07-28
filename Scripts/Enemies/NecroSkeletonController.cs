using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NecroSkeletonController : MonoBehaviour
{
    // Start is called before the first frame update
    private float health;
    private float attackCoolDown;
    public GameObject skeletonWave;
    private Vector3 fixedScale;
    private Vector3 fixedPos;
    private GameObject tears;
    private bool hitted;
    private GameObject blood;
    private GameObject damageMessagePopUp;
    private int amountOfTearsToDrop = 57;

    void Start()
    {
        tears = GetComponent<EnemyController>().getTears();
        blood = GetComponent<EnemyController>().getBlood();
        damageMessagePopUp = GetComponent<EnemyController>().getDamageMessagePopUp();
        hitted = false;
        health = 50;
    }

    // Update is called once per frame
    void Update()
    {
        hitted = GetComponent<EnemyController>().getHitted();
        if(hitted)
        {
            GetComponent<EnemyController>().cancelHitted();
            Hitted(GetComponent<EnemyController>().getDamageReceived());
        }
        if(health > 0)
        {
            if(!hitted)
            {
                if(attackCoolDown > 0f )
                {
                    attackCoolDown -= Time.deltaTime;
                    GetComponent<Animator>().Play("SkeletonIdle");
                }
                else
                {
                    GetComponent<Animator>().Play("SkeletonAttack");
                }
            }
            else
            {
                GetComponent<Animator>().Play("SkeletonHitted");
            }
        }
        else
        {
            GetComponent<Animator>().Play("SkeletonDeath");
        }
        
    }
    void attack()
    {
        attackCoolDown = 2;
        skeletonWave.transform.position = transform.position;
        fixedScale = skeletonWave.transform.localScale;
        fixedPos = skeletonWave.transform.position;
        fixedPos.y -= 0.6f;
        skeletonWave.transform.position = fixedPos;
            
        if(transform.localScale.x > 0)
        {
            
           fixedScale.x = 2.14f; 
        }
        else
        {
            fixedScale.x = -2.14f;
        }
        skeletonWave.transform.localScale = fixedScale;
        Instantiate(skeletonWave);
        
    }
    void Death(){
        health = 0; 
        damageMessagePopUp.GetComponent<TextMeshPro>().text =  "CRITICAL!! ";
        Instantiate(damageMessagePopUp, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        Instantiate(blood, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        GetComponent<Rigidbody2D>().AddForce(Vector2.right, ForceMode2D.Impulse);
        GetComponent<Rigidbody2D>().AddForce(Vector2.left, ForceMode2D.Impulse);
    }
    void finishDeath()
    {
        tears.GetComponent<TearsController>().currentValue = 0;
       tears.GetComponent<TearsController>().finalValue = amountOfTearsToDrop;
       Instantiate(tears, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        Destroy(gameObject);
    }
    void Hitted(float damage)
    {
        
        health -= damage;
        hitted = true;
        damageMessagePopUp.GetComponent<TextMeshPro>().text = (damage) + " ";
        health -= (damage);
        Instantiate(damageMessagePopUp, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        Instantiate(blood, transform.position + new Vector3(0, 0f, 0), Quaternion.identity);
        GetComponent<Rigidbody2D>().AddForce(Vector2.right, ForceMode2D.Impulse);
        GetComponent<Rigidbody2D>().AddForce(Vector2.left, ForceMode2D.Impulse);
        
    }
    void finishHitted()
    {
        hitted = false;
    }
    void HittedByBow(float damage)
    {
        Hitted(damage);
    }
}