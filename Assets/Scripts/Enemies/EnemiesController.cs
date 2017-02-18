﻿using UnityEngine;
using System.Collections;

public class EnemiesController : MonoBehaviour {

    public static EnemiesController instance;
    public GameObject[] enemies;

    private const float X_POS_OFFSET = 0.05f;
    private float lastSpawxPositionX;
    private float spawnTime = 3f;
    private float spawnDelay = 3f;
    private int lastDiffuclty;
    private int numOfCurrSpwans;
    private int numOfTotalSpwans;

    private bool didLastShotHitRight;
    private bool didLastShotHitLeft;

    //private Transform[] spawnPoints;

	// Use this for initialization
	void Start () {
        enemies = GameObject.FindGameObjectsWithTag("enemy");
        
        foreach (GameObject currEnemy in enemies)
        {
            currEnemy.SetActive(false);
        }

        //Debug.Log(string.Format("NUM OF ENEMIES IS {0}",enemies.Length));

        if (instance == null)
        {
            instance = this;
        }
        //InvokeRepeating("Spawn", spawnTime, spawnTime);
	}

    public void InitNewGame()
    {
        lastSpawxPositionX = 0;
        numOfCurrSpwans = 0;
        numOfTotalSpwans = 3;
        lastDiffuclty = 1;
        spawnDelay = spawnTime;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy"))
        {
            Destroy(enemy);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (GameService.GetInstance().IsAlive() && !GameService.GetInstance().GetIsPaused())
        {
            if (GameService.GetInstance().GetGameDiffuclty() != lastDiffuclty)
            {
                lastDiffuclty = GameService.GetInstance().GetGameDiffuclty();
                

                if (lastDiffuclty % 3 == 0 && numOfTotalSpwans < 7)
                {
                    numOfTotalSpwans++;
                }
            }

            StartCoroutine(waitAndSpawn());

            //kill nearest enemy
            GameObject nearestEnemy = GetNearestEnemy(Input.touches);

            if (GameService.GetInstance().IsObjectInTapArea(nearestEnemy, Input.touches) && AmmoController.GetInstance().HasAmmo())
            {
                nearestEnemy.GetComponent<Enemy>().Kill();
                GameService.GetInstance().addToPoints(1);
                //numOfCurrSpwans--;
            }
        }
	}

    public static EnemiesController GetInstance()
    {
        return instance;
    }

    private void Spawn()
    {
        if (GameService.GetInstance().IsAlive() && !GameService.GetInstance().GetIsPaused())
        {
            float spawnPointX = getRandomXPosition();
            Vector2 position = new Vector2(spawnPointX, 12);
            GameObject enemyObj = (GameObject)Instantiate(enemies[RandomEnemy()], position, Quaternion.identity);
            enemyObj.SetActive(true);

            numOfCurrSpwans--;
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
        if(GameService.GetInstance().IsAlive() && !GameService.GetInstance().GetIsPaused())
        {
            if (numOfCurrSpwans < numOfTotalSpwans)
            {
                numOfCurrSpwans++;
                spawnDelay = Random.Range(spawnTime / 2, spawnTime - lastDiffuclty * 0.05f);
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