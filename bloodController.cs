using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bloodController : MonoBehaviour
{
    public GameObject parent;
    void Start()
    {
        GetComponent<Animator>().Play(Random.Range(1, 9) + "");
    }

    // Update is called once per frame
    void Update()
    {
        if (parent != null)
        {
            transform.position = parent.transform.position;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    void exit()
    {
        Destroy(gameObject);

    }
    public void setParent(GameObject parent)
    {
        this.parent = parent;
    }
}
