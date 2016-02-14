using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Transform[] spawnPoints;
    public GameObject[] enemyShips;
    public GameObject enemyBoss;
    public float startSpawnTime = 1.2f;
    public static GameManager instance = null;
    public int level { get;  private set; }

    private int totalEnemy = 20;
    private float spawnTime = 1.2f;
    private bool isPlaying = true;
    private int takeDown;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            level = 1;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        foreach (var p in spawnPoints)
        {
            DontDestroyOnLoad(p);
        }
        EnemyShipScript.onEnemyShipDestroy += EnemyShipScript_onEnemyShipDestroy;
        StartCoroutine(SpawnEnemy());
    }

    private void EnemyShipScript_onEnemyShipDestroy()
    {
        takeDown += 1;
    }

    // Update is called once per frame
    void Update () {
        if (takeDown == totalEnemy)
        {
            MeetBoss();
        }
	}

    public void StartNewLevel()
    {
        level++;
        //Start new level
        InitGame();
    }

    void InitGame()
    {
        //Increase the difficulty
        totalEnemy = (int)Mathf.Log(level, 1.03f);
        //totalEnemy = 1;
        spawnTime = startSpawnTime * (1f - level / 25f);
        Debug.Log("Level: " + level + 
                ", Enemy: " + totalEnemy + 
                ", SpawnDelay: " + spawnTime);
        //Call SpawnEnemy to generate enemy ships.
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(.1f);
        for (int i = 0; i < totalEnemy; i++)
        {
            if (isPlaying)
            {
                Vector3 spawnPos = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                GameObject enemy = enemyShips[Random.Range(0, enemyShips.Length)];
                Instantiate(enemy, spawnPos, Quaternion.identity);
                yield return new WaitForSeconds(spawnTime);
            }
        }
    }

    void MeetBoss()
    {
        takeDown = 0;
        enemyBoss.SetActive(true);
        enemyBoss.GetComponent<EnemyBossScript>().SendMessage("Attack");
    }

    public void GameOver()
    {
        Debug.Log("game over");
        isPlaying = false;
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemyList)
        {
            Destroy(enemy);
        }
        StopCoroutine(SpawnEnemy());
        //UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
