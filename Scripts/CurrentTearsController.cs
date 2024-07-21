using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentTearsController : MonoBehaviour
{
  private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int currentTears = player.GetComponent<BarryController>().getCurrentTears();
        GetComponent<TextMeshProUGUI>().SetText(currentTears.ToString());
    }
}
