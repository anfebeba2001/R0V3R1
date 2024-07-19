using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueSingleSlashController : MonoBehaviour
{
    public Color luckyParryColor;
    public bool isLucky;
    private float lifeTime;
    private bool returned;
    void Start()
    {
        lifeTime = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if(isLucky)
        {
            GetComponent<SpriteRenderer>().color = luckyParryColor;
        }
        transform.position += new Vector3(0.2f,0,0)*(transform.localScale.x/10);
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.tag == "BarryCurrentSword")
        {
            if(isLucky)
            {
                transform.localScale = new Vector3(-transform.localScale.x,-transform.localScale.y,1);
            }
        }
    }
}
