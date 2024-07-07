using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public class BarryController : MonoBehaviour
{
    private float speed = 10.0f;
    private float powerAttack = 30;
    private float jumpingForce = 2.5f;
    private float freezeTimer;

    private new Rigidbody2D rigidbody2D;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider2D;
    private bool grounded;
    private bool frozen;
    private bool attacking;
    private bool running;
    private bool readyToAttack;
    private float attackCoolDown = 0;
    private float hurtCoolDown;
    private bool hurt;
    private float maxHealth;
    private float health;
    public GameObject damageMessagePopUp;
    public GameObject arrow;
    private GameObject mainCamera;
    private float damageOnBow = 10;

    private float bowCoolDown = 3;
    private bool isBowing;

    void Start()
    {
        bowCoolDown = 0;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        maxHealth = 1000;
        health = maxHealth;
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        attacking = false;
    }


    void FixedUpdate()
    {
       
        //Movement
        //  //Running

        if (!frozen && !hurt)
        {
           
            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && rigidbody2D.velocity.x > -2.5)
            {
                rigidbody2D.AddForce(UnityEngine.Vector2.left * speed, ForceMode2D.Force);
                transform.localScale = new UnityEngine.Vector3(-2, 2, 1);
                running = true;

            }
            else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && rigidbody2D.velocity.x < 2.5)
            {
                rigidbody2D.AddForce(UnityEngine.Vector2.right * speed, ForceMode2D.Force);
                transform.localScale = new UnityEngine.Vector3(2, 2, 1);
                running = true;

            }
            else
            {
                running = false;
            }


            //  //Jumping
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && grounded
            && rigidbody2D.velocity.y == 0)
            {
                rigidbody2D.AddForce(UnityEngine.Vector2.up * jumpingForce * 2, ForceMode2D.Impulse);
                animator.SetBool("Jumping", true);
                animator.Play("BarryJumps");
            }
            //Moving animations
            if (!running)
            {
                animator.SetBool("Running", false);
            }
            else
            {
                animator.SetBool("Running", true);
            }
            //Atacks
            if (Input.GetKey(KeyCode.X) && !attacking && readyToAttack )
            {
                attackCoolDown = 0.7f;
                attacking = true;
                animator.Play("BarryAttacks");
            }
            //Bow
            if (Input.GetKeyDown(KeyCode.Z) && !attacking && bowCoolDown <= 0 && !isBowing)
            {
                animator.Play("BarryBow");
                bowCoolDown = 0.6f;
                isBowing = true;
                attacking = true;
            }
        }


        //Timers
        //  //Attack
        if (attackCoolDown > 0)
        {
            readyToAttack = false;
            attackCoolDown -= Time.deltaTime;
        }
        else
        {
            attacking = false;
            readyToAttack = true;
        }
        //  //Freeze
        if (freezeTimer > 0)
        {
            frozen = true;
            freezeTimer -= Time.deltaTime;
        }
        else
        {
            frozen = false;
        }
        //  //Hurt
        if (hurtCoolDown > 0)
        {
            hurt = true;
            hurtCoolDown -= Time.deltaTime;
        }
        else
        {
            hurt = false;
        }
        if(bowCoolDown > 0)
        {
            bowCoolDown -= Time.deltaTime;
        }
        else
        {
            isBowing = false;
        }
    }
    //Getters
    public float getHealth()
    {
        return health;
    }
    public float getMaxHealth()
    {
        return maxHealth;
    }
    public float getDamageOnBow()
    {
        return damageOnBow;
    }





    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
        {

            grounded = true;
  

            animator.SetBool("Jumping", false);
        }
    }
    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
            grounded = false;

    }



    void freeze(float time)
    {
        freezeTimer = time;
    }
    void BarryGotAttacked(float damage)
    {
        if (!hurt)
        {
            hurtCoolDown = 0.5f;
            animator.Play("BarryHurt");
            health -= damage;
            damageMessagePopUp.GetComponent<TextMeshPro>().text = damage + "";

            Instantiate(damageMessagePopUp, this.gameObject.transform);
            mainCamera.GetComponent<Camera>().orthographicSize -= 0.2f;
        }

    }

    public bool getAttacking()
    {
        return attacking;
    }
    public float getPowerAttack()
    {
        return powerAttack;
    }

    void instantiateArrow()
    {
        Instantiate(arrow, this.gameObject.transform);
        isBowing = false;
    }
}
