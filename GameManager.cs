using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{

    public GameObject levelCompleteScreen;
    public GameObject gameOverScreen;
    public GameObject player;
    public Text timerText;
    public Text LevelNumberScreen;
    public Text EnemiesNumberScreen;
    public Text BulletNumberScreen;
    public AudioClip audioClip;
    public AudioSource audioSource;

    public int enemiesDead = 0;
    public int bulletsDestroyed = 0;



    [SerializeField]
    private int currentLevel;
    [SerializeField]
    private float currentAcc;
    [SerializeField]
    private int totalShots;
    [SerializeField]
    private int totalEnemiesKilled;
    private int sceneIndex = 0;
    private static bool created = false;
    [SerializeField]
    private int mTimer = 60;
    private int numberOfEnemies;
    private int numbOfLevelShots;
    //private int LastLevelIndex = 1;

    // Use this for initialization
    void Start () {
        //For HighScore 
        currentLevel = sceneIndex + 1;
        currentAcc = 0f;
        totalShots = 0;

        audioSource.clip = audioClip;

        FindObjectOfType<SpawnManager>().spawnedEnemies = 0;
        FindObjectOfType<SpawnManager>().isSpawned = false;
        FindObjectOfType<SpawnManager>().numberOfEnemies = 1;
        numbOfLevelShots = FindObjectOfType<SpawnManager>().getLevelShots(sceneIndex);
        GameObject.Find("RocketIcon").GetComponentInChildren<Text>().text = "x " + numbOfLevelShots;
        numberOfEnemies = 1;

        Time.timeScale = 1;

        InvokeRepeating("TimerCountdown", 0.5f, 1f);

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (bulletsDestroyed == FindObjectOfType<SpawnManager>().GetNumberOfShots() && enemiesDead < numberOfEnemies)
        {
            //Debug.Log("bullet Destroyed: " + bulletsDestroyed + "number of shots: " + FindObjectOfType<SpawnManager>().numberOfShots + "Enemies Dead: " + enemiesDead + "number of Enemies: " + numberOfEnemies);
            GameOver();
        }
        //updtade Accuracy
        if (totalShots > 0)
        {
            currentAcc = ((float)totalEnemiesKilled / (float)totalShots) * 100;
        }
    }

    public void CheckEndOfLevel()
    {
        enemiesDead++;
        //forHighScore
        totalEnemiesKilled++;
        checkHightScore();

        if (enemiesDead == FindObjectOfType<SpawnManager>().numberOfEnemies)
        {
            //Level Complete Screen
            sceneIndex++;
            currentLevel++;


            LevelNumberScreen.text = sceneIndex.ToString();
            EnemiesNumberScreen.text = FindObjectOfType<SpawnManager>().getLevelEnemies(sceneIndex).ToString();
            BulletNumberScreen.text = FindObjectOfType<SpawnManager>().getLevelShots(sceneIndex).ToString();

            levelCompleteScreen.SetActive(true);
            DestroyAllObjects();
            player.SetActive(false);

            CancelInvoke("TimerCountdown"); 

        }
    }

    public void NextLevel()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        enemiesDead = 0;
        bulletsDestroyed = 0;
        player.transform.position = new Vector2 (-0.2f, -4f);
        FindObjectOfType<BulletEmissor>().angle = 0f;
        numbOfLevelShots = FindObjectOfType<SpawnManager>().getLevelShots(sceneIndex);
        GameObject.Find("RocketIcon").GetComponentInChildren<Text>().text = "x " + numbOfLevelShots;

        player.SetActive(true);


        levelCompleteScreen.SetActive(false);
        FindObjectOfType<SpawnManager>().spawnedEnemies = 0;
        FindObjectOfType<SpawnManager>().isSpawned = false;

        FindObjectOfType<SpawnManager>().CheckScene(sceneIndex);

        mTimer = 60;
        InvokeRepeating("TimerCountdown", 0.5f, 1f);

        numberOfEnemies = FindObjectOfType<SpawnManager>().numberOfEnemies;
    }



    public void EndGame()
    {
        //Game is Over
        //Restart();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GameOver()
    {
        checkHightScore();

        audioSource.Play();

        bulletsDestroyed = 0;
        enemiesDead = 0;
        sceneIndex++;

        DestroyAllObjects();

        CancelInvoke("TimerCountdown");

        player.SetActive(false);
        gameOverScreen.SetActive(true);
        Debug.Log("Game Over");
        //birds flying bg
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void DestroyAllObjects()
    {
        var bulletClones = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (var clone in bulletClones)
        {
            Destroy(clone);
        }

        var enemyClones = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var clone in enemyClones)
        {
            Destroy(clone);
        }

        var poopClones = GameObject.FindGameObjectsWithTag("Poop");
        foreach (var clone in poopClones)
        {
            Destroy(clone);
        }
    }

    private void TimerCountdown()
    {
        if (mTimer > 0)
        {
            mTimer--;
        }
        else
        {
            FindObjectOfType<Player>().timeoutAnim();
            audioSource.Play();
        }

        timerText.text = mTimer.ToString();
    }

    public void IncrementTotalShots()
    {
        totalShots++;
    }

    private void checkHightScore()
    {
        if (currentLevel == PlayerPrefs.GetInt("HighScore", 0) && currentAcc > GetHighestAcc())
        {
            PlayerPrefs.SetFloat("HighAcc", currentAcc);
        }
        else if (currentLevel > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", currentLevel);
            PlayerPrefs.SetFloat("HighAcc", currentAcc);
        }
    }

    public int GetHighestLevel()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }

    public float GetHighestAcc()
    {
        return PlayerPrefs.GetFloat("HighAcc", 0);
    }

    public float GetCurrentAcc()
    {
        return currentAcc;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }




}
