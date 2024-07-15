using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class NecroController : MonoBehaviour
{
    private float normalRadius = 5f;
    private float tauntedRadius = 5f;
    private float invincibleCoolDown;
    private bool invincible;
    public GameObject shield;

    public GameObject fireRinge;
    public GameObject meteora;

    public GameObject SkeletonSoldier;
    private GameObject blood;
    private GameObject damageMessagePopUp;
    private bool taunted;
    private GameObject playerDetected;
    private float health;
    private float maxHealth;
    private float attackCoolDown;
    private bool resting;
    public Vector3[] possiblePositionFireRing = new Vector3[10];
    private float infernoCoolDown;
    private float meteoraCoolDown;
    private float summonCoolDown;
    private float healingCoolDown;
    private bool hitted;

    void Start()
    {
        resting = true;
        invincibleCoolDown = 100;
        maxHealth = 1;
        health = maxHealth;
        blood = GetComponent<EnemyController>().getBlood();
        damageMessagePopUp = GetComponent<EnemyController>().getDamageMessagePopUp();
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
        if(health <= 0)
        {
            GetComponent<Animator>().Play("NecroDead");
        }
        else
        {
            
        taunted = GetComponentInParent<EnemyController>().getTaunted();
        if(taunted && infernoCoolDown <= 0)
        {
            resting = false;
            inferno();
        }
        if(taunted && meteoraCoolDown <= 0)
        {
            resting = false;
            meteoraAttack();
        }
        if(taunted && summonCoolDown <= 0)
        {
            resting = false;
            summon();
        }
        if(taunted && healingCoolDown <= 0)
        {
            resting = false;
            heal();
        }
        
        }
          
    
        if(resting)
        {
            GetComponent<Animator>().Play("NecroIddle");
        }
        //Timer
    
            
        if(taunted)
        {

       
            if(infernoCoolDown > 0)
            {
                infernoCoolDown -= Time.deltaTime;
            }
            if(summonCoolDown > 0)
            {
                summonCoolDown -= Time.deltaTime;
            }
            if(meteoraCoolDown > 0)
            {
                meteoraCoolDown -= Time.deltaTime;
            }
            if(healingCoolDown > 0)
            {
                healingCoolDown -= Time.deltaTime;
            }


            if(invincibleCoolDown > 0)
            {
                shield.SetActive(true);
                invincibleCoolDown -= Time.deltaTime;
            }
            else
            {
                shield.SetActive(false);
            }
       }
    }

    private void Hitted(float damage)
    {
        if(invincibleCoolDown <=  0)
        {

       
        health -= damage;
        hitted = true;
        damageMessagePopUp.GetComponent<TextMeshPro>().text = (damage) + " ";
        health -= (damage);
        Instantiate(damageMessagePopUp, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        Instantiate(blood, transform.position + new Vector3(0, 0f, 0), Quaternion.identity);
        GetComponent<Rigidbody2D>().AddForce(Vector2.right, ForceMode2D.Impulse);
        GetComponent<Rigidbody2D>().AddForce(Vector2.left, ForceMode2D.Impulse);
        invincibleCoolDown = 100;
         }
         else{

        damageMessagePopUp.GetComponent<TextMeshPro>().text =  " Blocked!!";
        Instantiate(damageMessagePopUp, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
         }
    }

    private void meteoraAttack()
    {
        meteoraCoolDown = 7f;
        GetComponent<Animator>().Play("NecroMeteora");
        meteora.transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z);
        Instantiate(meteora);
        meteora.transform.position = new Vector3(transform.position.x+2,transform.position.y,transform.position.z);
        Instantiate(meteora);
        meteora.transform.position = new Vector3(transform.position.x+4,transform.position.y,transform.position.z);
        Instantiate(meteora);
        meteora.transform.position = new Vector3(transform.position.x+6,transform.position.y,transform.position.z);
        Instantiate(meteora);
        meteora.transform.position = new Vector3(transform.position.x-2,transform.position.y,transform.position.z);
        Instantiate(meteora);
        meteora.transform.position = new Vector3(transform.position.x-4,transform.position.y,transform.position.z);
        Instantiate(meteora);
        meteora.transform.position = new Vector3(transform.position.x-6,transform.position.y,transform.position.z);
        Instantiate(meteora);

    }

    private void summon()
    {
        GetComponent<Animator>().Play("NecroInvoke");
        summonCoolDown = 20f;
        SkeletonSoldier.transform.position = new Vector3(transform.position.x+(Random.Range(-6,6)),transform.position.y+7,transform.position.z);
        if(SkeletonSoldier.transform.position.x > transform.position.x )
        {
            SkeletonSoldier.transform.localScale = new Vector3(-1,1,0);
        }
        else
        {
            SkeletonSoldier.transform.localScale = new Vector3(1,1,0);
        }
        Instantiate(SkeletonSoldier);
    }

    private void inferno()
    {
        GetComponent<Animator>().Play("NecroInferno");
        fireRinge.transform.position = possiblePositionFireRing[Random.Range(0,10)];
        infernoCoolDown = 70;
        Instantiate(fireRinge);
    }

    

    private void heal()
    {
        GetComponent<Animator>().Play("NecroHealing");
        if(health < maxHealth-(maxHealth/4))
        {
            health += maxHealth/4;


        damageMessagePopUp.GetComponent<TextMeshPro>().text = "+" + maxHealth/4;

        Instantiate(damageMessagePopUp, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        }
        healingCoolDown = 80;
    }

    void setRadius(GameObject enemy)
    {
        enemy.GetComponent<EnemyDetector>().setNormalRadius(normalRadius);
        enemy.GetComponent<EnemyDetector>().setTauntedRadius(tauntedRadius);
    }
    void finishAttack()
    {
        resting = true;
    }
    void finishDeath()
    {
        Destroy(gameObject);
    }
}