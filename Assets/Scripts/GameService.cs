using UnityEngine;
using System.Collections;

public class GameService : MonoBehaviour {
   
    private const string PLAYER_OBJECT = "player";
    private const string ENEMY_OBJECT = "enemy";
    private const string MAP_OBJECT = "map";

    private static GameService instance;

    private bool isMenu;
    private bool isCredits;
    private bool isPaused;
    private bool isGameover;
    private bool isAlive;
    private bool wasNumOfKillesChanged;

    private float gameSpeed;
    private int gameDiffuclty;
    private int numOfKills;
    private int numOfLives;
    private int levelUpMultiplyer;

    private GameObject player;
    private GameObject playerHit;
    private GameObject[] mapObjects;

    private GameObject goodText;
    private GameObject gameoverScreen;
    private GameObject startBtn;
    private GameObject creditsBtn;

    private GameObject menuScreen;
    private GameObject menuStartBtn;
    private GameObject menuCreditsBtn;

    private GameObject creditsScreen;

    private GameObject pauseScreen;
    private GameObject pauseBackground;
    private GameObject continueButton;
    private GameObject pauseButton;

    private GUIStyle scoreStyle;
    private GUIStyle gameoverStyle;
    private Rect ammoRect;
    private Rect scoreRect;
    private Rect gameoverRect;

    private Rect leftScreenSide;
    private Rect rightScreenSide;

    private static AmmoController ammo;

    public GameService()
    {
        InitGameInstance();
    }

    void OnGUI()
    {
        if (IsAlive())
        {
            GUI.Label(scoreRect, string.Format("Score: {0}", numOfKills), scoreStyle);
            GUI.Label(ammoRect, string.Format("Ammo: {0}", ammo.GetAmmoCount()), scoreStyle);
        }
        else if (isGameover)
        {
            GUI.Label(gameoverRect, string.Format("Score: {0}", numOfKills), gameoverStyle);
        }
    }

	// Use this for initialization
	void Start () {

        InitGameObjects();
        initGUIStyles();
        InitGameTapControl();

        IsAlive(false);
        isMenu = true;
        isGameover = false;
        isCredits = false;

        toggleMenu(true);
        //StartNewGame();
        //SpawnNewEnemy();
	}
	
	// Update is called once per frame
	void Update () {
        if (IsAlive() && !GetIsPaused())
        {
            SceneMovement.GetInstance().SetSpeed(gameSpeed);
            updateGameDiffuclty();
        }
        else
        {
            if(GetIsPaused())
            {

            }

            SceneMovement.GetInstance().SetSpeed(0);
        }
	}

    private void initGUIStyles()
    {
        scoreStyle = new GUIStyle();
        scoreStyle.fontSize = 20;
        scoreStyle.normal.textColor = Color.black;
        //style.alignment = TextAnchor.MiddleCenter;
        scoreStyle.fontStyle = FontStyle.Bold;

        gameoverStyle = new GUIStyle();
        gameoverStyle.fontSize = 40;
        gameoverStyle.normal.textColor = Color.black;
        gameoverStyle.alignment = TextAnchor.MiddleCenter;
        gameoverStyle.fontStyle = FontStyle.Bold;

        scoreRect = new Rect(10, 10, 100, 20);
        ammoRect = new Rect(200, 10, 100, 20);
        gameoverRect = new Rect(0, Screen.height/2, Screen.width/4, Screen.height/2);
    }

    private void updateGameDiffuclty()
    {
        int currDiffuclty = GetGameDiffuclty();

        if (numOfKills % (currDiffuclty * levelUpMultiplyer) == 0 && wasNumOfKillesChanged)
        {
            levelUpMultiplyer++;
            wasNumOfKillesChanged = false;
            SetGameDiffuclty(currDiffuclty + 1);
            ammo.AddBullets(currDiffuclty * 3);

            if (numOfKills > 0)
            {
                goodText.SetActive(true);
            }
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
        if(ammo == null)
        {
            ammo = new AmmoController();
        }

        player = GameObject.Find(PLAYER_OBJECT);
        playerHit = GameObject.Find("Hit");

        goodText = GameObject.Find("GoodText");

        gameoverScreen = GameObject.Find("MenuScreen");

        pauseScreen = GameObject.Find("PauseScreen");
        pauseButton = GameObject.Find("PauseButton");
        continueButton = GameObject.Find("ContinueButton");

        menuScreen = GameObject.Find("OpenScreen");
        creditsScreen = GameObject.Find("CreditsScreen");

        goodText.SetActive(false);
        //playerHit.SetActive(false);

        playerHit.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

        InitMapObjects();
    }

    IEnumerator FadeObject(GameObject obj, bool fadeAway)
    {
        //obj.SetActive(true); ;

        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over X second backwards
            for (float i = 0.5f; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i);
                //img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over X second
            for (float i = 0; i <= 0.5f; i += Time.deltaTime)
            {
                // set color with i as alpha
                obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i);
                //img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }

    public void addToPoints(int points)
    {
        wasNumOfKillesChanged = true;
        numOfKills += points;
    }

    public void takeHit()
    {
        if (IsAlive())
        {
            StartCoroutine(FadeObject(playerHit, true));
            numOfLives -= 1;
            Debug.Log(string.Format("NUM OF LIVES: {0}", numOfLives));
            if (numOfLives == 0)
            {
                //show game over
                IsAlive(false);
            }
        }
        
    }

    public int GetNumOfLives()
    {
        return numOfLives;
    }

    public Vector2 TransformPixelToWorldPoint(Vector2 position)
    {
        Vector2 ret;

        Vector3 screenVector3 = new Vector3(position.x, position.y, 0);
        ret = (Vector2)Camera.main.ViewportToWorldPoint(screenVector3);

        return ret;
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
//        Vector2 objPos = ;
        //Debug.Log(string.Format("OBJECT POS: {0} {1}", objPos.x, objPos.y));

        if (rightScreenSide.Contains(obj.transform.localPosition) || leftScreenSide.Contains(obj.transform.localPosition))
        {
            ret = true;
        }

        return ret;
    }

    public bool IsObjectInTapArea(GameObject obj, Touch[] touchPoints)
    {
        bool ret = false;

        if (obj == null)
        {
            return ret;
        }

        if (IsTapRight(touchPoints) && rightScreenSide.Contains(obj.transform.localPosition))
        {
            ret = true;
        }
        else if (IsTapLeft(touchPoints) && leftScreenSide.Contains(obj.transform.localPosition))
        {
            ret = true;
        }
        else if(IsTapLeft(touchPoints) || IsTapRight(touchPoints))
        {
            //Debug.Log(string.Format("NOT IN TOUCH AREA {0} {1}", objectPos.x, objectPos.y));
        }

        return ret;
    }

    public bool IsTapRight(Touch[] touchPoints)
    {
        return Input.GetKeyDown("right") || IsTapInRect(rightScreenSide, touchPoints);
    }

    public bool IsTapLeft(Touch[] touchPoints)
    {
        return Input.GetKeyDown("left") || IsTapInRect(leftScreenSide, touchPoints);
    }

    public bool IsObjectInRight(GameObject obj)
    {
        bool ret = false;

        if (rightScreenSide.Contains(obj.transform.localPosition))
        {
            ret = true;
        }

        return ret;
    }

    public bool IsObjectInLeft(GameObject obj)
    {
        bool ret = false;

        if (leftScreenSide.Contains(obj.transform.localPosition))
        {
            ret = true;
        }

        return ret;
    }

    private bool IsTapInRect(Rect rect, Touch[] touchPoints) 
    {
        bool ret = false;

        if (touchPoints.Length == 1 && touchPoints[0].phase == TouchPhase.Began)
        {
            //foreach (Touch point in touchPoints)
            //{
                if(rect.Contains(Camera.main.ScreenToWorldPoint(touchPoints[0].position))) {
                    ret = true;
               //     break;
                }
            //}
        }

        return ret;
    }


    public static GameService GetInstance()
    {
        return instance;
    }

    public void StartNewGame()
    {
        numOfLives = 3;
        SetGameDiffuclty(1);
        SetGameSpeed(5f);
        levelUpMultiplyer = 5;

        isGameover = false;
        isMenu = false;
        isCredits = false;

        SetIsPaused(false);
        IsAlive(true);

        toggleMenu(false);
        EnemiesController.GetInstance().InitNewGame();
        ammo.InitAmmo(20);
        numOfKills = 0;
    }

    private void toggleMenu(bool toggle)
    {
        if (isMenu)
        {
            menuScreen.SetActive(toggle);
            gameoverScreen.SetActive(false);
            creditsScreen.SetActive(false);
        }
        else if (isGameover)
        {
            gameoverScreen.SetActive(toggle);
        }
        else if (isCredits)
        {
            creditsScreen.SetActive(toggle);
            gameoverScreen.SetActive(false);
            menuScreen.SetActive(false);
        }
        else if(GetIsPaused())
        {
            pauseScreen.SetActive(true);
            pauseButton.SetActive(false);
        }
        else
        {
            menuScreen.SetActive(toggle);
            creditsScreen.SetActive(toggle);
            gameoverScreen.SetActive(toggle);
            pauseScreen.SetActive(false);
            pauseButton.SetActive(true);
        }

        player.SetActive(!toggle);
    }

    public bool IsPlaying()
    {
        bool ret = IsAlive() && !GetIsPaused() && !isGameover;

        return ret;
    }

    public void IsAlive(bool status)
    {
        if (!status)
        {
            isGameover = true;
            toggleMenu(true);
        }

        isAlive = status;
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public float GetGameSpeed()
    {
        int multiplyer = gameDiffuclty;

        if (gameDiffuclty > 20)
        {
            multiplyer = 20;
        }

        return gameSpeed + gameSpeed * multiplyer* 0.05f;
    }

    public float GetSceneSpeed()
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

    public int GetGameDiffuclty()
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

        toggleMenu(state);
    }

    public void ShowCredits()
    {
        isCredits = true;
        isMenu = false;
        isGameover = false;

        toggleMenu(true);
    }
}
