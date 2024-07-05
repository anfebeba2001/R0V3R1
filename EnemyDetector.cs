using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject parent;
    private float normalRadius;
    private float tauntedRadius;
    void Start()
    {
        parent = transform.parent.gameObject;
        parent.SendMessage("setRadius", this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            parent.GetComponent<EnemyController>().setTaunted(true);
            parent.GetComponent<EnemyController>().setPLayerDetected(coll.gameObject);
            GetComponent<CircleCollider2D>().radius = tauntedRadius;

        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            parent.GetComponent<EnemyController>().setTaunted(false);
            parent.GetComponent<EnemyController>().setPLayerDetected(null);
            GetComponent<CircleCollider2D>().radius = normalRadius;
        }
    }
    public void setNormalRadius(float normalRadius)
    {
        this.normalRadius = normalRadius;
    }
    public void setTauntedRadius(float tauntedRadius)
    {
        this.tauntedRadius = tauntedRadius;
    }
}
