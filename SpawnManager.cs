using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour {

    public int numberOfEnemies;
    public bool isSpawned = false;
    public int numberOfShots = 7;
    public int spawnedEnemies = 0;

    //Enemy Speed
    float[,] levelSpeedInput = new float[50, 1] 
    {
      {0.3f},
      {0.3f},
      {0.3f},
      {0.4f},
      {0.5f},
      {0.5f},
      {0.5f},
      {0.6f},
      {0.6f},
      {0.6f},

      {0.8f},
      {0.8f},
      {0.8f},
      {0.9f},
      {0.9f},
      {0.9f},
      {1.0f},
      {1.0f},
      {1.0f},
      {1.0f},

      {1.0f},
      {1.0f},
      {1.0f},
      {1.1f},
      {1.1f},
      {1.1f},
      {1.2f},
      {1.2f},
      {1.2f},
      {1.3f},

      {1.4f},
      {1.4f},
      {1.5f},
      {1.5f},
      {1.7f},
      {1.8f},
      {1.9f},
      {2.0f},
      {2.1f},
      {2.2f},

      {2.3f},
      {2.4f},
      {2.5f},
      {2.6f},
      {2.7f},
      {2.8f},
      {2.9f},
      {2.9f},
      {3.0f},
      {3.0f},};

    //poopTimes, numOfEnemies, numOfShots
    int[,] levelGeneralInput = new int[50, 3]
    {
      {1, 1, 10},
      {2, 1, 9},
      {2, 1, 8},
      {2, 1, 7},
      {1, 2, 8},
      {1, 2, 7},
      {2, 2, 8},
      {2, 2, 7},
      {2, 2, 6},
      {2, 2, 6},

      {1, 1, 6},
      {2, 1, 6},
      {1, 2, 7},
      {1, 2, 7},
      {1, 2, 6},
      {2, 2, 6},
      {2, 2, 6},
      {3, 2, 5},
      {1, 3, 8},
      {1, 3, 7},

      {1, 1, 7},
      {1, 2, 7},
      {2, 2, 8},
      {2, 2, 8},
      {2, 3, 8},
      {2, 3, 8},
      {2, 3, 8},
      {3, 3, 8},
      {3, 4, 8},
      {3, 5, 8},

      {1, 1, 7},
      {1, 2, 7},
      {2, 2, 8},
      {2, 2, 8},
      {2, 3, 8},
      {2, 3, 8},
      {2, 3, 8},
      {3, 3, 8},
      {3, 4, 8},
      {3, 5, 8},
      {1, 1, 7},
      {1, 2, 7},
      {2, 2, 8},
      {2, 2, 8},
      {2, 3, 8},
      {2, 3, 8},
      {2, 3, 8},
      {3, 3, 8},
      {3, 4, 8},
      {3, 5, 8},};



    //int enemyIndex = 0;
    float enemySpeed;
    int poopTimes;
    //float spawnInterval;


    //private EnemyBehavior1 enemy1;
    [SerializeField] private GameObject enemy1Prefab;
    //private static bool created = false;


    // Use this for initialization
    void Start () {


        CheckScene(0);

    }

    private void FixedUpdate()
    {
        if (isSpawned == false)
        {
            CheckStopSpawn();
        }
        
    }


    public void CheckScene (int y) {

        if (y > 50)
        {
            FindObjectOfType<GameManager>().EndGame();
        }

        SetLevel(getLevelSpeed(y), getLevelPoopTimes(y), getLevelEnemies(y), getLevelShots(y));     

    }


    private void SetLevel (float _enemySpeed, int _poopTimes, int _numberOfEnemies, int _numberOfShots)
    {
        float maxSpawnInterval = 3.0f;
        spawnedEnemies = 0;

        enemySpeed = _enemySpeed;
        poopTimes = _poopTimes;
        numberOfEnemies = _numberOfEnemies;
        numberOfShots = _numberOfShots;     
        //Game manager takes care of playerbullets
        //FindObjectOfType<Player>().setNumberOfShots(numberOfShots);
        //spawnInterval = Random.Range(1f, maxSpawnInterval);
        InvokeRepeating("SpawnEnemy", 1f, maxSpawnInterval);
    }


    private void SpawnEnemy()
    {
 
        float ySpawn = Random.Range (0f, 3.8f);
        int direction = Random.Range(0, 2) * 2 - 1;

        GameObject enemy = Instantiate(enemy1Prefab, new Vector2(4 * direction, ySpawn), Quaternion.identity) as GameObject;

        enemy.GetComponent<EnemyBehavior1>().SetPoopTimes(poopTimes);
        enemy.GetComponent<EnemyBehavior1>().SetSpeed(enemySpeed);

        spawnedEnemies++;
        //Debug.Log("spawned Enemies: " + spawnedEnemies);
        //Debug.Log("isSpawned: " + isSpawned);


    }

    private void CheckStopSpawn()
    {

        if (spawnedEnemies < numberOfEnemies)
        {
            return;
        }
        CancelInvoke("SpawnEnemy");
        //Debug.Log("CancelInvoke");
        //Debug.Log("num of Enemies: " + numberOfEnemies);
        //Debug.Log("spawned Enemies: " + spawnedEnemies);

        isSpawned = true;

    }

    public void SetNumberOfShots(int _numberfShots)
    {
        numberOfShots = _numberfShots;
    }

    public int GetNumberOfShots()
    {
        return numberOfShots;
    }

    public float getLevelSpeed (int level)
    {
        return levelSpeedInput[level, 0];
    }

    public int getLevelPoopTimes (int level)
    {
        return levelGeneralInput[level, 0];
    }

    public int getLevelEnemies(int level)
    {
        return levelGeneralInput[level, 1];
    }

    public int getLevelShots(int level)
    {
        numberOfShots = levelGeneralInput[level, 2];
        return levelGeneralInput[level, 2];
    }

    public float GetLevelWind(int level)
    {
       
        float wind = 0;
        int i = level;

        //random wind for levels 20 to 50
        //min 20/50 and max 50/50
        if (i >= 10)
        {
            int windDirection = Random.Range(0, 2) * 2 - 1;
            wind = Random.Range(i, i * 1.2f) * windDirection / 100f;
            wind = Mathf.Round(wind * 100f) / 100f;
            //Debug.Log("windspawn: " + wind + " dir: " + windDirection);
        }
        
        return wind;
    }

}
