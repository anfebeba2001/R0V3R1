using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private GameObject player;
    public GameObject mainCamera;
    private bool taunted;
    public GameObject enemyHealthLifeBar;
    public GameObject dialogHandler;
    public float maxLimitPos;
    public float minLimitPos;

    private string[][] dialogs = new string[4][];
    private float dialogTimer;
    private float maxHealth;
    private float health;

    void Start()
    {
        taunted = false;        
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(taunted)
        {
          player.SendMessage("fightingBoss",false);  
        }
        if(!taunted && player.transform.position.x <  maxLimitPos && player.transform.position.x > minLimitPos && player.transform.position.y > transform.position.y - 0.5f && player.transform.position.y < transform.position.y + 0.5f)
        {
            enemyHealthLifeBar.SetActive(true);
            dialog("Start");            
            enemyHealthLifeBar.gameObject.SetActive(true);
            player.SendMessage("fightingBoss",true);
            taunted = true;
        }

        if(dialogTimer > 0)
        {
            dialogTimer -= Time.deltaTime;
        }
        else{
            dialogHandler.SetActive(false);
        }
    }
    public void dialog(string phrase)
    {
        dialogTimer = 1.5f;
        dialogHandler.SetActive(true);
        dialogHandler.GetComponent<Animator>().Play("DialogBeginning");
        switch(phrase)
        {
            case "Start":
                int choise = Random.Range(1,40);
                if(choise%4 == 0)
                    dialogHandler.GetComponent<TextMeshProUGUI>().text = dialogs[0][0];
                else if(choise%4 == 1)
                    dialogHandler.GetComponent<TextMeshProUGUI>().text = dialogs[0][1];
                else if(choise%4 == 2)
                    dialogHandler.GetComponent<TextMeshProUGUI>().text = dialogs[0][2];
                 else if(choise%4 == 3)
                    dialogHandler.GetComponent<TextMeshProUGUI>().text = dialogs[0][3];        
            break;
            case "Attack":
                int choiseAttack = Random.Range(1,40);
                if(choiseAttack%4 == 0)
                    dialogHandler.GetComponent<TextMeshProUGUI>().text = dialogs[1][0];
                else if(choiseAttack%4 == 1)
                    dialogHandler.GetComponent<TextMeshProUGUI>().text = dialogs[1][1];
                else if(choiseAttack%4 == 2)
                    dialogHandler.GetComponent<TextMeshProUGUI>().text = dialogs[1][2];
                 else if(choiseAttack%4 == 3)
                    dialogHandler.GetComponent<TextMeshProUGUI>().text = dialogs[1][3];  
            break;
            case "Losing":
            int coiseLosing = Random.Range(1,40);
                if(coiseLosing%4 == 0)
                    dialogHandler.GetComponent<TextMeshProUGUI>().text = dialogs[2][0];
                else if(coiseLosing%4 == 1)
                    dialogHandler.GetComponent<TextMeshProUGUI>().text = dialogs[2][1];
                else if(coiseLosing%4 == 2)
                    dialogHandler.GetComponent<TextMeshProUGUI>().text = dialogs[2][2];
                 else if(coiseLosing%4 == 3)
                    dialogHandler.GetComponent<TextMeshProUGUI>().text = dialogs[2][3];  
            break;
        }
    }
    public void setDialogs(string[][] dialogsPrefabs)
    {
        dialogs = dialogsPrefabs;
    }
    public bool getTaunted ()
    {
        return taunted;
    }
    public float getMaxPos()
    {
        return maxLimitPos;
    }
    public float getMinPos()
    {
        return minLimitPos;
    }

    internal float getMaxHealth()
    {
        return maxHealth;
    }

    internal float getHealth()
    {
        return health;
    }
    public void setMaxHealth( float maxHealth)
    {
        this.maxHealth = maxHealth;
    }
    public void setHealth(float health)
    {
        this.health = health;
    }
}
