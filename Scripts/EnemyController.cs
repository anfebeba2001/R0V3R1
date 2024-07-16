using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    

    // Start is called before the first frame update
    private bool taunted;
    private GameObject playerDetected;
    private float attackedCoolDown;
    private bool readyToBeAttacked;
    public GameObject damageMessagePopUp;
    public GameObject tears;
    public GameObject bloodFX;
    private bool hitted;
    private float damageReceived;

    void Start()
    {
        hitted = false;
        taunted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackedCoolDown > 0)
        {
            attackedCoolDown -= Time.deltaTime;
            readyToBeAttacked = false;
        }
        else
        {
            readyToBeAttacked = true;

        }
    }
    void Attacked(float damage)
    {
        if (readyToBeAttacked)
        {
            attackedCoolDown = 0.4f;
          

            SendMessage("Hitted",damage);
              

        }
        
    }
    public void cancelHitted()
    {
        hitted = false;
    }
    public bool getHitted()
    {
        return hitted;
    }
    public float getDamageReceived()
    {
        return damageReceived;
    }
    public void Hitted(float damage)
    {
        damageReceived = damage;
        hitted = true;
    }
    public GameObject getDamageMessagePopUp()
    {
        return damageMessagePopUp;
    }
    public GameObject getBlood()
    {
        return bloodFX;
    }

    //GETTERS
    public bool getTaunted()
    {
        return taunted;
    }
    public GameObject getPlayerDetected()
    {
        return playerDetected;
    }
    //Setters
    public void setTaunted(bool taunted)
    {
        this.taunted = taunted;
    }
    public void setPLayerDetected(GameObject playerDetected)
    {
        this.playerDetected = playerDetected;
    }
    
}
