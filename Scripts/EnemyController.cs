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
    void Start()
    {
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
            damageMessagePopUp.GetComponent<TextMeshPro>().text = damage + "";

            SendMessage("Hitted",damage);
            Instantiate(damageMessagePopUp, this.gameObject.transform);

        }
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
