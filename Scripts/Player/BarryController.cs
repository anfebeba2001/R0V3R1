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
    private GameObject leftButton;
    private GameObject rightButton;
    public Sprite upButtonSprite;
    public float speed;
    private int powerAttack = 25;
    public float jumpingForce = 2.5f;
    private int healingVials;
    private int costPerUpgrade;
    private int arrows;
    public float dashForce;
    private bool firstAttack;
    private bool secondAttack;
    private bool thirdAttack;
    private float freezeTimer;
    private bool groundTouched;
    public int currentTears;
    private GameObject buffsHelper;

    private bool canMove;

    private new Rigidbody2D rigidbody2D;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider2D;
    private bool grounded;
    private bool frozen;
    private bool attacking;
    private bool running;
    private float attackCoolDown = 0;
    private float comboAttackTimer = 0;
    private float healingCoolDown;
    private float hurtCoolDown;
    private bool hurt;
    private int defense;
    private int maxHealth;
    private int resistance;
    private int health;
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
    private float dashVelocity = 6.0f;
    public float dashTime = 0.4f;
    private float initialGravity;

    private IEnumerator coroutineFAT;
    private IEnumerator coroutineSAT;

    public int powerBoofModifier{get; private set;}

    private Vector2 moveDirection;
    private float jumpButtonValue;
    private float healthButtonValue;
    private float attackButtonValue;
    private float bowButtonValue;
    private float dashButtonValue;
    public SceneTravelerController sceneTravelerController;
    private Vector3[] originalButtonPosition = new Vector3[2];
    private Quaternion[] originalButtonRotation = new Quaternion[2];
    [SerializeField] private InputActionReference moveActionToUse;
    [SerializeField] private InputActionReference jumpActionToUse;
    [SerializeField] private InputActionReference dashActionToUse;
    [SerializeField] private InputActionReference attackActionToUse;
    [SerializeField] private InputActionReference bowActionToUse;
    [SerializeField] private InputActionReference healthActionToUse;

    private bool laddering;
    private float ladderingCoolDown;
    private bool fightingBossBool;
    private bool alreadySavedTearsCrystal;
    private int defenseBoofModifier;

    void Start()
    {
        /*
        maxHealth = 100;
            resistance = 50;
            defense = 5;
            powerAttack = 30;
            arrows = 5;
            healingVials = 5;
            costPerUpgrade = 400;
            currentTears = 0;
        SaveManager.savePlayerData(defense,maxHealth,powerAttack,resistance,currentTears,healingVials,arrows,costPerUpgrade);
        */

        buffsHelper = GameObject.FindGameObjectWithTag("BuffsHelper");
        rightButton = GameObject.FindGameObjectWithTag("RightButton");
        leftButton = GameObject.FindGameObjectWithTag("LeftButton");
        PlayerData loadedPlayerData =  SaveManager.loadPlayerData();
        if(loadedPlayerData != null)
        {
            
            maxHealth = loadedPlayerData.health;
            resistance = loadedPlayerData.resistance;
            defense = loadedPlayerData.defense;
            powerAttack = loadedPlayerData.damage;
            arrows = loadedPlayerData.arrows;
            healingVials = loadedPlayerData.healingOrbs;
            costPerUpgrade = loadedPlayerData.costPerUpgrade;
            currentTears = loadedPlayerData.tears;
        }
        else
        {
            maxHealth = 100;
            resistance = 50;
            defense = 5;
            powerAttack = 30;
            arrows = 5;
            healingVials = 5;
            costPerUpgrade = 400;
            currentTears = 0;
        }
        alreadySavedTearsCrystal = false;   
        originalButtonPosition[0] = rightButton.transform.position;
        originalButtonPosition[1] = leftButton.transform.position;
        originalButtonRotation[0] = rightButton.transform.rotation;
        originalButtonRotation[0] = leftButton.transform.rotation;


        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        
        maxStamina = resistance;
        health = maxHealth;
        stamina = maxStamina;
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        freeState = true;

        initialGravity = rigidbody2D.gravityScale;
        
        coroutineFAT = FirstAirAttack();
        coroutineSAT = SecondAirAttack();
        canMove = true;
    }


    void FixedUpdate()
    {
        

        if(health <= 0)
        {
            animator.Play("BarryDead");
            if(!alreadySavedTearsCrystal)
            {
                alreadySavedTearsCrystal = true;
                float[] positionForTears = new float[3];
                positionForTears[0] = transform.position.x;
                positionForTears[1] = transform.position.y;
                positionForTears[2] = transform.position.z;
                SaveManager.saveDroppenTearsData(currentTears,positionForTears);
                currentTears = 0;
                SaveManager.savePlayerData(defense,maxHealth,powerAttack,resistance,currentTears,healingVials,arrows,costPerUpgrade);
            }
        }
        
        moveDirection = moveActionToUse.action.ReadValue<UnityEngine.Vector2>();
        jumpButtonValue = jumpActionToUse.action.ReadValue<float>();
        attackButtonValue = attackActionToUse.action.ReadValue<float>();
        bowButtonValue = bowActionToUse.action.ReadValue<float>();
        healthButtonValue = healthActionToUse.action.ReadValue<float>();
        dashButtonValue = dashActionToUse.action.ReadValue<float>();


        if (frozen || hurt || health<=0 || isBowing || isDashing || laddering)
        {
            freeState =  false;
        }
        else{
            freeState = true;
        }

        if(freeState){

            //Running
            Run();
            

            //Jump
            Jump();

            //Dash
            if((Input.GetKey(KeyCode.C) || dashButtonValue == 1) && dashCoolDown <= 0 && !isDashing && groundTouched && stamina >= 8){
                StartCoroutine(Dash());
                stamina -= 8;
            }
            
            //Attacking
            FirstAttack();
            SecondAttack();
            ThirdAttack();

            //AirAttacking
            if ((Input.GetKey(KeyCode.X) || attackButtonValue == 1) && !grounded && !firstAttack && !secondAttack && !thirdAttack && attackCoolDown <= 0&& stamina >= 12)
            {
                StartCoroutine(FirstAirAttack());
            }

            if ((Input.GetKey(KeyCode.X) || attackButtonValue == 1) && !grounded && firstAttack && !secondAttack && !thirdAttack && (comboAttackTimer >= 0.5f) && (comboAttackTimer <= 0.8f) && stamina >= 12)
            {
                StartCoroutine(SecondAirAttack());
            }

            if ((Input.GetKey(KeyCode.X) || attackButtonValue == 1) && !grounded && !firstAttack && secondAttack && !thirdAttack && (comboAttackTimer >= 0.5f) && (comboAttackTimer <= 0.8f) && stamina >= 12)
            {
                ThirdAirAttack();
            }

            

            //BowAttack
            BowAttack();

            //Healing
            Healing();

            //Faling
            Falling();
        }


        if(laddering)
        {
            ladderingAction();
        }


        if(ladderingCoolDown > 0)
        {
            ladderingCoolDown -= Time.deltaTime;
        }
        //Timers
        //  //Attack
        if (attackCoolDown > 0)
        {
            attackCoolDown -= Time.deltaTime;
        }
        else
        {
            attacking = false;
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
        //Reset Attack Combo Timer
        if(comboAttackTimer >= 0){
            comboAttackTimer += Time.deltaTime;
        }
        //Reset Attack Combo
        if(comboAttackTimer >= 0.8f){
            if(grounded){
                firstAttack = false;
                secondAttack = false;
                thirdAttack = false;
            }
            else if(!grounded && (firstAttack || secondAttack)){
                
                rigidbody2D.gravityScale = initialGravity;
                firstAttack = false;
                secondAttack = false;
            }
            canMove = true;
        }
        
        //Keep Barry in air while air attack combo
        if(!grounded && secondAttack){
            StopCoroutine(coroutineFAT);
        }
        else if(!grounded && thirdAttack){
            StopCoroutine(coroutineSAT);
        }
    }

    private void ladderingAction()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || moveDirection.x > 0)
        {
            GetComponent<Animator>().Play("BarryLadderingMove");
            transform.position += new Vector3(0,1,0)*0.05f;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || moveDirection.x < 0)
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
        if(col.gameObject.tag == "Door")
        {
            if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || jumpButtonValue == 1))
            {
                col.GetComponent<DoorController>().transportBarry(gameObject);                
            }
        }
        if(col.gameObject.tag == "WorldLadder")
        {
            if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || jumpButtonValue == 1))
            {
                if(col.gameObject.name == "WorldLadder")
                {
                    sceneTravelerController.World();
                }
                else if(col.gameObject.name == "Purgatory")
                {
                    SaveManager.savePlayerData(defense,maxHealth,powerAttack,resistance,currentTears,healingVials,arrows,costPerUpgrade);
                    sceneTravelerController.Purgatory();
                }
                
            }
        }
        if(col.gameObject.tag == "Ladder" && !fightingBossBool)
        {
            if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || jumpButtonValue == 1) && !laddering && !hurt && ladderingCoolDown <= 0 && health > 0)
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
                leftButton.transform.rotation = new quaternion(0, 0, 180 , 0);
                rightButton.transform.rotation = new quaternion(0, 0, 0 , 0);
            }
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Ladder")
        {
            laddering = false;
            leftButton.transform.rotation =  Quaternion.Euler(0, 0, -270);
            rightButton.transform.rotation =  Quaternion.Euler(0, 0, 270);
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


        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || moveDirection.x < 0) && canMove && rigidbody2D.velocity.x > -maxSpeed)
        {
            rigidbody2D.AddForce(Vector2.left * speed, ForceMode2D.Force);
            transform.localScale = new Vector3(-2, 2, 1);
            if(rigidbody2D.velocity.x < -0.3f){
                animator.SetBool("Running", true);
            }
            
        }
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || moveDirection.x > 0) && canMove && rigidbody2D.velocity.x <maxSpeed)
        {
            rigidbody2D.AddForce(Vector2.right * speed, ForceMode2D.Force);
            transform.localScale = new Vector3(2, 2, 1);
            if(rigidbody2D.velocity.x > 0.3f){
                animator.SetBool("Running", true);
            }

        }
        else if(!(Input.GetKey(KeyCode.A) || moveDirection.x != 0 ||Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
        {
            animator.SetBool("Running", false);
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        }
    }

    private void Jump(){
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || jumpButtonValue == 1) && grounded
            && rigidbody2D.velocity.y == 0)
        {
            buffsHelper.GetComponent<BuffsOnBarryHelper>().jumped();
            rigidbody2D.AddForce(Vector2.up * jumpingForce * 2, ForceMode2D.Impulse);
            animator.SetBool("Jumping", true);
        }
    }

    private void Falling(){
        if(rigidbody2D.velocity.y < 0){
            animator.SetBool("Falling", true);
        }
    }

    private IEnumerator Dash(){

        isDashing = true;
        rigidbody2D.gravityScale = 0;
        rigidbody2D.velocity = new Vector2(dashVelocity * (transform.localScale.x/2), 0);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemies"), true);
        animator.Play("BarryDash");

        dashCoolDown = 1.0f;

        yield return new WaitForSeconds(dashTime);
        
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemies"), false);
        isDashing = false;
        rigidbody2D.gravityScale = initialGravity;
    }

    private void FirstAttack(){


        if ((Input.GetKey(KeyCode.X) || attackButtonValue == 1) && grounded && !firstAttack && !secondAttack && !thirdAttack && attackCoolDown <= 0 && stamina >= 12)
        {
            animator.Play("BarryAttacks");
            attackCoolDown = 0.9f;
            attacking = true;
            firstAttack = true;
            stamina -= 12;
            comboAttackTimer = 0;
        }
    }

    private void SecondAttack(){
        if ((Input.GetKey(KeyCode.X) || attackButtonValue == 1) && stamina >= 12 && grounded && firstAttack && !secondAttack && !thirdAttack && (comboAttackTimer >= 0.5f) && (comboAttackTimer <= 0.8f))
        {
            animator.Play("BarryAttack2");
            comboAttackTimer = 0;
            stamina -= 12;
            firstAttack = false;
            secondAttack = true;
            staminaCoolDown = Time.time;
            attacking = true;
        }
    }

    private void ThirdAttack(){
        if ((Input.GetKey(KeyCode.X) || attackButtonValue == 1) && stamina >= 12 && grounded && !firstAttack && secondAttack && !thirdAttack && (comboAttackTimer >= 0.5f) && (comboAttackTimer <= 0.8f))
        {
            animator.Play("BarryAttack3");
            attackCoolDown = 0.9f;
            stamina -= 12;
            secondAttack = false;
            thirdAttack = true;
            staminaCoolDown = Time.time;
            attacking = true;
        }
    }

    private IEnumerator FirstAirAttack(){
        if ((Input.GetKey(KeyCode.X) || attackButtonValue == 1) && !grounded && !firstAttack && !secondAttack && !thirdAttack && attackCoolDown <= 0)
        {
            canMove = false;
            firstAttack = true;
            rigidbody2D.gravityScale = 0;
            rigidbody2D.velocity = new Vector2(0, 0);
            animator.Play("BarryAirAttack1");
            comboAttackTimer = 0;
            attackCoolDown = 0.9f;            
            stamina -= 12;
            staminaCoolDown = Time.time;
            attacking = true;

            yield return new WaitForSeconds(0.65f);

            rigidbody2D.gravityScale = initialGravity;
        }
    }

    private IEnumerator SecondAirAttack(){
        if ((Input.GetKey(KeyCode.X) || attackButtonValue == 1) && !grounded && firstAttack && !secondAttack && !thirdAttack && (comboAttackTimer >= 0.5f) && (comboAttackTimer <= 0.8f))
        {
            canMove = false;
            firstAttack = false;
            secondAttack = true;
            rigidbody2D.gravityScale = 0;
            rigidbody2D.velocity = new Vector2(0, 0);
            animator.Play("BarryAirAttack2");
            comboAttackTimer = 0;
            attackCoolDown = 0.9f;
            staminaCoolDown = Time.time;
            stamina -= 12;
            attacking = true;

            yield return new WaitForSeconds(0.65f);

            rigidbody2D.gravityScale = initialGravity;
        }
    }

    private void ThirdAirAttack(){
        canMove = false;
        secondAttack = false;
        thirdAttack = true;
        rigidbody2D.gravityScale = initialGravity;
        rigidbody2D.velocity = new Vector2(0, -2);
        animator.Play("BarryAirAttack3");
        comboAttackTimer = 0;
        attackCoolDown = 0.9f;
        stamina -= 12;
        staminaCoolDown = Time.time;
        attacking = true;
    }
    public IEnumerator ThirdAirAttackEnd(){

        animator.Play("BarryAirAttack3-End");
        thirdAttack = false;
        GetComponent<CapsuleCollider2D>().isTrigger = true;

        yield return new WaitUntil(() => grounded == true);

        GetComponent<CapsuleCollider2D>().isTrigger = false;


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
    public int getMaxHealth()
    {
        return maxHealth;
    }
    public int getHealingVials(){
        return healingVials;
    }
    public int getNumberOfArrows(){
        return arrows;
    }
    public bool getGrounded(){
        return grounded;
    }
    public bool getThirdAttack(){
        return thirdAttack;
    }
    public void setThirdAttack(bool state){
        thirdAttack = state;
    }
    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            groundTouched = true;

            animator.SetBool("Falling", false);

            grounded = true;
            if(rigidbody2D.velocity.y <= 0){
                animator.SetBool("Jumping", false);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        
        
    }
    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
            grounded = false;

    }
    private void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Ground")
        {
            grounded = true;
            
        }
        if (coll.gameObject.name == "EndOfTheGround")
        {
            health = -100;
        }  
         
    }
    void freeze(float time)
    {
        freezeTimer = time;
    }
    public void BarryGotAttacked(float damage)
    {
        if (!hurt && hurtCoolDown <= 0 && health > 0)
        {
            GetComponent<AudioSource>().Play();
            if(isDashing){
                isDashing = false;
            }
            hurtCoolDown = 0.5f;
            if(health > 0)
            {animator.Play("BarryHurt");

            }
            
            if(damage > (defense + defenseBoofModifier))
            {
                health -= (int)(damage - (defense + defenseBoofModifier));
                damageMessagePopUp.GetComponent<TextMeshPro>().text = ((int)(damage - (defense + defenseBoofModifier))) + "";
            }
            else
            {
                damageMessagePopUp.GetComponent<TextMeshPro>().text = 0 + "";
            }
            
            
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
            health -= (int)damage;
            Instantiate(damageMessagePopUp, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            mainCamera.GetComponent<Camera>().orthographicSize -= 0.01f;
        }
    }

    public bool getAttacking()
    {
        return attacking;
    }
    public int getPowerAttack()
    {
        return powerAttack;
    }

    public int getPowerAttackToMobs()
    {
        return powerAttack+powerBoofModifier;
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
    void fightingBoss()
    {
        fightingBossBool = true;
    }
    public int getCostPerUpgrade()
    {
        return costPerUpgrade;
    }

    public void setHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    public int getDefense()
    {
        return defense;
    }

    public int getResistance()
    {
        return resistance;
    }

    internal void setPowerAttack(int powerAttack)
    {
        this.powerAttack = powerAttack;
    }

    internal void setDefense(int defense)
    {
        this.defense = defense;
    }

    internal void setResistance(int resistance)
    {
        this.resistance = resistance;
    }

    internal void setCurrentTears(int currentTears)
    {
        this.currentTears = currentTears;
    }

    internal void setCostPerUpgrade(int costPerUpgrade)
    {
        this.costPerUpgrade = costPerUpgrade;
    }

    internal void setHealingVials(int healingVials)
    {
        this.healingVials = healingVials;
    }
    internal void setNumberOfArrows(int arrows)
    {
        this.arrows = arrows;
    }

    internal void heal(int amount)
    {
        if(health < maxHealth - amount)
        {
            health += (int)amount;
            damageMessagePopUp.GetComponent<TextMeshPro>().text = "+" + amount + "";
            damageMessagePopUp.GetComponent<DamageMessagePopUpController>().showingTimer = 0.5f;
            Instantiate(damageMessagePopUp, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        }
    }

    internal void setPowerBoofModifier(int powerBoofModifier)
    {
        this.powerBoofModifier = powerBoofModifier;
    }
    internal int getPowerBoofModifier( )
    {
        return powerBoofModifier;
    }
    internal void setDefenseBoofModifier(int defenseBoofModifier)
    {
        this.defenseBoofModifier = defenseBoofModifier;
    }
    internal int getDefenseBoofModifier( )
    {
        return defenseBoofModifier;
    }
}
