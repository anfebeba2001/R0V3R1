using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MushController : MonoBehaviour
{
    public float maxPos;
    public float minPos;
    private float normalRadius = 1.5f;
    private float tauntedRadius = 3f;
    public bool taunted;
    private GameObject playerDetected;
    private GameObject damageMessagePopUp;

    public bool searching;
    public bool attacking;

    private float damage = 80;
    public float attackCoolDown;
    private float health = 200;
    private float throwAwayForce;
    public bool finishingAttack;
    public float runningAwayLimit;
    public GameObject colliderHelper;
    private GameObject detector;
    private GameObject tears;
    private float microDamageTimer;
    private GameObject blood;
    private bool parried;
    private bool hitted;
    private int amountOfTearsToDrop = 387;


    // Start is called before the first frame update
    void Start()
    {      
        tears = GetComponent<EnemyController>().getTears();
        blood = GetComponent<EnemyController>().getBlood();
        damageMessagePopUp = GetComponent<EnemyController>().getDamageMessagePopUp();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        hitted = GetComponent<EnemyController>().getHitted();
        if(hitted)
        {
            GetComponent<EnemyController>().cancelHitted();
            Hitted(GetComponent<EnemyController>().getDamageReceived());
        }
        if (microDamageTimer > 0)
        {
            
            microDamageTimer -= Time.deltaTime;
        }
      
        if (health <= 0)
        {

            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<BoxCollider2D>().isTrigger = true;
            GetComponent<Animator>().Play("MushDying");
        }
        if (health <= runningAwayLimit && health > 0 && !taunted)
        {
            detector.GetComponent<EnemyDetector>().setNormalRadius(normalRadius);
            taunted = false;
            attacking = false;
            searching = false;
            GetComponent<Animator>().Play("MushIdle");                
        }


        taunted = GetComponentInParent<EnemyController>().getTaunted();
        if (!taunted && health > 0)
        {
            GetComponent<Animator>().Play("MushIdle");
        }
        if (taunted && !searching && !attacking)
        {
            searching = true;
            playerDetected = GetComponentInParent<EnemyController>().getPlayerDetected();
        }
        if (taunted && searching && !attacking && attackCoolDown <= 0 && !parried && health > 0 &&
            playerDetected.transform.position.y > transform.position.y - 1.5f &&
            playerDetected.transform.position.y < transform.position.y + 1.5f &&
            playerDetected.transform.position.x > minPos &&
            playerDetected.transform.position.x < maxPos)
        {
            if (playerDetected.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(5.6f, 5.6f, 5.6f);
            }
            else if (playerDetected.transform.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-5.6f, 5.6f, 5.6f);
            }
        }
        if (!searching && attacking && !finishingAttack && attackCoolDown <= 0 && health > 0 && !parried)
        {
            GetComponent<Animator>().Play("MushAttack");
            throwAwayForce = 2f;
            damage = 30;
        }
        if (attackCoolDown > 0 && health > 0)
        {
            attackCoolDown -= Time.deltaTime;
            GetComponent<Animator>().Play("MushIdle");
        }
        if(parried)
        {
            GetComponent<Animator>().Play("MushParried");
        }
    }
    void setRadius(GameObject enemy)
    {
        detector = enemy;
        enemy.GetComponent<EnemyDetector>().setNormalRadius(normalRadius);
        enemy.GetComponent<EnemyDetector>().setTauntedRadius(tauntedRadius);
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        
        if (coll.gameObject.tag == "Player" && searching)
        {
            searching = false;
            attacking = true;
        }     
    }
    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player" && health <= 0 && microDamageTimer <= 0)
        {
            microDamageTimer = 0.5f;
            coll.gameObject.SendMessage("microDamage", 3);
        }
    }
    void OnCollisionEnter2D(Collision2D coll)
    {

        if (coll.gameObject.tag == "Player" && attacking)
        {
            if (finishingAttack)
            {
                coll.gameObject.SendMessage("BarryGotAttacked", damage);
                if (playerDetected.transform.position.x > transform.position.x)
                    coll.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(throwAwayForce, 2), ForceMode2D.Impulse);
                else
                    coll.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-throwAwayForce, 2), ForceMode2D.Impulse);
            }
            else
            {

                if (!coll.gameObject.GetComponent<BarryController>().getAttacking())
                {
                    coll.gameObject.SendMessage("BarryGotAttacked", damage);
                    if (playerDetected.transform.position.x > transform.position.x)
                        coll.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(throwAwayForce, 2), ForceMode2D.Impulse);
                    else
                        coll.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-throwAwayForce, 2), ForceMode2D.Impulse);
                }

            }

        }
    }
    void attackEnded()
    {
        attackCoolDown = 0.8f;
        attacking = false;
        searching = true;
        finishingAttack = false;
    }
    void beginAttack()
    {
        finishingAttack = true;
        throwAwayForce = 3f;
        damage = 80;
    }
    void Hitted(float damage)
    {
            if(health > 0)
            {
                    blood.transform.position = transform.position;
            damageMessagePopUp.transform.position = transform.position;
            damageMessagePopUp.GetComponent<DamageMessagePopUpController>().setShowTime(0.5f);
           
                    if (parried)
            {
                damageMessagePopUp.GetComponent<TextMeshPro>().text = "CRITICAL!!! ";
                ParriedEnd();
                if (playerDetected.transform.position.x < transform.position.x)
                {
                    GetComponent<Rigidbody2D>().AddForce(Vector2.right*2, ForceMode2D.Impulse);
                }
                else
                {
                    GetComponent<Rigidbody2D>().AddForce(Vector2.left*2, ForceMode2D.Impulse);
                }
                Instantiate(damageMessagePopUp, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                
               
               

                health -= ((damage * 3 )) ;
            
                Instantiate(damageMessagePopUp, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                Instantiate(blood, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            }
            else
            {


                damageMessagePopUp.GetComponent<TextMeshPro>().text = (damage ) + " ";
                health -= (damage );
                Instantiate(blood, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                Instantiate(damageMessagePopUp, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                if (playerDetected.transform.position.x < transform.position.x)
                {
                    GetComponent<Rigidbody2D>().AddForce(Vector2.right, ForceMode2D.Impulse);
                }
                else
                {
                    GetComponent<Rigidbody2D>().AddForce(Vector2.left, ForceMode2D.Impulse);
                }
            
               }
              
            }
                     
        

    }
    void HittedByBow(float damage)
    {
        if (finishingAttack)
        {
            damageMessagePopUp.transform.position = transform.position;
            GetComponent<Animator>().Play("MushParried");
            damageMessagePopUp.GetComponent<TextMeshPro>().text = "Thats a PARRY!!! ";
            Instantiate(damageMessagePopUp, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            searching = false;
            attacking = false;
            parried = true;
            taunted = false;
            finishingAttack = false;
        }
        else
        {
            Hitted(damage);
        }
    }
  
    void ParriedEnd()
    {
        attacking = true;
        searching = false;
        taunted = true;
        parried = false;
        
    }
    void finishDying()
    {
     
       tears.GetComponent<TearsController>().currentValue = 0;
       tears.GetComponent<TearsController>().finalValue = amountOfTearsToDrop;
       Instantiate(tears, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
       Destroy(gameObject);
    }

}