using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberOfArrowsController : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        int arrows = player.GetComponent<BarryController>().getNumberOfArrows();
        GetComponent<TextMeshProUGUI>().SetText(arrows.ToString());
    }
}
