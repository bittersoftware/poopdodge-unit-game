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
    float[,] levelSpeedInput = new float[10, 1] 
    { {0.5f},
      {0.5f},
      {0.5f},
      {1.0f},
      {1.0f},
      {1.2f},
      {1.5f},
      {1.7f},
      {2.0f},
      {2.5f},};
    //poopTimes, numOfEnemies, numOfShots
    int[,] levelGeneralInput = new int[10, 3]
    { {1, 1, 7},
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
    float spawnInterval;


    //private EnemyBehavior1 enemy1;
    [SerializeField] private GameObject enemy1Prefab;
    private static bool created = false;


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
           
        if (y == 0)
        //Speed, poopTimes, numOfEnemies, numOfShots
        {
            //SetLevel(0.5f, 1, 1, 7);
            SetLevel(getLevelSpeed(y), getLevelPoopTimes(y), getLevelEnemies(y), getLevelShots(y));
        }
        else if (y == 1)
        {
            //SetLevel(0.5f, 1, 2, 7);
            SetLevel(getLevelSpeed(y), getLevelPoopTimes(y), getLevelEnemies(y), getLevelShots(y));
        }
        else if (y == 2)
        {
            //SetLevel(0.5f, 2, 2, 8);
            SetLevel(getLevelSpeed(y), getLevelPoopTimes(y), getLevelEnemies(y), getLevelShots(y));
        }
        else if (y == 3)
        {
            //SetLevel(1f, 2, 2, 8);
            SetLevel(getLevelSpeed(y), getLevelPoopTimes(y), getLevelEnemies(y), getLevelShots(y));
        }
        else if (y == 4)
        {
            //SetLevel(1f, 2, 3, 8);
            SetLevel(getLevelSpeed(y), getLevelPoopTimes(y), getLevelEnemies(y), getLevelShots(y));
        }
        else if (y == 5)
        {
            //SetLevel(1.2f, 2, 3, 8);
            SetLevel(getLevelSpeed(y), getLevelPoopTimes(y), getLevelEnemies(y), getLevelShots(y));
        }
        else if (y == 6)
        {
            //SetLevel(1.5f, 2, 3, 8);
            SetLevel(getLevelSpeed(y), getLevelPoopTimes(y), getLevelEnemies(y), getLevelShots(y));
        }
        else if (y == 7)
        {
            //SetLevel(1.7f, 3, 3, 8);
            SetLevel(getLevelSpeed(y), getLevelPoopTimes(y), getLevelEnemies(y), getLevelShots(y));
        }
        else if (y == 8)
        {
            //SetLevel(2f, 3, 4, 8);
            SetLevel(getLevelSpeed(y), getLevelPoopTimes(y), getLevelEnemies(y), getLevelShots(y));
        }
        else if (y == 9)
        {
            //SetLevel(2.5f, 3, 5, 8);
            SetLevel(getLevelSpeed(y), getLevelPoopTimes(y), getLevelEnemies(y), getLevelShots(y));
        }
        else
        {
            FindObjectOfType<GameManager>().EndGame();
        }

    }


    private void SetLevel (float _enemySpeed, int _poopTimes, int _numberOfEnemies, int _numberOfShots)
    {
        float maxSpawnInterval = 3.0f;
        spawnedEnemies = 0;

        enemySpeed = _enemySpeed;
        poopTimes = _poopTimes;
        numberOfEnemies = _numberOfEnemies;
        numberOfShots = _numberOfShots;     
        
        FindObjectOfType<Player>().setNumberOfShots(numberOfShots);
        spawnInterval = Random.Range(1f, maxSpawnInterval);
        InvokeRepeating("SpawnEnemy", 1f, maxSpawnInterval);

    }


    private void SpawnEnemy()
    {
 
        float ySpawn = Random.Range (0f, 3.8f);

        GameObject enemy = Instantiate(enemy1Prefab, new Vector2(4, ySpawn), Quaternion.identity) as GameObject;

        enemy.GetComponent<EnemyBehavior1>().SetPoopTimes(poopTimes);
        enemy.GetComponent<EnemyBehavior1>().SetSpeed(enemySpeed);

        spawnedEnemies++;
        Debug.Log("spawned Enemies: " + spawnedEnemies);
        Debug.Log("isSpawned: " + isSpawned);


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

}
