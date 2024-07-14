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
        if (coll.gameObject.tag == "Enemy" && parent.GetComponent<BarryController>().getAttacking() && attackCoolDown <= 0)
        {
            coll.gameObject.SendMessage("Attacked", parent.GetComponent<BarryController>().getPowerAttack());
            mainCamera.GetComponent<Camera>().orthographicSize -= 0.1f;
            attackCoolDown = 0.7f;
        }

    }
}
