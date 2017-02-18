using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController {
    //public const int ENEMIES_COUNT_INIT = 10;

    private static WaveController _instance;
    private int _wave;

    private EnemiesController enemiesCtrl;
    private AmmoController ammoCtrl;
    private GameService gameService;

    public bool Ongoing { get; private set; }
    public int Wave { get; set; }
    
    public WaveController()
    {
        if(_instance == null)
        {
            _instance = this;

            enemiesCtrl = EnemiesController.GetInstance();
            ammoCtrl = AmmoController.GetInstance();
            gameService = GameService.GetInstance();
        }
    }

    public static WaveController GetInstance()
    {
        return _instance;
    }

    public void Init()
    {
        Wave = 1;
    }

    public void InitNextWave(int numOfEnemies, int extraAmmo, float gameSpeed, int gameDiffuclty)
    {
        Ongoing = false;

        enemiesCtrl.SpawnCount = numOfEnemies;
        ammoCtrl.AddBullets(extraAmmo);
    }

    public void StartWave()
    {
        Ongoing = true;
        Debug.Log(string.Format("Started Wave: {0}", Wave));
    }

    public void NextWave()
    {
        Wave++;


    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
