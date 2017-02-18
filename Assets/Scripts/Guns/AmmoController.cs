using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour {
    public int ammo;
    private static AmmoController instance;
    
    public AmmoController()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

	// Use this for initialization
	void Start () {
        //InitAmmo(0);
	}

    public static AmmoController GetInstance()
    {
        return instance;
    }

    public void InitAmmo(int bullets)
    {
        if(bullets < 0)
        {
            bullets = 0;
        }

        ammo = bullets;
    }

    public int GetAmmoCount()
    {
        return ammo;
    }

    public bool HasAmmo()
    {
        return ammo > 0;
    }

    public void DecrementAmmo()
    {
        if (ammo > 0)
        {
            ammo -= 1;
        }
    }
	
    public void AddBullets(int bullets)
    {
        ammo += bullets;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
