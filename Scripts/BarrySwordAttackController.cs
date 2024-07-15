using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BarrySwordAttackController : MonoBehaviour
{
    private GameObject parent;
    private GameObject mainCamera;
    private float attackCoolDown;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        parent = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(attackCoolDown > 0)
        {
            attackCoolDown -= Time.deltaTime;
        }
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        
        int damage = 0;
        if(parent.GetComponent<BarryController>().getAttackState() == 1){
            damage = (int)(parent.GetComponent<BarryController>().getPowerAttack() * 0.7f);
        }
        else if(parent.GetComponent<BarryController>().getAttackState() == 2){
            damage = (int)parent.GetComponent<BarryController>().getPowerAttack();
        }
        else if(parent.GetComponent<BarryController>().getAttackState() == 3){
            damage = 1000;
        }
        
        
        if ((coll.gameObject.tag == "Enemy" || coll.gameObject.tag == "Skeleton") && attackCoolDown <= 0)
        {
            Debug.Log("mamahuevo: " + parent.GetComponent<BarryController>().getAttackState());
            coll.gameObject.GetComponent<EnemyController>().Hitted(damage);
            //coll.gameObject.SendMessage("Attacked", damage);
            mainCamera.GetComponent<Camera>().orthographicSize -= 0.1f;
            attackCoolDown = 0f;
        }

    }
}
