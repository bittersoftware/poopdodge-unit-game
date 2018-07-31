using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Animator transitionAnim;
    public GameObject transitionPanel;
    public GameObject highScore;
    public GameObject aboutPanel;
    public Text versionTxt;

    private void Start()
    {
        versionTxt.text = "Version : " + Application.version;
    }

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
        Time.timeScale = 1f;
        highScore.SetActive(false);
    }

    public void About()
    {
        aboutPanel.SetActive(true);
    }

    public void AbouteBack()
    {
        aboutPanel.SetActive(false);
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
