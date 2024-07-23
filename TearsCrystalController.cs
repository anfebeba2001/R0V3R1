using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TearsCrystalController : MonoBehaviour
{
    public GameObject tears;
    public int amount;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            tears.GetComponent<TearsController>().currentValue = 0;
       tears.GetComponent<TearsController>().finalValue = amount;
       Instantiate(tears, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
       Destroy(gameObject);
       string dataPath = Application.persistentDataPath + "/droppenTears.save";
        File.Delete(dataPath);
        }
       
    }
}
