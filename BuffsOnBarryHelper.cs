using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BuffsOnBarryHelper : MonoBehaviour
{
    private GameObject player;
    public GameObject buffoSlash;
    public Color unrevealedColor;
    public Color RevealedColor;

    public GameObject[] buffosScreen = new GameObject[4];
    private bool[] buffos = new bool[16];
    public Sprite[] buffosSprites = new Sprite[16];
    private int buffoCounter;

    public GameObject raisingSun;
    private float quietPlaceCoolDown;
    private Vector3 originalScale;
    private float injusticeCoolDown;

    internal void AddBuff()
    {
        int selection = Random.Range(1,1600);
        if(buffos[selection%16])
        {
            AddBuff();
        }
        else
        {
            buffosScreen[buffoCounter].GetComponent<UnityEngine.UI.Image>().sprite = buffosSprites[selection%16];
            buffos[selection%16] = true;
            buffoCounter++;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        originalScale = transform.localScale;
        raisingSun.SetActive(false);
        buffoCounter = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<BarryController>().setPowerBoofModifier(0);
        player.GetComponent<BarryController>().setDefenseBoofModifier(0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
        
        



        if(buffos[0])
        {
            raisingSun.SetActive(true);
        }
        if(buffos[1])
        {
            if(quietPlaceCoolDown > 0)
            {
                quietPlaceCoolDown -= Time.deltaTime;
            }
            else
            {
                quietPlaceCoolDown = 40f;
                player.GetComponent<BarryController>().heal((int)(player.GetComponent<BarryController>().getMaxHealth()*0.18f));
            }
        }
        if(buffos[2])
        {
            //Occuped
        }
        if(buffos[3])
        {
            player.GetComponent<BarryController>().setPowerBoofModifier((int)(player.GetComponent<BarryController>().getPowerBoofModifier() + (player.GetComponent<BarryController>().getPowerAttack() * 0.2f )));
            player.GetComponent<BarryController>().setDefenseBoofModifier((int)(player.GetComponent<BarryController>().getDefenseBoofModifier() + (player.GetComponent<BarryController>().getDefense() * 0.2f )));
        }
        if(buffos[5])
        {
            player.GetComponent<BarryController>().setPowerBoofModifier((int)(player.GetComponent<BarryController>().getPowerBoofModifier() + (player.GetComponent<BarryController>().getPowerAttack() * 0.8f )));
        }
        if(buffos[6])
        {
            player.GetComponent<BarryController>().setDefenseBoofModifier((int)(player.GetComponent<BarryController>().getDefenseBoofModifier() + (player.GetComponent<BarryController>().getDefense() * 0.8f )));
        }
        if(buffos[7])
        {

        }
        if(buffos[8])
        {
            player.GetComponent<BarryController>().setPowerBoofModifier((int)(player.GetComponent<BarryController>().getPowerBoofModifier() - (player.GetComponent<BarryController>().getPowerAttack() * 0.8f )));
        }
        if(buffos[9])
        {
            player.GetComponent<BarryController>().setDefenseBoofModifier((int)(player.GetComponent<BarryController>().getDefenseBoofModifier() + (player.GetComponent<BarryController>().getDefense() * 0.8f )));
        }
        if(buffos[11])
        {
            
            if(injusticeCoolDown > 0)
            {
                injusticeCoolDown -= Time.deltaTime;
            }
            else
            {
                injusticeCoolDown = 15f;
                player.GetComponent<BarryController>().microDamage(15);
            }
        }
        if(buffos[0])
        {

        }
        if(buffos[0])
        {

        }
        if(buffos[0])
        {

        }
        if(buffos[0])
        {

        }
        if(buffos[0])
        {

        }
        if(buffos[0])
        {

        }
    }

    internal void AttackingAnEnemy()
    {
        if(buffos[2])
        {
            buffoSlash.transform.position = transform.position; 
            if(player.transform.localScale.x>0)
            {
                buffoSlash.transform.localScale = new Vector3(3.8f,3.8f,transform.localScale.z);
            }
            else
            {
                buffoSlash.transform.localScale = new Vector3(-3.8f,3.8f,transform.localScale.z);
            }
            Instantiate(buffoSlash);
        }
        if(buffos[4])
        {
            player.GetComponent<BarryController>().heal((int)(player.GetComponent<BarryController>().getMaxHealth()*0.02f));
        }
    }
    internal void jumped()
    {
       if(buffos[10])
        {
            player.GetComponent<BarryController>().microDamage(4);
        } 
    }
}
