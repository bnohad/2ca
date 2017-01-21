﻿using UnityEngine;
using System.Collections;

public class GameService : MonoBehaviour {
   
    private const string PLAYER_OBJECT = "player";
    private const string ENEMY_OBJECT = "enemy";
    private const string MAP_OBJECT = "map";

    private static GameService instance;

    private bool isMenu;
    private bool isPaused;
    private bool isAlive;

    private float gameSpeed;
    private int gameDiffuclty;

    private GameObject player;
    private GameObject[] mapObjects;

    //private ArrayList enemies;

    private Rect leftScreenSide;
    private Rect rightScreenSide;

    public GameService()
    {
        InitGameInstance();
    }

	// Use this for initialization
	void Start () {
      
        
        InitGameObjects();
        InitGameTapControl();

        StartNewGame();
        //SpawnNewEnemy();
	}
	
	// Update is called once per frame
	void Update () {
        if (IsAlive())
        {
            SceneMovement.GetInstance().SetSpeed(gameSpeed);
        }
        else
        {
            SceneMovement.GetInstance().SetSpeed(0);
        }
	}

    private void InitGameTapControl()
    {
        Vector2 middle = TransformPixelToWorldPoint(new Vector2(1, 1));

        Debug.Log(string.Format("TAP AREA {0} {1}", middle.x, middle.y));

        leftScreenSide = new Rect(-middle.x, -middle.y, middle.x, 2*middle.y);
        rightScreenSide = new Rect(0, -middle.y, middle.x, 2*middle.y);
        
        //leftScreenSide = new Rect(0, 0, screenVector2.x / 2, 2 * screenVector2.y);
        //rightScreenSide = new Rect(screenVector2.x, 0, screenVector2.x / 2, 2 * screenVector2.y);
    }

    private void InitGameObjects()
    {
        player = GameObject.Find(PLAYER_OBJECT);
        //enemies = new ArrayList();
        
        InitMapObjects();
    }

    public Vector2 TransformPixelToWorldPoint(Vector2 position)
    {
        Vector2 ret;

        Vector3 screenVector3 = new Vector3(position.x, position.y, 0);
        ret = (Vector2)Camera.main.ViewportToWorldPoint(screenVector3);

        return ret;
    }

    public void SpawnNewEnemy()
    {
        //Enemy newEnemy = new Enemy();
    }

    private void InitGameInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void InitMapObjects()
    {
        mapObjects = GameObject.FindGameObjectsWithTag(MAP_OBJECT);

        float mapHeight = 0;
        int numOfMaps = mapObjects.Length;
        int currentCounter = 0;

        foreach(GameObject map in mapObjects) {
            BackgroundScript mapInstance = map.GetComponent<BackgroundScript>().GetInstance();

            if (mapHeight == 0)
            {
                mapHeight = mapInstance.GetHeight();
            }

            mapInstance.SetYPosition(mapHeight * currentCounter++);
        }
    }

    public bool IsObjectInWorldView(GameObject obj)
    {
        bool ret = false;
        Vector2 objPos = obj.transform.localPosition;
        //Debug.Log(string.Format("OBJECT POS: {0} {1}", objPos.x, objPos.y));

        if (rightScreenSide.Contains(objPos) || leftScreenSide.Contains(objPos))
        {
            ret = true;
        }

        return ret;
    }

    public bool IsObjectInTapArea(Vector2 objectPos, Touch[] touchPoints)
    {
        bool ret = false;

        if (IsTapRight(touchPoints) && rightScreenSide.Contains(objectPos))
        {
            ret = true;
        }
        else if (IsTapLeft(touchPoints) && leftScreenSide.Contains(objectPos))
        {
            ret = true;
        }
        else if(IsTapLeft(touchPoints) || IsTapRight(touchPoints))
        {
            Debug.Log(string.Format("NOT IN TOUCH AREA {0} {1}", objectPos.x, objectPos.y));
        }

        return ret;
    }

    public bool IsTapRight(Touch[] touchPoints)
    {
        return Input.GetKey("right") || IsTapInRect(rightScreenSide, touchPoints);
    }

    public bool IsTapLeft(Touch[] touchPoints)
    {
        return Input.GetKey("left") || IsTapInRect(leftScreenSide, touchPoints);
    }

    private bool IsTapInRect(Rect rect, Touch[] touchPoints) 
    {
        bool ret = false;

        if (touchPoints.Length > 0)
        {
            foreach (Touch point in touchPoints)
            {
                if(rect.Contains(Camera.main.ScreenToWorldPoint(point.position))) {
                    ret = true;
                    break;
                }
            }
        }

        return ret;
    }


    public static GameService GetInstance()
    {
        return instance;
    }

    public void StartNewGame()
    {
        SetGameSpeed(3f);
        SetGameDiffuclty(1);

        IsAlive(true);
        SetIsPaused(false);
    }

    public void IsAlive(bool status)
    {
        isAlive = status;
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public float GetGameSpeed()
    {
        return gameSpeed;
    }

    public void SetGameSpeed(float speed)
    {
        if (speed > 0f)
        {
            Debug.Log(string.Format("GAME SERVICE SET SPEED {0}", speed));
            gameSpeed = speed;
        }
    }

    public float GetGameDiffuclty()
    {
        return gameDiffuclty;
    }

    public void SetGameDiffuclty(int diffuclty)
    {
        if (diffuclty > 0)
        {
            gameDiffuclty = diffuclty;
        }
    }

    public bool GetIsPaused()
    {
        return isPaused;
    }

    public void SetIsPaused(bool state)
    {
        isPaused = state;
    }
}
