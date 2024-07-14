using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class SkeletonWaveController : MonoBehaviour
{
    public Color originalColor;
    public Color reversedColor;
    private bool reversed;
    private Vector3 fixedPos;

    private float damage = 20;
    private float originalScale;
    private Vector3 fixedScale;
    private bool ended;

    void Start()
    {
        reversed = false;
        ended = false;
        GetComponent<SpriteRenderer>().color = originalColor;
        originalScale =  transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(!ended)
        {

       
        if(originalScale > 0)
        {
if(!reversed)
        {
            fixedPos = transform.position;
            fixedPos.x += 0.1f;
            transform.position = fixedPos;
        }
        else
        {
            fixedPos = transform.position;
            fixedPos.x -= 0.1f;
            transform.position = fixedPos;
        }
        }
        else
        {
            if(!reversed)
        {
            fixedPos = transform.position;
            fixedPos.x -= 0.1f;
            transform.position = fixedPos;
        }
        else
        {
            fixedPos = transform.position;
            fixedPos.x += 0.1f;
            transform.position = fixedPos;
        }
        }
         }
         else{
            GetComponent<Animator>().Play("SkeletonWaveEnd");
         }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        
        if (coll.gameObject.tag == "Player" && !reversed)
        {

                ended = true;
                coll.gameObject.SendMessage("BarryGotAttacked",damage);
           
            
                   
        }
        if(coll.gameObject.tag == "BarryCurrentSword")
        {
           
                reversed =true;
                GetComponent<SpriteRenderer>().color = reversedColor;
                fixedScale = transform.localScale;
                fixedScale.x *= -1;
                transform.localScale = fixedScale;
               
        }
        if(reversed)
        if (coll.gameObject.tag == "Skeleton" && reversed)
        {
            ended = true;
            coll.gameObject.SendMessage("Death");
        }
    }
    void Destroy()
    {
        Destroy(gameObject);
    }
}
