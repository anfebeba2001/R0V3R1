using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BarrySwordAttackController : MonoBehaviour
{
    private GameObject parent;
    private GameObject mainCamera;
    private float attackCoolDown;
    private GameObject buffsHelper;

    // Start is called before the first frame update
    void Start()
    {
        buffsHelper = GameObject.FindGameObjectWithTag("BuffsHelper");
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
            damage = (int)(parent.GetComponent<BarryController>().getPowerAttack() * 1.5f);
        }
        
        damage += parent.GetComponent<BarryController>().getPowerBoofModifier();
        
        if ((coll.gameObject.tag == "Enemy" || coll.gameObject.tag == "Skeleton") && attackCoolDown <= 0)
        {
            coll.gameObject.GetComponent<EnemyController>().Hitted(damage);
            mainCamera.GetComponent<Camera>().orthographicSize -= 0.1f;
            attackCoolDown = 0f;
            GetComponent<AudioSource>().Play();


            buffsHelper.GetComponent<BuffsOnBarryHelper>().AttackingAnEnemy();
        }

    }
}
