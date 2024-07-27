using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffoSlashController : MonoBehaviour
{
    private int damage = 5;
    private float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        lifeTime = 0.6f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x + (0.1f * transform.localScale.x/5),transform.position.y,transform.position.z);
        if(lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if ((coll.gameObject.tag == "Enemy" || coll.gameObject.tag == "Skeleton") )
        {
            coll.gameObject.GetComponent<EnemyController>().Hitted(damage);
        }
    }
}
