using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Animator transitionAnim;
    public GameObject transitionPanel;
    public GameObject highScore;

    public void StartGame()
    {
        transitionPanel.SetActive(true);
        transitionAnim.SetTrigger("anim");

        Invoke("LoadGame", 0.5f);
    }

    public void HighScore()
    {
        highScore.SetActive(true);
    }

    public void HighScoreBack()
    {
        highScore.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void LoadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
