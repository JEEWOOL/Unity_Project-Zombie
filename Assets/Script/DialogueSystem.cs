using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueSystem : MonoBehaviour
{
    public Text txtName;
    public Text txtSentence;

    Queue<string> sentences = new Queue<string>();

    public Animator anim;

    public void Begin(Dialogue info)
    {
        anim.SetBool("isOpen", true);

        sentences.Clear();

        txtName.text = "<color=#ffffff>" + info.name + "</color>";

        foreach (var sentence in info.sentences)
        {
            sentences.Enqueue(sentence);
        }

        Next();
    }

    public void Next()
    {
        if(sentences.Count == 0)
        {
            End();
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                SceneManager.LoadScene(4);
            }
            else
            {
                SceneManager.LoadScene(6);
            }            
        }

        //txtSentence.text = sentences.Dequeue();
        txtSentence.text = string.Empty;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentences.Dequeue()));
    }

    IEnumerator TypeSentence(string sentence)
    {
        foreach (var letter in sentence)
        {
            txtSentence.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void End()
    {
        anim.SetBool("isOpen", false);
        txtSentence.text = string.Empty;
    }
}
