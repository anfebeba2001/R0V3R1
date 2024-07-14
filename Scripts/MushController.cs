using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MushController : MonoBehaviour
{
    private float normalRadius = 1.5f;
    private float tauntedRadius = 3f;
    private bool taunted;
    private GameObject playerDetected;
    private GameObject damageMessagePopUp;
    private Vector3 originalPos;
    private bool escapingOnLeft;
    private bool searching;
    private bool attacking;

    public float maxDistance;
    public float minDistance;
    private float damage = 80;
    private float speed = 10f;
    private float maxVel = 2.8f;
    private float attackCoolDown;
    private float defense = 10;
    private float health = 70;
    private float throwAwayForce;
    private bool finishingAttack;
    public float runningAwayLimit;
    public GameObject colliderHelper;
    private GameObject detector;
    private bool escaping;
    private float microDamageTimer;

    public bool triggerlyCanBeAttacked;
    private GameObject blood;
    private bool parried;


    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        blood = GetComponent<EnemyController>().getBlood();
        damageMessagePopUp = GetComponent<EnemyController>().getDamageMessagePopUp();
        escapingOnLeft = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (microDamageTimer > 0)
        {
            microDamageTimer -= Time.deltaTime;
        }
        if (playerDetected != null)
        {
            if (Mathf.Abs(playerDetected.transform.position.x - transform.position.x) >= 1.2f)
            {
                triggerlyCanBeAttacked = false;
            }
            else
            {
                triggerlyCanBeAttacked = true;
            }
        }
        if (health <= 0)
        {
            GetComponent<Animator>().Play("MushDying");
            colliderHelper.GetComponent<BoxCollider2D>().isTrigger = true;
            escaping = false;
            transform.position = new Vector3(transform.position.x, originalPos.y, transform.position.z);
        }
        if (health <= runningAwayLimit && health > 0)
        {
            detector.GetComponent<EnemyDetector>().setNormalRadius(normalRadius);
            escaping = true;
            taunted = false;
            attacking = false;
            searching = false;
            GetComponent<Animator>().Play("MushIdle");
            transform.position = new Vector3(transform.position.x, originalPos.y - 0.9f, transform.position.z);
            colliderHelper.GetComponent<BoxCollider2D>().isTrigger = true;
            if (escaping)
            {
                if (escapingOnLeft)
                {
                    transform.position = new Vector3(transform.position.x + 0.016f * 2.8f, transform.position.y, transform.position.z);
                    if (transform.position.x > maxDistance)
                    {
                        escapingOnLeft = false;
                    }
                }
                else
                {
                    transform.position = new Vector3(transform.position.x - 0.016f * 2.8f, transform.position.y, transform.position.z);
                    if (transform.position.x < minDistance)
                    {
                        escapingOnLeft = true;
                    }
                }
            }
        }


        taunted = GetComponentInParent<EnemyController>().getTaunted();
        if (!taunted && !escaping && health > 0)
        {
            GetComponent<Animator>().Play("MushIdle");
        }
        if (taunted && !searching && !attacking && !escaping)
        {

            searching = true;
            playerDetected = GetComponentInParent<EnemyController>().getPlayerDetected();
        }
        if (taunted && searching && !attacking && attackCoolDown <= 0 && !parried)
        {
            GetComponent<Animator>().Play("MushRun");
            if (playerDetected.transform.position.x > transform.position.x && GetComponent<Rigidbody2D>().velocity.x < maxVel)
            {
                GetComponent<Rigidbody2D>().AddForce(UnityEngine.Vector2.right * speed, ForceMode2D.Force);
                transform.localScale = new Vector3(3, 3, 3);
            }
            else if (playerDetected.transform.position.x < transform.position.x && GetComponent<Rigidbody2D>().velocity.x > -maxVel)
            {
                transform.localScale = new Vector3(-3, 3, 3);
                GetComponent<Rigidbody2D>().AddForce(Vector2.left * speed, ForceMode2D.Force);
            }
        }
        if (!searching && attacking && !finishingAttack && attackCoolDown <= 0 && health > 0 && !parried)
        {
            GetComponent<Animator>().Play("MushAttack");
            throwAwayForce = 3f;
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
        if (coll.gameObject.tag == "BarryCurrentSword" && escaping)
        {
            Hitted(20);
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
        throwAwayForce = 5f;
        damage = 80;
    }
    void Hitted(float damage)
    {
        if (triggerlyCanBeAttacked)
        {
            blood.transform.position = transform.position;
            damageMessagePopUp.transform.position = transform.position;
            damageMessagePopUp.GetComponent<DamageMessagePopUpController>().setShowTime(0.5f);
            if (parried)
            {
                blood.transform.localScale = new Vector3(2.5f,2.5f , 3);
                damageMessagePopUp.transform.localScale = new Vector3(
                damageMessagePopUp.transform.localScale.x * 3, damageMessagePopUp.transform.localScale.y *3, 3);

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
                
                 damageMessagePopUp.transform.localScale = new Vector3(damageMessagePopUp.transform.localScale.x / 3, damageMessagePopUp.transform.localScale.y /3, 3);

                damageMessagePopUp.GetComponent<TextMeshPro>().text = ((damage * 3 )- defense) + "";
                health -= ((damage * 3 )- defense) ;
                damageMessagePopUp.transform.localScale = new Vector3(
                    damageMessagePopUp.transform.localScale.x * 2, damageMessagePopUp.transform.localScale.y *2, 3);

                Instantiate(damageMessagePopUp, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                Instantiate(blood, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                 blood.transform.localScale = new Vector3(1f,1f , 3);
                damageMessagePopUp.transform.localScale = new Vector3(
                    damageMessagePopUp.transform.localScale.x / 2, damageMessagePopUp.transform.localScale.y /2, 3);
            }
            else
            {


                damageMessagePopUp.GetComponent<TextMeshPro>().text = (damage - defense) + " ";
                health -= (damage - defense);
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
        Destroy(gameObject);
    }

}