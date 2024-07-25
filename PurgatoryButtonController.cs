using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PurgatoryButtonController : MonoBehaviour
{
    public int optionToUpgrade;
    private GameObject player;
    private GameObject tab;
    public GameObject currentTearsText;
    public GameObject costText;
    public GameObject healthText;
    public GameObject powerText;
    public GameObject defenseText;
    public GameObject resistanceText;
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
            costText.GetComponent<TextMeshProUGUI>().text = player.GetComponent<BarryController>().getCostPerUpgrade() + "";
            healthText.GetComponent<TextMeshProUGUI>().text = player.GetComponent<BarryController>().getMaxHealth() + "";
            powerText.GetComponent<TextMeshProUGUI>().text = player.GetComponent<BarryController>().getPowerAttack() + "";
            defenseText.GetComponent<TextMeshProUGUI>().text = player.GetComponent<BarryController>().getDefense() + "";          
            resistanceText.GetComponent<TextMeshProUGUI>().text = player.GetComponent<BarryController>().getResistance() + "";
        }
    }
    public void upgrade()
    {
        
        switch(optionToUpgrade)
        {
            case 1:
            if(player.GetComponent<BarryController>().getCurrentTears() >= player.GetComponent<BarryController>().getCostPerUpgrade())
            {
                player.GetComponent<BarryController>().setCostPerUpgrade((int)(player.GetComponent<BarryController>().getCostPerUpgrade() * 1.12f));
                player.GetComponent<BarryController>().setCurrentTears(player.GetComponent<BarryController>().getCurrentTears() - player.GetComponent<BarryController>().getCostPerUpgrade());
                player.GetComponent<BarryController>().setHealth((int)(player.GetComponent<BarryController>().getMaxHealth() * 1.12f));
            }
            else
                {
                    textHelper.GetComponent<TextMeshProUGUI>().text = "NECESITAS MÁS LÁGRIMAS";
                    textHelper.SetActive(true);
                }
                            
            break;
            case 2:
             if(player.GetComponent<BarryController>().getCurrentTears() >= player.GetComponent<BarryController>().getCostPerUpgrade())
            {
                player.GetComponent<BarryController>().setCurrentTears(player.GetComponent<BarryController>().getCurrentTears() - player.GetComponent<BarryController>().getCostPerUpgrade());
                player.GetComponent<BarryController>().setCostPerUpgrade((int)(player.GetComponent<BarryController>().getCostPerUpgrade() * 1.12f));
                player.GetComponent<BarryController>().setPowerAttack((int)(player.GetComponent<BarryController>().getPowerAttack() * 1.12f));
                }
                else
                {
                    textHelper.GetComponent<TextMeshPro>().text = "NECESITAS MÁS LÁGRIMAS";
                    textHelper.SetActive(true);
                }
            break;
            case 3:
             if(player.GetComponent<BarryController>().getCurrentTears() >= player.GetComponent<BarryController>().getCostPerUpgrade())
            {
                player.GetComponent<BarryController>().setCurrentTears(player.GetComponent<BarryController>().getCurrentTears() - player.GetComponent<BarryController>().getCostPerUpgrade());
                player.GetComponent<BarryController>().setCostPerUpgrade((int)(player.GetComponent<BarryController>().getCostPerUpgrade() * 1.12f));
                player.GetComponent<BarryController>().setDefense((int)(player.GetComponent<BarryController>().getDefense() * 1.12f));
            }
            else
                {
                    textHelper.GetComponent<TextMeshPro>().text = "NECESITAS MÁS LÁGRIMAS";
                    textHelper.SetActive(true);
                }
            break;
            case 4:
             if(player.GetComponent<BarryController>().getCurrentTears() >= player.GetComponent<BarryController>().getCostPerUpgrade())
            {
                player.GetComponent<BarryController>().setCurrentTears(player.GetComponent<BarryController>().getCurrentTears() - player.GetComponent<BarryController>().getCostPerUpgrade());
                player.GetComponent<BarryController>().setCostPerUpgrade((int)(player.GetComponent<BarryController>().getCostPerUpgrade() * 1.12f));
                player.GetComponent<BarryController>().setResistance((int)(player.GetComponent<BarryController>().getPowerAttack() * 1.12f));}
                else
                {
                    textHelper.GetComponent<TextMeshPro>().text = "NECESITAS MÁS LÁGRIMAS";
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
