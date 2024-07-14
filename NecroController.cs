using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class NecroController : MonoBehaviour
{
    private float normalRadius = 5f;
    private float tauntedRadius = 5f;

    public GameObject fireRinge;
    public GameObject meteora;

    public GameObject SkeletonSoldier;
    private GameObject blood;
    private GameObject damageMessagePopUp;
    private bool taunted;
    private GameObject playerDetected;
    private float health;
    private float maxHealth;

    private bool freeState;
    private float attackCoolDown;
    private bool resting;
    public Vector3[] possiblePositionFireRing = new Vector3[10];
    private float infernoCoolDown;
    private float meteoraCoolDown;
    private float summonCoolDown;

    void Start()
    {
        maxHealth = 50;
        health = maxHealth;
        freeState = true;
        blood = GetComponent<EnemyController>().getBlood();
        damageMessagePopUp = GetComponent<EnemyController>().getDamageMessagePopUp();
    }

    // Update is called once per frame
    void Update()
    {
        taunted = GetComponentInParent<EnemyController>().getTaunted();
        if(taunted && infernoCoolDown <= 0)
        {
            inferno();
        }
        if(taunted && meteoraCoolDown <= 0)
        {
            meteoraAttack();
        }
        if(taunted && summonCoolDown <= 0)
        {
            summon();
        }
    
        
          
    

        //Timer
    
            
        
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
            if(meteoraCoolDown > 0 && infernoCoolDown > 0 || !taunted)
            {
                GetComponent<Animator>().Play("NecroIddle");
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
    
}