using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueSlashController : MonoBehaviour
{
    public float damage;
    private float microDamageTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (microDamageTimer > 0)
        {
            microDamageTimer -= Time.deltaTime;
        }
    }
    void Destroy()
    {
        Destroy(gameObject);
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            if(microDamageTimer <= 0)
            {
                microDamageTimer = 0.2f;
                col.gameObject.SendMessage("microDamage", damage);
            }
        }
    }
}