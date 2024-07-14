using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

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
    private int leftMeteora;
    private float meteoraCoolDown;

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
        if(taunted && health > 0)
        {
            playerDetected = GetComponentInParent<EnemyController>().getPlayerDetected();
            if(freeState)
            {
                leftMeteora = 3;
              /*  switch((Random.Range(1,12)%4))
                {
                    case 0:
                        if(health < maxHealth)
                        {
                            freeState = false;
                            heal();
                        }
                    break;
                    case 1:
                        freeState = false;
                        leftMeteora = 3;
                    break;
                    case 2:
                        freeState = false;
                        inferno();
                    break;
                    case 3:
                        freeState = false;
                        summon();
                    break;
                }*/
            }
        }
        else if(health <= 0)
        {
            GetComponent<Animator>().Play("NecroDead");
        }
        else
        {
            GetComponent<Animator>().Play("NecroIddle");
        }

        //Timer
        if(!freeState)
        {
            if(attackCoolDown > 0)
            {
                attackCoolDown -= Time.deltaTime;
            }
            else{
                freeState = true;
            }
        }
        if(meteoraCoolDown > 0)
        {
            meteoraCoolDown -= Time.deltaTime;
        }
       
        if(leftMeteora > 0 && meteoraCoolDown <= 0f)
        {
            leftMeteora--;
            meteoraCoolDown = 2;
            meteora.transform.position = new Vector3(playerDetected.transform.position.x,playerDetected.transform.position.y+7,playerDetected.transform.position.z);
            Instantiate(meteora);
        }
    }


    private void summon()
    {
        throw new System.NotImplementedException();
    }

    private void inferno()
    {
        throw new System.NotImplementedException();
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
        attackCoolDown = 2f;
    }
}
