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

new string[] {"Levanta tu espada!",
              "Pelea mientras te quede aliento",
              "Enterraré tu cuerpo junto a los demás",
              "Serás el festín de los gusanos"

            },
new string[] {
            "Miserable escoria...",
            "Ningún niño podrá conmigo!",
            "Me estoy comenzando a cansar de esto",
            "No huyas del filo de tu destino"

}
};
    private bool taunted;
    private float attackCoolDown;
    private bool attacking;
    private GameObject player;
    private bool chargingHorizontal;
    public float maxPos;
    public float minPos;
 
    private float makingDistanceValue;
    public GameObject cameraObj;
    private bool chargingVertical;
    private bool goingDownAttackBool;
    private Vector3 originalPos;
    public GameObject rogueSlash;
    public GameObject rogueSingleSlash;
    public GameObject rogueVortex;
    private float dialogCoolDown;
    private float health;
    private float maxHealth;
    private int luckySingleSlash;
    private int cruchToChargeSlashCounterBack;

    // Initialize the elements.

    void Start()
    {
        dialogCoolDown = 20;
        maxHealth = 3000;
        health = maxHealth;
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
                if(choiseAttack%3 == 0)
                {
                    cruchToCharge(true);
                }
                else if(choiseAttack%3 == 1)
                {
                    cruchToChargeVortex(true);
                }  
                else if(choiseAttack%3 == 2)
                {
                    
                    cruchToChargeSlashCounterBack= Random.Range(3,6);
                    luckySingleSlash = Random.Range(1,cruchToChargeSlashCounterBack);
                    cruchToChargeSlash(true);
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


            if(dialogCoolDown > 0)
            {
                dialogCoolDown -= Time.deltaTime;
            }
            else
            {
                dialogCoolDown = 10f;
                if(health > maxHealth/3)
                {
                    GetComponent<BossController>().dialog("Attack");
                }
                else
                {
                    GetComponent<BossController>().dialog("Losing");
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
            if( transform.position.y >= originalPos.y + 3)
            {
                cameraObj.GetComponent<Animator>().Play("CameraShakeEffect");
                chargingVertical = false;
                goingDownAttackBool = true;
            }
        }

        if(goingDownAttackBool)
        {
            transform.position = new Vector3(transform.position.x,transform.position.y-0.5f/7f,0);
            GetComponent<Animator>().Play("RogueChargingAttackVerticalDrop");
            if(transform.position.y <= originalPos.y)
            {
                cameraObj.GetComponent<Animator>().Play("CameraShakeEffect");
                attacking = false;
                goingDownAttackBool = false;
                makingDistanceValue = 1;
                attackCoolDown = 2f;
                rogueSlash.transform.position = transform.position;
                rogueSlash.transform.position += new Vector3(0,-1,0)*0.5f;
                Instantiate(rogueSlash);
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
    void endCharginVortex()
    {
        cruchToChargeVortex(false);
    }
    void endCharginSlash()
    {
        cruchToChargeSlash(false);
        cruchToChargeSlashCounterBack--;
        if(cruchToChargeSlashCounterBack == 0)
        {
            attacking = false;
            makingDistanceValue = 1;
            attackCoolDown = 2f;    
        }
    }
    
    private void cruchToChargeSlash(bool charging)
    {
        if(charging)
        {
            GetComponent<Animator>().Play("RogueChargingSlash");
        }
        else
        {
            if(player.transform.position.x > transform.position.x)
            {   
                transform.localScale = new Vector3(1,1,1)*4.1795f;
            }
            else
            {
                transform.localScale = new Vector3(-1,1,1)*4.1795f;
                
            }

            rogueSingleSlash.transform.localScale = transform.localScale*10/4.1795f;
            rogueSingleSlash.transform.position = transform.position;        
            rogueSingleSlash.GetComponent<RogueSingleSlashController>().isLucky = luckySingleSlash == cruchToChargeSlashCounterBack;
            Instantiate(rogueSingleSlash);
        }
    }

    private void cruchToChargeVortex(bool charging)
    {
       if(charging)
        {
            GetComponent<Animator>().Play("RogueChargingVortex");
        }
        else
        {
            rogueVortex.transform.position = new Vector3(
                                                         Random.Range(maxPos*10,minPos*10)/10,
                                                         Random.Range((originalPos.y+3)*10,originalPos.y*10)/10,
                                                         rogueVortex.transform.position.z);
                                                         attacking = false;
            makingDistanceValue = 1;
            attackCoolDown = 2f;
            Instantiate(rogueVortex);
        }
    }
    
}
