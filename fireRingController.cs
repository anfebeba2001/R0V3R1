using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireRingController : MonoBehaviour
{
    private float damage = 50;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.name == "Barry")
        {
            coll.gameObject.SendMessage("BarryGotAttacked",damage);
        }
    }
    void destroy()
    {
        Destroy(gameObject);
    }
}
