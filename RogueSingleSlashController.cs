using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueSingleSlashController : MonoBehaviour
{
    public Color luckyParryColor;
    public GameObject rogue;
    public bool isLucky;
    private Vector3 originalPos;
    private float lifeTime;
    private bool returned;
    public float velocity;

    void Start()
    {
        originalPos = transform.position;
        returned = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isLucky)
        {
            GetComponent<SpriteRenderer>().color = luckyParryColor;
        }
        transform.position += new Vector3(0.08f,0,0)*(transform.localScale.x/5)*velocity;
        lifeTime -= Time.deltaTime;
        if(transform.position.x >= originalPos.x+12 || transform.position.x <= originalPos.x-12)
        {
            if(!returned)
            {
                transform.position += new Vector3(0,1,0)*2f;
                GetComponent<SpriteRenderer>().flipY = !GetComponent<SpriteRenderer>().flipY;
                transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
            }
            else
            {
                transform.position += new Vector3(0,1,0)*2f;
                GetComponent<SpriteRenderer>().flipY = !GetComponent<SpriteRenderer>().flipY;
                transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
            }
            
        }
        if(transform.position.y >= originalPos.y+8 || transform.position.y <= originalPos.y-2)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "BarryCurrentSword")
        {
            if(isLucky && !returned)
            {
                transform.localScale = new Vector3(-transform.localScale.x,-transform.localScale.y,1);
                returned = true;
                GetComponent<SpriteRenderer>().flipY = !GetComponent<SpriteRenderer>().flipY;
            }
        }


        if(col.gameObject.tag == "Player")
        {
            col.SendMessage("BarryGotAttacked",20);
            Destroy(gameObject);
        }
        if(col.gameObject == rogue)
        {
            if(isLucky && returned)
            {
                rogue.GetComponent<RogueController>().gotParried();
                Destroy(gameObject);
            }
        }
    }
}
