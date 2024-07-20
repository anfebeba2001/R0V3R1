using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
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
    private float dyingRage = 2.5f;
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
    private bool parried;
    private Vector3 originalPos;
    public GameObject rogueSlash;
    public GameObject rogueSingleSlash;
    public GameObject rogueVortex;
    private float dialogCoolDown;
    public float health;
    private float maxHealth;
    private int luckySingleSlash;
    private int cruchToChargeSlashCounterBack;
    private bool hitted;
     private GameObject blood;
    private GameObject damageMessagePopUp;

    // Initialize the elements.

    void Start()
    {
        blood = GetComponent<EnemyController>().getBlood();
        damageMessagePopUp = GetComponent<EnemyController>().getDamageMessagePopUp();
        dialogCoolDown = 20;
        maxHealth = 1000;
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
    void FixedUpdate()
    {
        GetComponent<BossController>().setHealth(health);
        GetComponent<BossController>().setMaxHealth(maxHealth);
        hitted = GetComponent<EnemyController>().getHitted();
        if(hitted)
        {
            GetComponent<EnemyController>().cancelHitted();
            Hitted(GetComponent<EnemyController>().getDamageReceived());
        }
        if(health <= 0)
        {
            GetComponent<Animator>().Play("RogueDead");
        }
        else
        {
        taunted = GetComponent<BossController>().getTaunted();
        if(taunted)
        {
            if(!parried)
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
                    
                    cruchToChargeSlashCounterBack= Random.Range(1,4);
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
            }
            else{
                GetComponent<Animator>().Play("RogueParried");
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
            transform.position += new Vector3(1,0,0)*(transform.localScale.x/6)/4.17f*dyingRage*(1.5f-health/maxHealth);
            if((transform.localScale.x > 0 && transform.position.x >= maxPos - .5f ) ||
             (transform.localScale.x < 0 && transform.position.x <= minPos + 0.5f ))
            {
                makingDistanceValue = -1;
                attackCoolDown = 2f*(health/maxHealth);
                attacking = false;
                chargingHorizontal = false;
                cameraObj.GetComponent<Animator>().Play("CameraShakeEffect");
            }
        }
        if(chargingVertical)
        {
            transform.position += new Vector3(1*(transform.localScale.x/6)/4.17f,0.5f/9f,0)*dyingRage*(1.5f-health/maxHealth);
            if( (transform.position.y >= originalPos.y + 3) ||
            ((transform.localScale.x > 0 && transform.position.x >= maxPos - .5f ) ||
             (transform.localScale.x < 0 && transform.position.x <= minPos + 0.5f )))
            {
                cameraObj.GetComponent<Animator>().Play("CameraShakeEffect");
                chargingVertical = false;
                goingDownAttackBool = true;
            }
        }

        if(goingDownAttackBool)
        {
            transform.position = new Vector3(transform.position.x,transform.position.y-0.051f,0);
            GetComponent<Animator>().Play("RogueChargingAttackVerticalDrop");
            if(transform.position.y <= originalPos.y)
            {
                cameraObj.GetComponent<Animator>().Play("CameraShakeEffect");
                attacking = false;
                goingDownAttackBool = false;
                makingDistanceValue = 1;
                attackCoolDown = 2f*health/maxHealth ;
                rogueSlash.transform.position = transform.position;
                rogueSlash.transform.position += new Vector3(0,-1,0)*0.5f;
                Instantiate(rogueSlash);
            }
            
        }


        if(attackCoolDown > 0 && taunted)
        {
            attackCoolDown -= Time.deltaTime;
            transform.position += new Vector3(1,0,0)*(transform.localScale.x/100)/4.17f*dyingRage*(1.5f-health/maxHealth);
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
    }

    private void Hitted(float damage)
    {
        if(!parried)
        {
            health -= damage;
            hitted = true;
            damageMessagePopUp.GetComponent<TextMeshPro>().text = (damage) + " ";
            health -= (damage);
            Instantiate(damageMessagePopUp, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            Instantiate(blood, transform.position + new Vector3(0, 0f, 0), Quaternion.identity);
        }
        else
        {
             health -= damage*3;
            hitted = true;
            damageMessagePopUp.GetComponent<TextMeshPro>().text = ("CRITICAL!!!!") + " ";
            parried = false;
            attacking = false;
            makingDistanceValue = 1;
            attackCoolDown = 2f*health/maxHealth;    
            health -= (damage);
            Instantiate(damageMessagePopUp, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            Instantiate(blood, transform.position + new Vector3(0, 0f, 0), Quaternion.identity);
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
            attackCoolDown = 2f*health/maxHealth;    
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
                transform.localScale = new Vector3(1,0.4f,1)*4.1795f;
                rogueSingleSlash.GetComponent<SpriteRenderer>().flipY = false;
            }
            else
            {
                transform.localScale = new Vector3(-1,0.4f,1)*4.1795f;
                rogueSingleSlash.GetComponent<SpriteRenderer>().flipY = true;
            }

            rogueSingleSlash.transform.localScale = transform.localScale*10/4.1795f;
            rogueSingleSlash.transform.position = transform.position;        
            
            rogueSingleSlash.GetComponent<RogueSingleSlashController>().isLucky = luckySingleSlash == cruchToChargeSlashCounterBack;
            rogueSingleSlash.GetComponent<RogueSingleSlashController>().rogue = gameObject;
            rogueSingleSlash.GetComponent<RogueSingleSlashController>().velocity = dyingRage*(1.5f-health/maxHealth)/2;
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
            attackCoolDown = 2f*health/maxHealth;
            Instantiate(rogueVortex);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player" )
        {
            if(chargingHorizontal || chargingVertical || goingDownAttackBool)
            {
                coll.gameObject.SendMessage("BarryGotAttacked",50);
                if (coll.gameObject.transform.position.x > transform.position.x)
                    coll.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(3, 2), ForceMode2D.Impulse);
                else
                    coll.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(3, 2), ForceMode2D.Impulse);
            }            
        }
    }

    internal void gotParried()
    {
        damageMessagePopUp.GetComponent<TextMeshPro>().text =  " That's a Parry!!!!";
        Instantiate(damageMessagePopUp, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        parried = true;
    }
    void endParried()
    {
        parried = false;
        attacking = false;
        makingDistanceValue = 1;
        attackCoolDown = 2f*health/maxHealth;    
    }
}
