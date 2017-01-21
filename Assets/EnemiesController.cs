using UnityEngine;
using System.Collections;

public class EnemiesController : MonoBehaviour {

    //public GameObject enemy;
    public GameObject[] enemies;
    private float spawnTime = 3f;
    //private Transform[] spawnPoints;

	// Use this for initialization
	void Start () {
        enemies = GameObject.FindGameObjectsWithTag("enemy");

        foreach (GameObject currEnemy in enemies)
        {
            currEnemy.SetActive(false);
        }

        Debug.Log(string.Format("NUM OF ENEMIES IS {0}",enemies.Length));
        InvokeRepeating("Spawn", spawnTime, spawnTime);
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void Spawn()
    {
        if (GameService.GetInstance().IsAlive() && !GameService.GetInstance().GetIsPaused())
        {
            float spawnPointX = getRandomXPosition();
            Vector2 position = new Vector2(spawnPointX, 12);
            GameObject enemyObj = (GameObject)Instantiate(enemies[RandomEnemy()], position, Quaternion.identity);
            enemyObj.SetActive(true);
        }
    }

    private float getRandomXPosition()
    {
        Vector3 randomPos3;
        float randomX;

        if (Random.Range(0, 1f) >= 0.5f)
        {
            randomX = Random.Range(Screen.width * 0.6f, Screen.width);
        }
        else
        {
            randomX = Random.Range(0, Screen.width * 0.4f);
        }

        randomPos3 = Camera.main.ScreenToWorldPoint(new Vector3(randomX,0,0));

        return randomPos3.x;
    }

    private int RandomEnemy()
    {
        int numOfEnemies;

        if (enemies.Length > 0)
        {
            numOfEnemies = enemies.Length;
        }
        else
        {
            numOfEnemies = 0;
        }

        return Random.Range(0, numOfEnemies);
    }
}
