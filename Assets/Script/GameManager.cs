using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    private void Awake()
    {
        if(gm == null)
        {
            gm = this;
        }
    }

    public enum GameState
    {
        Ready,
        Go,
        Pause,
        GameOver,
        Complete,
    }

    public GameObject gameOption;
    public GameState gmState;
    public GameObject gameLabel;
    //public bool isOption = false;
    Text gameText;
    PlayerMove player;

    public bool esc = false;

    private void Start()
    {
        gmState = GameState.Ready;
        gameText = gameLabel.GetComponent<Text>();
        gameText.text = "Are you Ready?";
        gameText.color = new Color(255, 0, 0, 255);
        StartCoroutine(ReadyToStart());

        player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    IEnumerator ReadyToStart()
    {
        yield return new WaitForSeconds(2f);
        gameText.text = "START!";

        yield return new WaitForSeconds(0.5f);
        gameLabel.SetActive(false);

        gmState = GameState.Go;
    }

    private void Update()
    {
        if (player.HP <= 0)
        {
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0f);
            gameLabel.SetActive(true);
            gameText.text = "Game OVER";
            gameText.color = new Color(255, 0, 0, 255);
            Time.timeScale = 0f;
            gmState = GameState.GameOver;
        }

        GameObject smObject = GameObject.Find("ScoreManager");
        ScoreManager sm = smObject.GetComponent<ScoreManager>();
        if (sm.currentScore >= sm.bestScore)
        {
            gameLabel.SetActive(true);
            gameText.text = "Mission Complete";
            gameText.color = new Color(0, 0, 255, 255);
            //Time.timeScale = 0f;
            gmState = GameState.Complete;

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (SceneManager.GetActiveScene().buildIndex == 4)
                {
                    SceneManager.LoadScene(5);
                }
                else
                {
                    SceneManager.LoadScene(6);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenOptionWindow();
            esc = true;
        }
        if (esc == true)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CloseOptionWindow();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                RestartGame();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                QuitGame();
            }
        }       
    }

    public void OpenOptionWindow()
    {
        Debug.Log("´­¸²");
        //isOption = !isOption;
        gameOption.SetActive(true);
        Time.timeScale = 0f;
        gmState = GameState.Pause;        
    }
    
    public void CloseOptionWindow()
    {        
        gameOption.SetActive(false);
        Time.timeScale = 1f;
        gmState = GameState.Go;
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //SceneManager.LoadScene(4);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
