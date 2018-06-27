using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    

    private int sceneIndex = 0;
    private static bool created = false;
    private int mTimer = 60;
    private int numberOfEnemies;
    //private int LastLevelIndex = 1;

    // Use this for initialization
    void Start () {
        FindObjectOfType<SpawnManager>().spawnedEnemies = 0;
        FindObjectOfType<SpawnManager>().isSpawned = false;
        FindObjectOfType<SpawnManager>().numberOfEnemies = 1;
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
		
	}

    public void CheckEndOfLevel()
    {
        enemiesDead++;

        if (enemiesDead == FindObjectOfType<SpawnManager>().numberOfEnemies)
        {
            //Level Complete Screen
            sceneIndex++;

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


        player.SetActive(true);
        GameObject.Find("RocketIcon").GetComponentInChildren<Text>().text = "x 8";

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
            GameOver();
        }

        timerText.text = mTimer.ToString();
    }



    
    
}
