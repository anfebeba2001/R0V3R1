using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeteoraController : MonoBehaviour
{
    private Vector3 fixedPos;
    private bool goingDown;
    private float damage = 45;
    // Start is called before the first frame update
    void Start()
    {
        goingDown = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(goingDown)
        {
            fixedPos = transform.position;
            fixedPos.y -= 0.1f;
            transform.position = fixedPos;  
        }
         
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        goingDown = false;
        GetComponent<CircleCollider2D>().isTrigger = true;
        GetComponent<Animator>().Play("MeteoraExplode");
        if(coll.gameObject.tag == "Player")
        {
            SendMessage("BarryGotHitted",damage);
            if (coll.transform.position.x > transform.position.x)
                coll.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(3, 2), ForceMode2D.Impulse);
            else
                coll.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-3, 2), ForceMode2D.Impulse);
        }
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            SendMessage("BarryGotHitted",damage/2);
            if (coll.transform.position.x > transform.position.x)
                coll.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1.5f, 2), ForceMode2D.Impulse);
            else
                coll.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1.5f, 2), ForceMode2D.Impulse);
        }
    }
    void destroy()
    {
        Destroy(gameObject);
    }
}
