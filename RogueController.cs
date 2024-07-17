using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueController : MonoBehaviour
{
string[][] dialogs =  {
new string[] {"Fuiste demasiado lejos... Ahora pagarás.",
              "Tu osadía ha superado tu sentido común... Muere... ",
              "Ni tus gritos podrán escapar.",
              "DESFALLECE"},
};

// Initialize the elements.

    void Start()
    {
        GetComponent<BossController>().setDialogs(setDialogsLocal());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string[][] setDialogsLocal()
    {
        return dialogs;
    }
    
    

}
