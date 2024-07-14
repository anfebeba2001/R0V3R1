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
        if(taunted && health > 0 && attackCoolDown <= 0)
        {
            playerDetected = GetComponentInParent<EnemyController>().getPlayerDetected();
            if(freeState)
            {
                attackCoolDown = 90f;
                resting = false;
                
                switch((Random.Range(1,12)%4))
                {
                    case 0:
                        if(health < maxHealth)
                        {
                            freeState = false;
                            heal();
                        }
                        else
                        {
                            attackCoolDown = 0f;
                            resting = true;
                        }
                    break;
                    case 1:
                        freeState = false;
                        inferno();
                    break;
                    case 2:
                        freeState = false;
                        inferno();
                    break;
                    case 3:
                    if(health < maxHealth)
                        {
                            freeState = false;
                            summon();
                        }
                        else
                        {
                            attackCoolDown = 0f;
                            resting = true;
                        }
                        
                    break;
                }
            }
        }
        else if(health <= 0)
        {
            GetComponent<Animator>().Play("NecroDead");
        }
   
        else if(resting)
        {
            GetComponent<Animator>().Play("NecroIddle");
        }

        //Timer
    
            if(attackCoolDown > 0)
            {
                freeState = false;
                attackCoolDown -= Time.deltaTime;
            }
            else{
                freeState = true;
            }
        
   
      
    }
    private void meteoraAttack()
    {
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
        throw new System.NotImplementedException();
    }

    private void inferno()
    {
        fireRinge.transform.position = possiblePositionFireRing[Random.Range(0,10)];
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
