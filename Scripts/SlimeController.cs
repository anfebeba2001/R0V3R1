using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    // Start is called before the first frame update
    private float health = 50;
    private bool taunted;
    private bool searching;
    private GameObject playerDetected;
    private float speed = 5;
    private bool attacking;
    private Color normalColor = new Color(1, 1, 1, 1);
    private Color attackingColor = new Color(1.0f, 0.0f, 0.0f, 0.8f);


    private Vector3 upAttackingPos;
    private float goingUpTimer;
    private float goingDownTimer;
    private bool goingUp;
    private bool goingDown;
    private float goingDownLimit = 0.06f;
    private float goingUpLimit = 2.5f;
    private Vector3 fixedPlayerPosition;
    private float goingUpVelocity = 0.0075f;
    private float goingDownVelocity = 0.24f;
    private float normalRadius = 0.9f;
    private float tauntedRadius = 1.5f;
    private float damage = 300;
    private float maxVel = 1f;
    private bool dying;
    private GameObject damageMessagePopUp;
    private bool firstAttacked;
    private float firstAttackedTimer;
    public GameObject effectHandler;
    private float hittedEffecTimer;

    void Start()
    {

        damageMessagePopUp = GetComponent<EnemyController>().getDamageMessagePopUp();
        dying = false;
        goingDownTimer = 500;
        goingUpTimer = 500;
    }

    // Update is called once per frame
    void Update()
    {
        if (effectHandler.transform.localScale.x > 0)
        {
            effectHandler.SetActive(true);
            hittedEffecTimer -= Time.deltaTime;
            effectHandler.transform.localScale = new Vector3(
                effectHandler.transform.localScale.x - 0.02f,
                effectHandler.transform.localScale.x - 0.02f,
                effectHandler.transform.localScale.x - 0.02f
            );
        }
        else
        {
            effectHandler.SetActive(false);

        }
        if (health <= 0)
        {
            transform.localScale = new Vector3(transform.localScale.x + 0.05f, transform.localScale.y + 0.05f, transform.localScale.z);
            dying = true;
        }
        if (dying)
        {
            GetComponent<Animator>().SetBool("Dying", true);
            GetComponent<Animator>().Play("SlimeDead");
        }
        taunted = GetComponentInParent<EnemyController>().getTaunted();
        if (!taunted && !dying)
        {
            GetComponent<SpriteRenderer>().color = normalColor;
        }
        if (taunted && !searching && !attacking)
        {
            searching = true;
            playerDetected = GetComponentInParent<EnemyController>().getPlayerDetected();
        }
        if (taunted && searching && !attacking)
        {
            GetComponent<SpriteRenderer>().color = attackingColor;
            if (playerDetected.transform.position.x > transform.position.x && GetComponent<Rigidbody2D>().velocity.x < maxVel)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * speed, ForceMode2D.Force);
            }
            else if (playerDetected.transform.position.x < transform.position.x && GetComponent<Rigidbody2D>().velocity.x > -maxVel)
            {

                GetComponent<Rigidbody2D>().AddForce(Vector2.left * speed, ForceMode2D.Force);
            }
        }
        if (attacking)
        {

            searching = false;
            GetComponent<SpriteRenderer>().color = attackingColor;
            playerDetected.GetComponent<CapsuleCollider2D>().isTrigger = true;
            transform.localScale = new UnityEngine.Vector3(3, 7.5f, 1);
            GetComponent<Rigidbody2D>().isKinematic = true;
            playerDetected.GetComponent<Rigidbody2D>().isKinematic = true;
            fixedPlayerPosition = transform.position;
            fixedPlayerPosition.y -= 0.25f;
            playerDetected.transform.position = fixedPlayerPosition;
            transform.position = upAttackingPos;


            if (goingUpTimer < goingUpLimit)
            {
                goingUpTimer += Time.deltaTime;
                upAttackingPos.y += goingUpVelocity;
                goingUp = true;
            }
            else
            {
                if (goingUp)
                {
                    goingDownTimer = 0;
                    goingUp = false;
                    goingDown = true;
                }

            }

            if (goingDownTimer < goingDownLimit && goingDown)
            {
                goingDownTimer += Time.deltaTime;
                upAttackingPos.y -= goingDownVelocity;
            }
            else if (goingDown && !(goingDownTimer < goingDownLimit))
            {
                attacking = false;
                searching = true;
                goingDown = false;
                GetComponent<SpriteRenderer>().color = attackingColor;
                playerDetected.GetComponent<CapsuleCollider2D>().isTrigger = false;
                transform.localScale = new UnityEngine.Vector3(2, 2f, 1);
                GetComponent<Rigidbody2D>().isKinematic = false;
                fixedPlayerPosition = playerDetected.transform.position;
                fixedPlayerPosition.x -= 1.5f;
                playerDetected.transform.position = fixedPlayerPosition;
                playerDetected.SendMessage("BarryGotAttacked", damage);

                playerDetected.GetComponent<Rigidbody2D>().isKinematic = false;
                playerDetected.GetComponent<Rigidbody2D>().AddForce(new Vector2(-5, 5), ForceMode2D.Impulse);
            }


        }
        if (firstAttackedTimer > 0)
        {
            firstAttackedTimer -= Time.deltaTime;
        }
        else
        {
            firstAttacked = false;
            GetComponent<Rigidbody2D>().gravityScale = 1f;
        }
    }
    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player" && health > 0 && playerDetected != null)
        {
            goingUpTimer = 0;
            coll.gameObject.SendMessage("freeze", 3.3f);
            upAttackingPos = playerDetected.transform.position;
            attacking = true;
            searching = false;
        }
        if (coll.gameObject.tag == "Ground")
        {
            goingDownTimer = 500;
        }

    }
    void setRadius(GameObject enemy)
    {
        enemy.GetComponent<EnemyDetector>().setNormalRadius(normalRadius);
        enemy.GetComponent<EnemyDetector>().setTauntedRadius(tauntedRadius);
    }
    void Hitted(float damage)
    {
        effectHandler.transform.localScale = new Vector3(1, 1, 1);
        if (firstAttacked)
        {
            health -= damage;
            damageMessagePopUp.GetComponent<TextMeshPro>().text = damage + "";
            Instantiate(damageMessagePopUp, this.gameObject.transform);
            if (playerDetected.transform.position.x < transform.position.x)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * 7, ForceMode2D.Impulse);
            }
            else
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.left * 7, ForceMode2D.Impulse);
            }
            firstAttacked = false;
            GetComponent<Rigidbody2D>().gravityScale = 1f;
        }
        else
        {
            damageMessagePopUp.GetComponent<TextMeshPro>().text = "BLOCKED!!";
            Instantiate(damageMessagePopUp, this.gameObject.transform);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0.3f, 2) * 2, ForceMode2D.Impulse);
            GetComponent<Rigidbody2D>().gravityScale = 0.4f;
            firstAttacked = true;
            firstAttackedTimer = 2f;
        }


    }
    void Die()
    {
        Destroy(this.gameObject);
    }
}
