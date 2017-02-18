using UnityEngine;
using System.Collections;

public class EnemiesController : MonoBehaviour {

    public GameObject[] enemies;

    public static EnemiesController instance;

    private const float X_POS_OFFSET = 0.05f;
    private float lastSpawxPositionX;
    private float spawnTime = 3f;
    private float spawnDelay = 3f;
    private int numOfCurrSpwans;
    private int numOfTotalSpwans;
    
    private bool didLastShotHitRight;
    private bool didLastShotHitLeft;

    private int _spawnCount;
    public int SpawnCount {
        get
        {
            return _spawnCount;
        }
        set {
            if(value >= 0)
            {
                _spawnCount = value;
            }
        }
    }

    public int ParalelEnemis
    {
        get
        {
            return numOfTotalSpwans;
        }
        set
        {
            numOfTotalSpwans = value;
        }
    }

    public bool WaveFinished()
    {
        return SpawnCount == 0;
    }

    public EnemiesController()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void fetchAllEnemyGameObjects()
    {
        // List of all possible enemies
        enemies = GameObject.FindGameObjectsWithTag("enemy");

        foreach (GameObject currEnemy in enemies)
        {
            currEnemy.SetActive(false);
        }
    }

    public void DestroyAllEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy"))
        {
            Destroy(enemy);
        }
    }

	// Use this for initialization
	void Start () {
        fetchAllEnemyGameObjects();
	}

    public void InitNewGame()
    {
        lastSpawxPositionX = 0;
        numOfCurrSpwans = 0;
        numOfTotalSpwans = 3;
//        lastDiffuclty = 1;
        spawnDelay = spawnTime;

        DestroyAllEnemies();
    }
	
	// Update is called once per frame
	void Update () {
        if (GameService.GetInstance().IsPlaying() && WaveController.GetInstance().Ongoing)
        {
            StartCoroutine(waitAndSpawn());
            //kill nearest enemy
            GameObject nearestEnemy = GetNearestEnemy(Input.touches);

            if (GameService.GetInstance().IsObjectInTapArea(nearestEnemy, Input.touches) && AmmoController.GetInstance().HasAmmo())
            {
                nearestEnemy.GetComponent<Enemy>().Kill();
                GameService.GetInstance().addToPoints(1);
            }


        }
        //Debug.Log(string.Format("{0}, {1}", GameService.GetInstance().IsPlaying(), WaveController.GetInstance().Ongoing));
            
        
	}

    public static EnemiesController GetInstance()
    {
        return instance;
    }

    private void Spawn()
    {
        if (GameService.GetInstance().IsPlaying())
        {
            float spawnPointX = getRandomXPosition();
            Vector2 position = new Vector2(spawnPointX, 12);
            GameObject enemyObj = (GameObject)Instantiate(enemies[RandomEnemy()], position, Quaternion.identity);
            enemyObj.SetActive(true);

            numOfCurrSpwans--;
            SpawnCount--;
        }
    }

    public GameObject GetNearestEnemy(Touch[] touches)
    {
        GameObject nearestEnemy = null;

        Debug.Log(string.Format("NUM OF ENEMIES ON SCREEN {0}", GameObject.FindGameObjectsWithTag("enemy").Length));

            foreach (GameObject currEnemy in GameObject.FindGameObjectsWithTag("enemy"))
            {
                if (nearestEnemy == null && currEnemy.GetComponent<Enemy>().IsAlive() && GameService.GetInstance().IsObjectInTapArea(currEnemy, touches))
                {
                    nearestEnemy = currEnemy;
                }
                else if (currEnemy != null && nearestEnemy != null && !currEnemy.Equals(nearestEnemy) && GameService.GetInstance().IsObjectInTapArea(currEnemy, touches) && nearestEnemy.transform.localPosition.y > currEnemy.transform.localPosition.y && currEnemy.GetComponent<Enemy>().IsAlive())
                {
                    nearestEnemy = currEnemy;
                }
            }

        return nearestEnemy;
    }

    IEnumerator waitAndSpawn()
    {
        if(GameService.GetInstance().IsPlaying())
        {
            if (numOfCurrSpwans < numOfTotalSpwans && SpawnCount > 0)
            {
                numOfCurrSpwans++;
                yield return new WaitForSeconds(spawnDelay);
                Spawn();
            }
        }
    }

    private float getRandomXPosition()
    {
        Vector3 randomPos3;
        float randomX;

        if (randomBool())
        {
            randomX = Random.Range(Screen.width * 0.55f, Screen.width * 0.95f);
            
        }
        else
        {
            randomX = Random.Range(Screen.width * 0.05f, Screen.width * 0.45f);
        }

        if(Mathf.Abs(randomX - lastSpawxPositionX) <= X_POS_OFFSET)
        {
            randomX -= X_POS_OFFSET * 2;
        }

        lastSpawxPositionX = randomX;
        randomPos3 = Camera.main.ScreenToWorldPoint(new Vector3(randomX,0,0));

        return randomPos3.x;
    }

    private bool randomBool()
    {
        return Random.Range(0, 10f) >= 5f;
    }

    private int RandomEnemy()
    {
        return Random.Range(0, enemies.Length);
    }
}
