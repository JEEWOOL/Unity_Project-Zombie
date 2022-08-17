using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public Text currentScoreUI;
    public int currentScore;
    public Text bestScoreUI;
    public int bestScore;

    private void Start()
    {
        bestScore = PlayerPrefs.GetInt("Best Score", 20);
        bestScoreUI.text = "ÀûÀÇ ¼ö : " + 20;
    }    
}
