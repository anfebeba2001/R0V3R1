using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
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

    public GameObject buffoIdentifierEffect;

    public GameObject[] buffosScreen = new GameObject[4];
    public bool[] buffos = new bool[16];
    public Sprite[] buffosSprites = new Sprite[16];
    private int buffoCounter;

    public GameObject raisingSun;
    private float quietPlaceCoolDown;
    private Vector3 originalScale;
    private float injusticeCoolDown;
    public GameObject text;
    private string buffoText;

    public void AddBuff()
    {
        int selection = Random.Range(1,160);
        if(buffos[selection%16] ||  selection%16 == 7 || selection%16 == 12 || selection%16 == 13 ||  selection%16 == 14 ||  selection%16 == 15)
        {
            AddBuff();
        }
        else
        {
            buffoIdentifierEffect.GetComponent<SpriteRenderer>().sprite = buffosSprites[selection%16];
            switch(selection%16)
            {
                
                case 0:
                    buffoText = "BENDICIÓN - Sol ascendente: Genera un aura que quema a los enemigos progresivamente.";
                break;
                case 1:
                    buffoText = "BENDICIÓN - Lugar calmado: Te curas un porcentaje de tu vida máxima cada cierto tiempo. ";    
                break;
                case 2:
                    buffoText = "BENDICIÓN - Resonancia: Genera cortes en el aire que aumentan el rango de tus ataques y traspasan enemigos.";    
                break;
                case 3:
                    buffoText = "BENDICIÓN - Berserker: Aumenta un poco tu defensa y tu poder de ataque.";    
                break;
                case 4:
                    buffoText = "BENDICIÓN - Sanguinario: Cuando atacas a un enemigo te curas un porcentaje de tu vida ";    
                break;
                case 5:
                    buffoText = "BENDICIÓN - Mano firme: El daño que infliges AUMENTA en un 30%.";    
                break;
                case 6:
                    buffoText = "BENDICIÓN - Severo: El daño que recibes disminuye en un 30%.";    
                break;
                case 8:
                    buffoText = "MALDICIÓN - Sollozante: El daño que recibes aumenta en un 20%.";    
                break;
                case 9:
                    buffoText = "MALDICIÓN - Debilucho: El daño que infliges REDUCE en 30%.";    
                break;
                case 10: 
                    buffoText = "MALDICIÓN - Pies heridos: Por cada salto o desplazamiento rápido que utilices, pierdes una pequeña cantidad de vida";   
                break;
                case 11: 
                    buffoText = "MALDICIÓN - Injusticia: Cada cierto tiempo pierdes un porcentaje de tu vida máxima";   
                break;
            }   
            text.GetComponent<TextMeshProUGUI>().text = buffoText;

            buffosScreen[buffoCounter].GetComponent<UnityEngine.UI.Image>().color = RevealedColor;
            buffosScreen[buffoCounter].GetComponent<UnityEngine.UI.Image>().sprite = buffosSprites[selection%16];
            buffos[selection%16] = true;
            buffoCounter++;
        }
          if(buffos[3])
        {
            player.GetComponent<BarryController>().setPowerBoofModifier((int)(player.GetComponent<BarryController>().getPowerBoofModifier() + (player.GetComponent<BarryController>().getPowerAttack() * 0.2f )));
            player.GetComponent<BarryController>().setDefenseBoofModifier((int)(player.GetComponent<BarryController>().getDefenseBoofModifier() + player.GetComponent<BarryController>().getDefense()));
        }
        if(buffos[5])
        {
            player.GetComponent<BarryController>().setPowerBoofModifier((int)(player.GetComponent<BarryController>().getPowerBoofModifier() + (player.GetComponent<BarryController>().getPowerAttack() * 0.8f )));
        }
        if(buffos[6])
        {
            player.GetComponent<BarryController>().setDefenseBoofModifier((int)(player.GetComponent<BarryController>().getDefenseBoofModifier() + (player.GetComponent<BarryController>().getDefense()  * 2.5f )));
        }
        if(buffos[8])
        {
            player.GetComponent<BarryController>().setDefenseBoofModifier((int)(player.GetComponent<BarryController>().getDefenseBoofModifier() - (player.GetComponent<BarryController>().getDefense() * 2.5f )));
        }
        if(buffos[9])
        {
            player.GetComponent<BarryController>().setPowerBoofModifier((int)(player.GetComponent<BarryController>().getPowerBoofModifier() - (player.GetComponent<BarryController>().getPowerAttack() * 0.4f )));
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        text = GameObject.FindGameObjectWithTag("DialogHelper");
        text.GetComponent<TextMeshProUGUI>().text = "";
        buffosScreen[0].GetComponent<UnityEngine.UI.Image>().color = unrevealedColor;
        buffosScreen[1].GetComponent<UnityEngine.UI.Image>().color = unrevealedColor;
        buffosScreen[2].GetComponent<UnityEngine.UI.Image>().color = unrevealedColor;
        buffosScreen[3].GetComponent<UnityEngine.UI.Image>().color = unrevealedColor;
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
                quietPlaceCoolDown = 15f;
                player.GetComponent<BarryController>().heal((int)(player.GetComponent<BarryController>().getMaxHealth()*0.04f));
            }
        }
       
        if(buffos[7])
        {

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
