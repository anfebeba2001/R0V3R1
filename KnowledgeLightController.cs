using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KnowledgeLightController : MonoBehaviour
{
    public string knowledge;
    public GameObject text;
    void Start()
    {
        text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            text.SetActive(true);
            text.GetComponent<TextMeshProUGUI>().text = knowledge;
            text.GetComponent<Animator>().Play("Empty"); 
        }
        
    }
    void OnTriggerExit2D(Collider2D col)
    {
        text.GetComponent<Animator>().Play("DialogBeginning"); 
    }
    void endedAnimation()
    {
        text.SetActive(false);
    }
}