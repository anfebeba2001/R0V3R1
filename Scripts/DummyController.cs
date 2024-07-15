using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DummyController : MonoBehaviour
{
    private GameObject blood;
    private GameObject damageMessagePopUp;


    // Start is called before the first frame update
    void Start()
    {
        blood = GetComponent<EnemyController>().getBlood();
        damageMessagePopUp = GetComponent<EnemyController>().getDamageMessagePopUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hitted(float damage)
    {
        Debug.Log(damage);

        damageMessagePopUp.GetComponent<TextMeshPro>().text = (damage) + " ";

        Instantiate(damageMessagePopUp, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        Instantiate(blood, transform.position + new Vector3(0, 0f, 0), Quaternion.identity);
        GetComponent<Rigidbody2D>().AddForce(Vector2.right, ForceMode2D.Impulse);
        GetComponent<Rigidbody2D>().AddForce(Vector2.left, ForceMode2D.Impulse);
    }
}
