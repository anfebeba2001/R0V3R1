using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BarrySwordAttackController : MonoBehaviour
{
    private GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy" && parent.GetComponent<BarryController>().getAttacking())
        {
            coll.gameObject.SendMessage("Attacked", parent.GetComponent<BarryController>().getPowerAttack());
        }

    }
}
