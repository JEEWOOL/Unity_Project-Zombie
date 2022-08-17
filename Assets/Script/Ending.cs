using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Application.Quit();
        }
    }
}
