using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageMessagePopUpController : MonoBehaviour
{
    // Start is called before the first frame update
    public float showingTimer;
    private TextMeshPro textMeshPro;

    void Start()
    {
        textMeshPro = gameObject.GetComponent<TextMeshPro>();
     
    }

    // Update is called once per frame
    Vector3 positionFixed;
    void Update()
    {
       
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
    public void setShowTime(float showingTimerReceived)
    {
        showingTimer = showingTimerReceived;    
    }

}
