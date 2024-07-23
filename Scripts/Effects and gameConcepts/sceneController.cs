using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneController : MonoBehaviour
{
    public GameObject tearsCrystal;
    void Start()
    {
       DroppenTearsData droppenTearsData =   SaveManager.loadDroppenTearsData();
       if(droppenTearsData != null)
       {
            tearsCrystal.GetComponent<TearsCrystalController>().amount = droppenTearsData.amount;
            tearsCrystal.transform.position = new Vector3(droppenTearsData.position[0],droppenTearsData.position[1],droppenTearsData.position[2]);
            Instantiate(tearsCrystal);
            
       }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Respawn()
    {
        SceneManager.LoadScene("FirstScene");
    }
}
