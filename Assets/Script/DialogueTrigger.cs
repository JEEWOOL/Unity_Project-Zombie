using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue info;


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void Trigger()
    {        
        var system = FindObjectOfType<DialogueSystem>();
        system.Begin(info);
        //Cursor.lockState = CursorLockMode.None;
    }

    public void skip()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            SceneManager.LoadScene(4);
        }             
    }
}
