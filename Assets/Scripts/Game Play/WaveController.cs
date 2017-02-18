using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour {
    private static WaveController _instance;
    private int _wave;

    public static WaveController Instance { get; private set; }
    public int Wave { get; set; }
    
    public WaveController()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void Init()
    {
        Wave = 1;
    }

    public void InitNextWave(int numOfEnemies, int extraAmmo, float gameSpeed, int gameDiffuclty)
    {

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
