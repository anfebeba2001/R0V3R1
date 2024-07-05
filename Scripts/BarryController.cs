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
    private bool isParrying;

    void Start()
    {
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
            if(Input.GetKeyDown(KeyCode.Z) && !attacking)
            {
                if(grounded)
                {
                    rigidbody2D.AddForce(UnityEngine.Vector2.up * jumpingForce, ForceMode2D.Impulse);
                }
                
                animator.Play("BarryParry");
                isParrying = true;
                animator.SetBool("Parrying",true);
            }
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
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && grounded)
            {
                rigidbody2D.AddForce(UnityEngine.Vector2.up * jumpingForce, ForceMode2D.Impulse);
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
            if (Input.GetKey(KeyCode.X) && !attacking && readyToAttack && !isParrying)
            {
                attackCoolDown = 0.7f;
                attacking = true;
                animator.Play("BarryAttacks");
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
    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            grounded = true;
            animator.SetBool("Parrying", false);
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
}
