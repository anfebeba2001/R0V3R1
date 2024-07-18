using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class RogueController : MonoBehaviour
{
string[][] dialogs =  {
new string[] {"Fuiste demasiado lejos... Ahora pagarás.",
              "Tu osadía ha superado tu sentido común... Muere... ",
              "Ni tus gritos podrán escapar.",
              "DESFALLECE"},
};
    private bool taunted;
    private float attackCoolDown;
    private bool attacking;
    private GameObject player;
    private bool chargingHorizontal;
    private float maxPos;
    private float minPos;
 
    private float makingDistanceValue;
    private GameObject cameraObj;
    private bool chargingVertical;
    private bool goingDownAttackBool;
    private Vector3 originalPos;
    public GameObject RogueSlash;

    // Initialize the elements.

    void Start()
    {
        originalPos = transform.position;
        makingDistanceValue = 1;
        attackCoolDown = 1f;
        maxPos = GetComponent<BossController>().getMaxPos();
        minPos = GetComponent<BossController>().getMinPos();
        cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<BossController>().setDialogs(setDialogsLocal());
    }

    // Update is called once per frame
    void Update()
    {
        taunted = GetComponent<BossController>().getTaunted();
        if(taunted)
        {
            if(!attacking && attackCoolDown <= 0)
            {

            attacking = true;
            int choiseAttack = Random.Range(1,60);
                if(choiseAttack%6 >= 0)
                {
                    cruchToCharge(true);
                }
                else if(choiseAttack%6 == 1)
                {

                }  
                else if(choiseAttack%6 == 2)
                {

                }    
                else if(choiseAttack%6 == 3)
                {

                } 
                else if(choiseAttack%6 == 4)
                {

                } 
                else if(choiseAttack%6 == 5)
                {

                } 
            }      
        }
        else
        {
            GetComponent<Animator>().Play("RogueIdle");
        }
        if(chargingHorizontal)
        {
            transform.position += new Vector3(1,0,0)*(transform.localScale.x/6)/4.17f;
            if((transform.localScale.x > 0 && transform.position.x >= maxPos - .5f ) || (transform.localScale.x < 0 && transform.position.x <= minPos + 0.5f ))
            {
                makingDistanceValue = -1;
                attackCoolDown = 2f;
                attacking = false;
                chargingHorizontal = false;
                cameraObj.GetComponent<Animator>().Play("CameraShakeEffect");
            }
        }
        if(chargingVertical)
        {
            transform.position += new Vector3(1*(transform.localScale.x/6)/4.17f,0.5f/9f,0);
            if((transform.localScale.x > 0 && transform.position.x >= maxPos - 5.5f ) || (transform.localScale.x < 0 && transform.position.x <= minPos + 5.5f ))
            {
                cameraObj.GetComponent<Animator>().Play("CameraShakeEffect");
                chargingVertical = false;
                goingDownAttackBool = true;
            }
        }

        if(goingDownAttackBool)
        {
            transform.position = new Vector3(transform.position.x,transform.position.y-0.5f/9f,0);
            GetComponent<Animator>().Play("RogueChargingAttackVerticalDrop");
            if(transform.position.y == originalPos.y)
            {
                cameraObj.GetComponent<Animator>().Play("CameraShakeEffect");
                goingDownAttackBool = false;
                makingDistanceValue = 1;
                attackCoolDown = 2f;
                RogueSlash.transform.position = transform.position;
                Instantiate(RogueSlash);
            }
            
        }


        if(attackCoolDown > 0 && taunted)
        {
            attackCoolDown -= Time.deltaTime;
            transform.position += new Vector3(1,0,0)*(transform.localScale.x/100)/4.17f;
            if(player.transform.position.x > transform.position.x)
            {   
                
                transform.localScale = new Vector3(1*makingDistanceValue,1,1)*4.1795f;                
            }
            else
            {
                transform.localScale = new Vector3(-1*makingDistanceValue,1,1)*4.1795f;
            }
            GetComponent<Animator>().Play("RogueWalking");
        }
    }

    private void cruchToCharge(bool charging)
    {
        if(charging)
        {
            GetComponent<Animator>().Play("RogueChargingAttack");
        }
        else
        {
            if(player.transform.position.x > transform.position.x)
                {   
                    transform.localScale = new Vector3(1,1,1)*4.1795f;
                }
                else{
                    transform.localScale = new Vector3(-1,1,1)*4.1795f;
                }
            if(Random.Range(0,40)%2 == 0)
            {
                chargingHorizontal = true;                
                GetComponent<Animator>().Play("RogueChargingAttackHorizontal");
            }
            else
            {
                chargingVertical = true;                
                GetComponent<Animator>().Play("RogueChargingAttackVertical");
            }
        }
    }

    string[][] setDialogsLocal()
    {
        return dialogs;
    }
    void endAttack()
    {
        attacking = false;
    }
    void endChargin()
    {
        cruchToCharge(false);
    }
    
}
