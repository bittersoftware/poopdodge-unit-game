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
    private bool isTimeoutFXPlayed;
    private bool rewardTime;
    private bool rewardBullet;

    // Use this for initialization
    void Start () {
        //For HighScore 
        currentLevel = sceneIndex + 1;
        currentAcc = 0f;
        totalShots = 0;
        isTimeoutFXPlayed = false;

        //For rewards
        rewardTime = false;
        rewardBullet = false;

    FindObjectOfType<AudioManager>().Play("Theme");


        FindObjectOfType<SpawnManager>().spawnedEnemies = 0;
        FindObjectOfType<SpawnManager>().isSpawned = false;
        FindObjectOfType<SpawnManager>().numberOfEnemies = 1;
        numbOfLevelShots = FindObjectOfType<SpawnManager>().getLevelShots(sceneIndex);
        GameObject.Find("RocketIcon").GetComponentInChildren<Text>().text = "x " + numbOfLevelShots;
        numberOfEnemies = 1;

        Time.timeScale = 1;

        InvokeRepeating("TimerCountdown", 1f, 1f);

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
            //AudioFX - Timeout Management
            FindObjectOfType<AudioManager>().Stop("Timeout");
            isTimeoutFXPlayed = false;

            //reset Reward
            //For rewards
            rewardTime = false;
            rewardBullet = false;

            //Level Complete Screen
            sceneIndex++;
            currentLevel++;


            LevelNumberScreen.text = sceneIndex.ToString();
            EnemiesNumberScreen.text = "x" + FindObjectOfType<SpawnManager>().getLevelEnemies(sceneIndex).ToString();
            BulletNumberScreen.text = "x" + FindObjectOfType<SpawnManager>().getLevelShots(sceneIndex).ToString();
            BulletNumberScreen.color = new Color(0f / 255.0f, 0f / 255.0f, 0f / 255.0f, 255.0f / 255.0f);

            levelCompleteScreen.SetActive(true);

            //Re-enalbe reward buttons
            FindObjectOfType<NextLevel>().EnableRewardButtons();

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
        mTimer = 60;
        timerText.text = mTimer.ToString();


        levelCompleteScreen.SetActive(false);
        FindObjectOfType<SpawnManager>().spawnedEnemies = 0;
        FindObjectOfType<SpawnManager>().isSpawned = false;

        FindObjectOfType<SpawnManager>().CheckScene(sceneIndex);

        
        InvokeRepeating("TimerCountdown", 0.5f, 1f);

        numberOfEnemies = FindObjectOfType<SpawnManager>().numberOfEnemies;

        //Check Rewards
        if (rewardBullet == true)
        {
            numbOfLevelShots++;
            FindObjectOfType<Player>().setNumberOfShots(numbOfLevelShots);
            GameObject.Find("RocketIcon").GetComponentInChildren<Text>().text = "x " + numbOfLevelShots;
        }
        
        if (rewardTime == true)
        {
            mTimer = mTimer+10;
            timerText.color = new Color(52.0f / 255.0f, 152.0f / 255.0f, 219.0f / 255.0f, 255.0f / 255.0f);
        }

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

            if (mTimer < 60)
            {
                timerText.color = new Color(255.0f, 255.0f, 255.0f, 255.0f);
            }

            if (mTimer <= 10 && !isTimeoutFXPlayed)
            {
                FindObjectOfType<AudioManager>().Play("Timeout");
                isTimeoutFXPlayed = true;
            }
        }
        else
        {
            FindObjectOfType<Player>().timeoutAnim();
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

    public void setRewardTime()
    {
        rewardTime = true;
    }

    public void setRewardBullet()
    {
        rewardBullet = true;

        int extrabullet = FindObjectOfType<SpawnManager>().getLevelShots(sceneIndex) + 1;
        BulletNumberScreen.text = "x" + extrabullet.ToString();
        BulletNumberScreen.color = new Color(52.0f / 255.0f, 152.0f / 255.0f, 219.0f / 255.0f, 255.0f / 255.0f);
    }


}
