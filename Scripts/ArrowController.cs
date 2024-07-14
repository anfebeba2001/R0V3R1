using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float damage;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x > 0)
        {
            transform.position = new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z);
        }
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag != "Player")
        {
            if (coll.gameObject.tag == "Enemy")
            {
                coll.gameObject.SendMessage("HittedByBow", damage);
                Destroy(gameObject);
            }
        }
    }
}
