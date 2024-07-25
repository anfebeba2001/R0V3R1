using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaisingSunController : MonoBehaviour
{
    private float attackCoolDown;
    private float damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if( attackCoolDown > 0)
       {
            attackCoolDown -= Time.deltaTime;             
       }
    }
    void OnTriggerStay2D(Collider2D coll)
    {
        if ((coll.gameObject.tag == "Enemy" || coll.gameObject.tag == "Skeleton") && attackCoolDown <= 0)
        {
            coll.gameObject.GetComponent<EnemyController>().Hitted(damage);
            attackCoolDown = 0.2f;
        }
    }
}
