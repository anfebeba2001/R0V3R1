using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject healthBar;
    private GameObject player;
    private GameObject staminaBar;
    public GameObject boss;
    public GameObject healthBarBoss;
    void Start()
    {
        healthBar = GameObject.FindGameObjectWithTag("HealthBar");
        player = GameObject.FindGameObjectWithTag("Player");
        staminaBar =  GameObject.FindGameObjectWithTag("StaminaBar");
    }

    // Update is called once per frame
    void Update()
    {
        healthBarBoss.GetComponent<Slider>().maxValue = boss.GetComponent<BossController>().getMaxHealth();
        healthBarBoss.GetComponent<Slider>().value = boss.GetComponent<BossController>().getHealth();
        healthBar.GetComponent<Slider>().maxValue = player.GetComponent<BarryController>().getMaxHealth();
        healthBar.GetComponent<Slider>().value = player.GetComponent<BarryController>().getHealth();
        staminaBar.GetComponent<Slider>().value = player.GetComponent<BarryController>().getStamina();
        staminaBar.GetComponent<Slider>().maxValue = player.GetComponent<BarryController>().getMaxStamina();
    }
}
