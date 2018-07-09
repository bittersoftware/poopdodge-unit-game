using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenuHolder : MonoBehaviour {

    [SerializeField] private GameObject pausePanel;
    public AudioMixer audioMixer;
    public Text HighestLevel;
    public Text HighestAcc;
    public Text CurrentLevel;
    public Text CurrentAcc;



    public void PauseGame()
    {
        Invoke("RefreshScreen", 0.5f);
        pausePanel.SetActive(true);
        //Disable scripts that still work while timescale is set to 0
    }
    public void ContinueGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        //enable the scripts again
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetVolume (float volume)
    {
        //audioMixer.SetFloat("volume", volume);
        AudioListener.volume = volume;
    }

    public void Reset()
    {
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.DeleteKey("HighAcc");
        Time.timeScale = 0.1f;
        RefreshScreen();

    }

    public void RefreshScreen()
    {
        Debug.Log("PrintOnEnable: MENU was enabled!!");
        HighestLevel.text = "Level " + FindObjectOfType<GameManager>().GetHighestLevel();
        HighestAcc.text = "Accuracy: " + FindObjectOfType<GameManager>().GetHighestAcc().ToString("F2") + "%";
        CurrentLevel.text = "Level " + FindObjectOfType<GameManager>().GetCurrentLevel();
        CurrentAcc.text = "Accuracy: " + FindObjectOfType<GameManager>().GetCurrentAcc().ToString("F2") + "%";
        Time.timeScale = 0;
    }
}
