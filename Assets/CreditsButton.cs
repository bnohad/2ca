﻿using UnityEngine;
using System.Collections;

public class CreditsButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        Debug.Log("CLICKED!");
        ShowCredits();
    }

    private void ShowCredits()
    {
        GameService.GetInstance().ShowCredits();
    }
}
