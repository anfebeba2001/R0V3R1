using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageMessagePopUpController : MonoBehaviour
{
    // Start is called before the first frame update
    private float showingTimer;
    private TextMeshPro textMeshPro;

    void Start()
    {

        showingTimer = 1f;
        textMeshPro = gameObject.GetComponent<TextMeshPro>();

    }

    // Update is called once per frame
    Vector3 positionFixed;
    void Update()
    {
        //OrientationCorrection
        if (transform.parent.gameObject.transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(
                    -0.09256908f,
                    transform.localScale.y,
                    transform.localScale.z
                );
            
        }
        else
        {
            transform.localScale = new Vector3(
                    0.09256908f,
                    transform.localScale.y,
                    transform.localScale.z
                );
         

        }




        if (showingTimer > 0)
        {
            showingTimer -= Time.deltaTime;
            positionFixed = transform.position;
            positionFixed.y += 0.005f;
            transform.position = positionFixed;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void activate()
    {

    }
}
