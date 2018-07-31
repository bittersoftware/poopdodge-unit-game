using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public Animator transitionAnim;
    public GameObject levelCompleteScreen;
    public GameObject gameOverScreen;
    public GameObject player;
    public GameObject transitionImage;
    public Text timerText;
    public Text LevelNumberScreen;
    public Text EnemiesNumberScreen;
    public Text BulletNumberScreen;
    public GameObject windLeft;
    public GameObject windRight;
    public int enemiesDead = 0;
    public int bulletsDestroyed = 0;
    public float wind;
    public bool isNewRecord = false;



    [SerializeField]
    private int currentLevel;
    [SerializeField]
    private float currentAcc;
    [SerializeField]
    private int totalShots;
    [SerializeField]
    private int totalEnemiesKilled;
    private int sceneIndex = 0;
    [SerializeField]
    private int mTimer = 60;
    private int numberOfEnemies;
    [SerializeField]
    private int numbOfLevelShots;


    private bool isTimeoutFXPlayed;
    private bool rewardTime;
    private bool rewardBullet;
    private int totalTime;
    private int startTime;

    //#3498db blue HEX
    //(52, 152, 219) blue rgb

    // Use this for initialization
    void Start () {
        //Transition Anim
        StartCoroutine(ZoomDownAnim());

        //For HighScore 
        currentLevel = sceneIndex + 1;
        currentAcc = 0f;
        totalShots = 0;
        totalTime = 0;
        startTime = mTimer;
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
        if (bulletsDestroyed == numbOfLevelShots && enemiesDead < numberOfEnemies)
        {
            //Debug.Log("bullet Destroyed: " + bulletsDestroyed + "number of shots: " + FindObjectOfType<SpawnManager>().numberOfShots + "Enemies Dead: " + enemiesDead + "number of Enemies: " + numberOfEnemies);
            GameOver();
        }
        //updtade Accuracy
        if (totalShots > 0)
        {
            currentAcc = ((float)totalEnemiesKilled / (float)totalShots) * 100;
            //Debug.Log("RealTime CurrentAcc: " + currentAcc);
        }
    }

    public void CheckEndOfLevel()
    {
        enemiesDead++;
        //forHighScore
        totalEnemiesKilled++;
        

        if (enemiesDead == FindObjectOfType<SpawnManager>().numberOfEnemies)
        {
            //Check highscore when level is completed
            //Debug.Log("HighLevel: " + GetHighestLevel());
            //Debug.Log("HighAcc: " + GetHighestAcc());
            //Debug.Log("CurhLevel: " + GetCurrentLevel());
            //Debug.Log("CurLAcc: " + GetCurrentAcc());
            //Debug.Log("CurLAccVar: " + currentAcc);
            //checkHighScore();

            //totalTime for HighScore Log
            totalTime = totalTime + startTime - mTimer;

            //Debug.Log ("totalTime: " + totalTime + "StartTime: " + startTime + "mTimer: " + mTimer);

            //AudioFX - Timeout Management
            FindObjectOfType<AudioManager>().Stop("Timeout");
            

            //reset Reward
            //For rewards
            rewardTime = false;
            rewardBullet = false;

            //Transition Anim
            StartCoroutine(ZoomUpAnim());

            //Level Complete Screen
            sceneIndex++;

            LevelNumberScreen.text = sceneIndex.ToString();
            EnemiesNumberScreen.text = "x" + FindObjectOfType<SpawnManager>().getLevelEnemies(sceneIndex).ToString();
            BulletNumberScreen.text = "x" + FindObjectOfType<SpawnManager>().getLevelShots(sceneIndex).ToString();
            BulletNumberScreen.color = new Color(0f / 255.0f, 0f / 255.0f, 0f / 255.0f, 255.0f / 255.0f);

            //Transition Anim
            StartCoroutine(ZoomDownAnim());
            
        }
    }

    public void NextLevel()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        isNewRecord = false;
        enemiesDead = 0;
        bulletsDestroyed = 0;
        player.transform.position = new Vector2 (-0.2f, -4f);
        FindObjectOfType<BulletEmissor>().angle = 0f;
        numbOfLevelShots = FindObjectOfType<SpawnManager>().getLevelShots(sceneIndex);
        GameObject.Find("RocketIcon").GetComponentInChildren<Text>().text = "x " + numbOfLevelShots;

        //Setup Wind
        wind = FindObjectOfType<SpawnManager>().GetLevelWind(sceneIndex);

        //Debug.Log("WindRaw: " + wind);
        //Debug.Log("WindInt: " + (int) wind*10);
        //Mathf.Round(wind * 100f) / 100f;

        if ((Mathf.Round(wind * 100f) / 10f) >= 1)
        {
            GameObject.Find("WindIcon").GetComponentInChildren<Text>().text = "" + (Mathf.Abs(Mathf.Round(wind * 100f) / 10f));
            windRight.SetActive(true);
        }
        else if ((Mathf.Round(wind * 100f) / 10f) <= -1)
        {
            GameObject.Find("WindIcon").GetComponentInChildren<Text>().text = "" + (Mathf.Abs(Mathf.Round(wind * 100f) / 10f));
            windLeft.SetActive(true);
        }

        Debug.Log("managerwind: " + (Mathf.Round(wind * 100f) / 10f));

        player.SetActive(true);
        FindObjectOfType<Player>().setNumberOfShots(numbOfLevelShots);
        isTimeoutFXPlayed = false;
        mTimer = 60;
        timerText.text = mTimer.ToString();

        //Check Rewards
        if (rewardBullet == true)
        {
            //Debug.Log("Before Reward Player:" + FindObjectOfType<Player>().getNumberOfShots()); 
            numbOfLevelShots++;
            FindObjectOfType<Player>().setNumberOfShots(numbOfLevelShots);
            GameObject.Find("RocketIcon").GetComponentInChildren<Text>().text = "x " + numbOfLevelShots;
            //Debug.Log("After Reward Player:" + FindObjectOfType<Player>().getNumberOfShots());
        }

        if (rewardTime == true)
        {
            mTimer = mTimer + 10;
            timerText.text = mTimer.ToString();
            timerText.color = new Color(52.0f / 255.0f, 152.0f / 255.0f, 219.0f / 255.0f, 255.0f / 255.0f);
        }

        startTime = mTimer;

        //Start Level Preset
        FindObjectOfType<SpawnManager>().spawnedEnemies = 0;
        FindObjectOfType<SpawnManager>().isSpawned = false;
        FindObjectOfType<SpawnManager>().CheckScene(sceneIndex);
        numberOfEnemies = FindObjectOfType<SpawnManager>().numberOfEnemies;

        levelCompleteScreen.SetActive(false);
        InvokeRepeating("TimerCountdown", 1f, 1f);
        
    }



    public void EndGame()
    {
        //Game is Over
        //Restart();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GameOver()
    {
        //AudioFX - Timeout Management
        FindObjectOfType<AudioManager>().Stop("Timeout");

        bulletsDestroyed = 0;
        enemiesDead = 0;
        sceneIndex++;

        DestroyAllObjects();

        CancelInvoke("TimerCountdown");

        player.SetActive(false);
        gameOverScreen.SetActive(true);
        //Debug.Log("Game Over");
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

    private void checkHighScore()
    {
        if (currentLevel == PlayerPrefs.GetInt("HighScore", 0) && currentAcc > GetHighestAcc())
        {
            PlayerPrefs.SetString("Date", System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

            PlayerPrefs.SetInt("HighScore", currentLevel);
            PlayerPrefs.SetFloat("HighAcc", currentAcc);

            PlayerPrefs.SetInt("EnemiesKilled", totalEnemiesKilled);
            PlayerPrefs.SetInt("BullestsUsed", totalShots);
            PlayerPrefs.SetInt("TotalTime", totalTime);
                        
            isNewRecord = true;

        }
        else if (currentLevel > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetString("Date", System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

            PlayerPrefs.SetInt("HighScore", currentLevel);
            PlayerPrefs.SetFloat("HighAcc", currentAcc);

            PlayerPrefs.SetInt("EnemiesKilled", totalEnemiesKilled);
            PlayerPrefs.SetInt("BullestsUsed", totalShots);
            PlayerPrefs.SetInt("TotalTime", totalTime);

            isNewRecord = true;

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

    private IEnumerator ZoomUpAnim()
    {
        //transition Anim
        transitionAnim.SetTrigger("zoomup");


        //yield return new WaitForSeconds(5f);
        yield return new WaitForSeconds(0.7f);
        //CheckHighscore
        checkHighScore();
        //increment Current Level for High Score only after reading it
        currentLevel++;
        //Finished loading complete level screen
        levelCompleteScreen.SetActive(true);
        //Re-enalbe reward buttons
        FindObjectOfType<NextLevel>().EnableRewardButtons();

        windRight.SetActive(false);
        windLeft.SetActive(false);

        DestroyAllObjects();
        player.SetActive(false);

        CancelInvoke("TimerCountdown");
    }

    private IEnumerator ZoomDownAnim()
    {
        //transition Anim
        transitionAnim.SetTrigger("zoomdown");

        //yield return new WaitForSeconds(0.4f);
        yield return new WaitForSeconds(0.5f);
    }

}
