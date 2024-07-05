using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject healthBar;
    private GameObject player;
    void Start()
    {
        healthBar = GameObject.FindGameObjectWithTag("HealthBar");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.GetComponent<Slider>().maxValue = player.GetComponent<BarryController>().getMaxHealth();
        healthBar.GetComponent<Slider>().value = player.GetComponent<BarryController>().getHealth();
    }
}
