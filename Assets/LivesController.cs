﻿using UnityEngine;
using System.Collections;

public class LivesController : MonoBehaviour {

    private GameObject[] lives;
    private int lastNumOfLives;
	// Use this for initialization
	void Start () {
        lives = new GameObject[3];

        lives[0] = GameObject.Find("life1");
        lives[1] = GameObject.Find("life2");
        lives[2] = GameObject.Find("life3");

        updateLivesIndicator(3);
	}
	
	// Update is called once per frame
	void Update () {
        if (GameService.GetInstance().GetNumOfLives() != lastNumOfLives)
        {
            lastNumOfLives = GameService.GetInstance().GetNumOfLives();

            updateLivesIndicator(lastNumOfLives);
        }        
	}

    private void updateLivesIndicator(int newValue)
    {
        for (int i = 0; i < lives.Length; i++)
        {
            if (i < newValue)
            {
                lives[i].SetActive(true);
            }
            else
            {
                Animator currAnimator = lives[i].GetComponent<Animator>();
                currAnimator.SetTrigger("isLost");

                StartCoroutine(wait(lives[i]));

                //lives[i].SetActive(false);
            }
        }
    }

    IEnumerator wait(GameObject obj)
    {
        print(Time.time);
        yield return new WaitForSeconds(0.8f);
        obj.SetActive(false);
        print(Time.time);
    }

}
