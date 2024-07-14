using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;

public class BarryController : MonoBehaviour
{
    private float speed = 5.0f;
    private float powerAttack = 20;
    private float jumpingForce = 2.5f;
    private int healingVials = 3;
    private int arrows = 10;
    private float freezeTimer;

    private new Rigidbody2D rigidbody2D;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider2D;
    private bool grounded;
    private bool frozen;
    public bool attacking;
    private bool running;
    public bool readyToAttack;
    private float attackCoolDown = 0;
    private float healingCoolDown;
    private float hurtCoolDown;
    private bool hurt;
    private float maxHealth;
    private float health;
    private float stamina;
    private float maxStamina;
    public GameObject damageMessagePopUp;
    public GameObject arrowPrefab;
    private GameObject mainCamera;
    private float damageOnBow = 10;

    private float bowCoolDown = 3;
    private float staminaCoolDown;
    public bool isBowing;
    private bool freeState;

    private float horizontal;

    private UnityEngine.Vector2 moveDirection;
    private float jumpButtonValue;
    private float healthButtonValue;
    public float attackButtonValue;
    private float bowButtonValue;
    [SerializeField] private InputActionReference moveActionToUse;
    [SerializeField] private InputActionReference jumpActionToUse;
    [SerializeField] private InputActionReference attackActionToUse;
    [SerializeField] private InputActionReference bowActionToUse;
    [SerializeField] private InputActionReference healthActionToUse;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        maxHealth = 100;
        maxStamina = 100;
        health = maxHealth;
        stamina = maxStamina;
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        freeState = true;
    }


    void FixedUpdate()
    {

        if(health <= 0)
        {
            animator.Play("BarryDead");
        }
        
        moveDirection = moveActionToUse.action.ReadValue<UnityEngine.Vector2>();
        jumpButtonValue = jumpActionToUse.action.ReadValue<float>();
        attackButtonValue = attackActionToUse.action.ReadValue<float>();
        bowButtonValue = bowActionToUse.action.ReadValue<float>();
        healthButtonValue = healthActionToUse.action.ReadValue<float>();
        //Movement
        //  //Running


        if(freeState){

            //Running
            Run();

            //Jump
            Jump();

            //Dash
            Dash();
            
            //Attacking
            Attack();

            //BowAttack
            BowAttack();

            //Healing
            Healing();
        }
        

        if (frozen || hurt || health<=0 || isBowing)
        {
            freeState =  false;
        }
        else{
            freeState = true;
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
        if(healingCoolDown > 0 )
        {
            healingCoolDown -= Time.deltaTime;
        }
        if (bowCoolDown > 0)
        {
            bowCoolDown -= Time.deltaTime;
        }
        else
        {
            isBowing = false;
        }
        //Stamina
        if(stamina < maxStamina && Time.time > staminaCoolDown + 1){
            stamina += 0.2f;
        }
    }

    private void Healing(){
        if (healingCoolDown <= 0 && healingVials >= 1 && health<maxHealth && (healthButtonValue > 0 || Input.GetKey(KeyCode.F)))
        {
            healingCoolDown = 1;
            healingVials -= 1;
            animator.Play("BarryHeal");
        }
    }

    private void Run(){
        
        horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal < 0 || moveDirection.x < 0)
        {
            transform.localScale = new Vector3(-2, 2, 1);
        }
        else if(horizontal > 0 || moveDirection.x > 0)
        {
            transform.localScale = new Vector3(2, 2, 1);
        }

        if(horizontal != 0){
            rigidbody2D.velocity = new Vector2(horizontal * speed, rigidbody2D.velocity.y);
            animator.SetBool("Running", horizontal != 0.0f);
        }
        else if (moveDirection.x != 0){
            rigidbody2D.velocity = new Vector2(moveDirection.x * speed, rigidbody2D.velocity.y);
            animator.SetBool("Running", moveDirection.x != 0.0f);
        }
        else{
            animator.SetBool("Running", false);
        }
        
        //animator.Play("BarryRuns");
    }

    private void Jump(){
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || jumpButtonValue == 1) && grounded
            && rigidbody2D.velocity.y == 0)
        {
            rigidbody2D.AddForce(UnityEngine.Vector2.up * jumpingForce * 2, ForceMode2D.Impulse);
            animator.SetBool("Jumping", true);
            animator.Play("BarryJumps");
        }
    }

    private void Dash(){
        if (Input.GetKey(KeyCode.C))
        {
            animator.SetBool("Dashing", true);
            animator.Play("BarryDash");
        }
    }

    private void Attack(){
        if ((Input.GetKey(KeyCode.X) || attackButtonValue > 0) && attackCoolDown <= 0 && stamina >= 30)
        {
            attackCoolDown = 0.9f;
            stamina -= 30;
            staminaCoolDown = Time.time;
            attacking = true;
            animator.Play("BarryAttacks");
        }
    }

    private void BowAttack(){
        if ((Input.GetKey(KeyCode.Z) || bowButtonValue > 0) && bowCoolDown <= 0 && arrows >= 1 && stamina >= 10)
        {
            bowCoolDown = 0.6f;
            arrows -= 1;
            isBowing = true;
            stamina -= 10;
            staminaCoolDown = Time.time;
            animator.Play("BarryBow");
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
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
    public int getHealingVials(){
        return healingVials;
    }
    public int getNumberOfArrows(){
        return arrows;
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
        if (!hurt && hurtCoolDown <= 0)
        {
            hurtCoolDown = 0.5f;
            if(health > 0)
            {animator.Play("BarryHurt");

            }
            
            health -= damage;
            damageMessagePopUp.GetComponent<TextMeshPro>().text = damage + "";
            damageMessagePopUp.GetComponent<DamageMessagePopUpController>().showingTimer = 0.5f;
            Instantiate(damageMessagePopUp, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            mainCamera.GetComponent<Camera>().orthographicSize -= 0.2f;
        }

    }
    void microDamage(float damage)
    {
        if (!hurt)
        {
            damageMessagePopUp.GetComponent<TextMeshPro>().text = damage + "";
            damageMessagePopUp.GetComponent<DamageMessagePopUpController>().showingTimer = 0.5f;
            health -= damage;
            Instantiate(damageMessagePopUp, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            mainCamera.GetComponent<Camera>().orthographicSize -= 0.01f;
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
        Vector3 direction;
        if(transform.localScale.x > 0){
            direction = Vector2.right;
        }
        else{
            direction = Vector2.left;
        }

        GameObject arrow = Instantiate(arrowPrefab, transform.position + direction*0.1f, Quaternion.Euler(0, 0, -90));
        arrow.GetComponent<ArrowController>().SetDirection(direction);
        arrow.GetComponent<ArrowController>().setDamage(damageOnBow);
        isBowing = false;
    }
    void endingAttack()
    {
        attacking = false;
    }

    void endingHEaling()
    {

        if(health < (maxHealth*5/6))
        {
            health += maxHealth/6;
        }
        else{
            health = maxHealth;
        }
    }

    public float getStamina(){
        return stamina;
    }
    public float getMaxStamina(){
        return maxStamina;
    }
    





}
