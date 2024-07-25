using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopButtonController : MonoBehaviour
{
    public int optionToUpgrade;
    private GameObject player;
    private GameObject tab;
    public GameObject currentTearsText;
    public GameObject healingsText;
    public GameObject healingsCost;
    public GameObject arrowText;
    public GameObject arrowsCost;
    public GameObject generalMenu;
    public GameObject textHelper;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        tab = GameObject.FindGameObjectWithTag("Corner");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(optionToUpgrade == 5)
        {
            currentTearsText.GetComponent<TextMeshProUGUI>().text = player.GetComponent<BarryController>().getCurrentTears()+"";
            healingsCost.GetComponent<TextMeshProUGUI>().text = (int)(player.GetComponent<BarryController>().getCostPerUpgrade()/10) + "";
            arrowsCost.GetComponent<TextMeshProUGUI>().text = (int)(player.GetComponent<BarryController>().getCostPerUpgrade()/12) + "";

            arrowText.GetComponent<TextMeshProUGUI>().text = player.GetComponent<BarryController>().getNumberOfArrows() + "";
            healingsText.GetComponent<TextMeshProUGUI>().text = player.GetComponent<BarryController>().getHealingVials() + "";

        }
    }
    public void upgrade()
    {
        
        switch(optionToUpgrade)
        {
            case 1:
            if(player.GetComponent<BarryController>().getCurrentTears() >= player.GetComponent<BarryController>().getCostPerUpgrade() / 10 && player.GetComponent<BarryController>().getHealingVials() <= 17)
            {
                player.GetComponent<BarryController>().setCurrentTears(player.GetComponent<BarryController>().getCurrentTears() - (player.GetComponent<BarryController>().getCostPerUpgrade()/12));
                player.GetComponent<BarryController>().setHealingVials((int)(player.GetComponent<BarryController>().getHealingVials() + 3 ));
            }
            else if(player.GetComponent<BarryController>().getCurrentTears() < player.GetComponent<BarryController>().getCostPerUpgrade() / 10)
                {
                    textHelper.GetComponent<TextMeshProUGUI>().text = "NECESITAS MÁS LÁGRIMAS";
                    textHelper.SetActive(true);
                }
             else if(player.GetComponent<BarryController>().getHealingVials() > 17)
             {
                textHelper.GetComponent<TextMeshProUGUI>().text = "Limite alcanzado";
                textHelper.SetActive(true);
             }               
            break;
            case 2:

            if(player.GetComponent<BarryController>().getCurrentTears() >= player.GetComponent<BarryController>().getCostPerUpgrade() / 12 && player.GetComponent<BarryController>().getNumberOfArrows() <= 15)
            {
                player.GetComponent<BarryController>().setCurrentTears(player.GetComponent<BarryController>().getCurrentTears() - (player.GetComponent<BarryController>().getCostPerUpgrade()/12));
                player.GetComponent<BarryController>().setNumberOfArrows((int)(player.GetComponent<BarryController>().getNumberOfArrows() + 5 ));
            }
            else if(player.GetComponent<BarryController>().getCurrentTears() < player.GetComponent<BarryController>().getCostPerUpgrade() / 12)
                {
                    textHelper.GetComponent<TextMeshProUGUI>().text = "NECESITAS MÁS LÁGRIMAS";
                    textHelper.SetActive(true);
                }
             else if(player.GetComponent<BarryController>().getNumberOfArrows() > 15)
             {
                textHelper.GetComponent<TextMeshProUGUI>().text = "Limite alcanzado";
                textHelper.SetActive(true);
             }     
            break;
           
            case 5:
                tab.SetActive(false);
                generalMenu.SetActive(true);
            break;
        
       
            
        
    
    
        }
         SaveManager.savePlayerData(    
            player.GetComponent<BarryController>().getDefense(),
            player.GetComponent<BarryController>().getMaxHealth(),
            player.GetComponent<BarryController>().getPowerAttack(),
            player.GetComponent<BarryController>().getResistance(),
            player.GetComponent<BarryController>().getCurrentTears(),
            player.GetComponent<BarryController>().getHealingVials(),
            player.GetComponent<BarryController>().getNumberOfArrows(),
            player.GetComponent<BarryController>().getCostPerUpgrade()
         );
    }
}
