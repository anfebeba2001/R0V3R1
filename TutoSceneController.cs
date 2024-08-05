using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoSceneController : MonoBehaviour
{
    public GameObject[] buttons;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject button in buttons)
        {
            button.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void firstFlagOnTouch()
    {
        buttons[1].SetActive(true);
    }
}
