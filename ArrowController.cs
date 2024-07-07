using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private float damage;
    void Start()
    {
        damage = GetComponentInParent<BarryController>().getDamageOnBow();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.transform.localScale.x > 0)
        {
            transform.position = new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z);
            transform.localScale = new Vector3(1,transform.localScale.y,transform.localScale.z);
        }
        else{
            transform.localScale = new Vector3(-1,transform.localScale.y,transform.localScale.z);
            transform.position = new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z);
        }
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            coll.gameObject.SendMessage("Hitted", damage);
        }
        Destroy(this.gameObject);
    }
}
