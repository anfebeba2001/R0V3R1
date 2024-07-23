using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeteoraController : MonoBehaviour
{
    private float damage = 10;
    private float microDamageTimer;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
      if (microDamageTimer > 0)
        {
            microDamageTimer -= Time.deltaTime;
        }
         
    }
    

    void OnTriggerStay2D(Collider2D coll)
    {
        
         if (coll.gameObject.tag == "Player" && microDamageTimer <= 0)
        {
            microDamageTimer = 0.4f;
            coll.gameObject.SendMessage("microDamage", damage);
        }
    }
    void destroy()
    {
        Destroy(gameObject);
    }
}
