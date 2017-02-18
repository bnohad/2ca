using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController {
    //public const int ENEMIES_COUNT_INIT = 10;

    private static WaveController _instance;
    private bool _finished;
    private int _wave;

    private EnemiesController enemiesCtrl;
    private AmmoController ammoCtrl;
    private GameService gameService;

    public bool Ongoing { get; private set; }
    public bool Finished
    {
        get
        {
            return _finished;
        }
        set
        {
            _finished = value;
        }
    }
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

        if(numOfEnemies % 7 == 0)
        {
            enemiesCtrl.ParalelEnemis += 1;
        }
        
    }

    public void StartWave()
    {
        Ongoing = true;
        Finished = false;
        Debug.Log(string.Format("Started Wave: {0}", Wave));
    }

    public void NextWave()
    {
        Wave++;


    }

    public bool WasFinished()
    {

        Finished = enemiesCtrl.WaveFinished();
        
        return Finished;
    }
}
