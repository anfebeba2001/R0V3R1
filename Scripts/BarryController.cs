using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

//using System.Numerics;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BarryController : MonoBehaviour
{
    public GameObject leftButton;
    public GameObject rightButton;
    public float speed;
    private float powerAttack = 25;
    public float jumpingForce = 2.5f;
    private int healingVials = 3;
    private int arrows = 10;
    public float dashForce;
    private bool firstAttack;
    private bool secondAttack;
    private bool thirdAttack;
    private float freezeTimer;
    private bool groundTouched;
    private int currentTears;

    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider2D;
    private bool grounded;
    private bool frozen;
    private bool attacking;
    private bool running;
    private bool readyToAttack;
    private float attackCoolDown = 0;
    private float comboAttackTimer = 0;
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
    private float dashCoolDown;
    private float staminaCoolDown;
    private bool isBowing;
    private bool isDashing;
    private bool freeState;
    public float maxSpeed;
    private float horizontal;

    private UnityEngine.Vector2 moveDirection;
    private float jumpButtonValue;
    private float healthButtonValue;
    private float attackButtonValue;
    private float bowButtonValue;
    private UnityEngine.Vector3[] originalButtonPosition = new UnityEngine.Vector3[2];
    [SerializeField] private InputActionReference moveActionToUse;
    [SerializeField] private InputActionReference jumpActionToUse;
    [SerializeField] private InputActionReference attackActionToUse;
    [SerializeField] private InputActionReference bowActionToUse;
    [SerializeField] private InputActionReference healthActionToUse;

    private bool laddering;
    private float ladderingCoolDown;

    void Start()
    {
        
        originalButtonPosition[0] = rightButton.transform.position;
        originalButtonPosition[1] = leftButton.transform.position;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        maxHealth = 1000;
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
            FirstAttack();
            SecondAttack();
            ThirdAttack();

            //BowAttack
            BowAttack();

            //Healing
            Healing();
        }


        if(laddering)
        {
            ladderingAction();
        }
        

        if (frozen || hurt || health<=0 || isBowing || isDashing || laddering)
        {
            freeState =  false;
        }
        else{
            freeState = true;
        }


        if(ladderingCoolDown > 0)
        {
            ladderingCoolDown -= Time.deltaTime;
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
        //Dash
        if(dashCoolDown >0){
            dashCoolDown -= Time.deltaTime;
        }
        //ResetCombo
        if(comboAttackTimer >= 0){
            comboAttackTimer += Time.deltaTime;
        }
        if(comboAttackTimer >= 0.8f){
            firstAttack = false;
            secondAttack = false;
            thirdAttack = false;
        }
    }

    private void ladderingAction()
    {
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.UpArrow) || moveDirection.x > 0)
        {
            GetComponent<Animator>().Play("BarryLadderingMove");
            transform.position += new Vector3(0,1,0)*0.05f;
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.DownArrow) || moveDirection.x < 0)
        {
            GetComponent<Animator>().Play("BarryLadderingMove");
            transform.position += new Vector3(0,-1,0)*0.05f;
        }
        else 
        {
            GetComponent<Animator>().Play("BarryLadderingStay");
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.tag == "Ladder")
        {
            if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || jumpButtonValue == 1) && !laddering && !hurt && ladderingCoolDown <= 0)
            {
                if(transform.position.x > col.gameObject.transform.position.x )
                    transform.localScale = new Vector3(-2, 2, 1);
                else
                    transform.localScale = new Vector3(2, 2, 1);
                laddering = true;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);    
                GetComponent<Rigidbody2D>().isKinematic = true;      
                rightButton.transform.position = leftButton.transform.position;
                rightButton.transform.position += new Vector3(0,1,0)*270;
                rightButton.transform.rotation = new quaternion(0, 0, 180 , 0);
                leftButton.transform.rotation = new quaternion(0, 0, 0 , 0);
            }
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Ladder")
        {
            laddering = false;
            rightButton.transform.rotation = quaternion.Euler(0,0,-67.5f);
            leftButton.transform.rotation = quaternion.Euler(0,0,67.5f);
            rightButton.transform.position = originalButtonPosition[0];
            leftButton.transform.position = originalButtonPosition[1];
            GetComponent<Animator>().Play("BarryRuns");
            if(GetComponent<Rigidbody2D>().isKinematic)
                GetComponent<Rigidbody2D>().isKinematic = false;

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


        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || moveDirection.x < 0) && rigidbody2D.velocity.x > -maxSpeed)
        {
            rigidbody2D.AddForce(UnityEngine.Vector2.left * speed, ForceMode2D.Force);
            transform.localScale = new Vector3(-2, 2, 1);
            if(rigidbody2D.velocity.x < -0.3f){
                animator.SetBool("Running", true);
            }
            
        }
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || moveDirection.x > 0) && rigidbody2D.velocity.x <maxSpeed)
        {
            rigidbody2D.AddForce(Vector2.right * speed, ForceMode2D.Force);
            transform.localScale = new Vector3(2, 2, 1);
            if(rigidbody2D.velocity.x > 0.3f){
                animator.SetBool("Running", true);
            }

        }
        else if(!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
        {
            animator.SetBool("Running", false);
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        }



        /*horizontal = Input.GetAxisRaw("Horizontal");

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
        }*/
    }

    private void Jump(){
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || jumpButtonValue == 1) && grounded
            && rigidbody2D.velocity.y == 0)
        {
            rigidbody2D.AddForce(Vector2.up * jumpingForce * 2, ForceMode2D.Impulse);
            animator.SetBool("Jumping", true);
            //animator.Play("BarryJumps");
        }
    }

    private void Dash(){
        if (Input.GetKey(KeyCode.C) && dashCoolDown <= 0 && !isDashing && groundTouched)
        {
            if(transform.localScale.x > 0){
                rigidbody2D.AddForce(new Vector2(dashForce, 0), ForceMode2D.Impulse);
            }
            else{
                rigidbody2D.AddForce(new Vector2(-dashForce, 0), ForceMode2D.Impulse);
            }
            isDashing = true;
            groundTouched = false;
            animator.Play("BarryDash");
            rigidbody2D.isKinematic = true;
        }
    }
    public void EndDash(){
        rigidbody2D.isKinematic = false;
        rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        isDashing = false;
        dashCoolDown = 0.5f;
    }

    private void FirstAttack(){


        if (Input.GetKey(KeyCode.X) && !firstAttack && !secondAttack && !thirdAttack && attackCoolDown <= 0)
        {
            animator.Play("BarryAttacks");
            attackCoolDown = 0.9f;
            attacking = true;
            firstAttack = true;
            //stamina -= 30;
            comboAttackTimer = 0;
        }
    }

    private void SecondAttack(){
        if (Input.GetKey(KeyCode.X) && firstAttack && !secondAttack && !thirdAttack && (comboAttackTimer >= 0.5f) && (comboAttackTimer <= 0.8f))
        {
            animator.Play("BarryAttack2");
            comboAttackTimer = 0;
            //stamina -= 30;
            firstAttack = false;
            secondAttack = true;
            staminaCoolDown = Time.time;
            attacking = true;
        }
    }

    private void ThirdAttack(){
        if (Input.GetKey(KeyCode.X) && !firstAttack && secondAttack && !thirdAttack && (comboAttackTimer >= 0.5f) && (comboAttackTimer <= 0.8f))
        {
            animator.Play("BarryAttack3");
            attackCoolDown = 0.9f;
            //stamina -= 30;
            secondAttack = false;
            thirdAttack = true;
            staminaCoolDown = Time.time;
            attacking = true;
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
            groundTouched = true;

            grounded = true;
            if(rigidbody2D.velocity.y <= 0){
                animator.SetBool("Jumping", false);
            }
            
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
    public void BarryGotAttacked(float damage)
    {
        if (!hurt && hurtCoolDown <= 0)
        {
            if(isDashing){
                isDashing = false;
            }
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
    public void microDamage(float damage)
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
    
    

    public int getAttackState(){
        if(firstAttack){
            return 1;
        }
        else if(secondAttack){
            return 2;
        }
        else{
            return 3;
        }
    }

    public void addTears(int tearsIncoming)
    {
        currentTears += tearsIncoming;
    }
    public int getCurrentTears()
    {
        return currentTears;
    }
}
