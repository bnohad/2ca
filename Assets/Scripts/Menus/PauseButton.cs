using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(GameService.GetInstance().IsPlaying() || GameService.GetInstance().GetIsPaused())
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Debug.Log("CLICKED ESC!");
                togglePause();
            }
        }
        
    }

    void OnMouseDown()
    {
        togglePause();
    }

    private void togglePause()
    {
        bool state = GameService.GetInstance().GetIsPaused();
        GameService.GetInstance().SetIsPaused(!state);
    }
}
