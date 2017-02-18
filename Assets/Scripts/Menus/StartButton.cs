using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            Debug.Log("CLICKED ENTER!");
            StartGame();
        }
	}

    void OnMouseDown()
    {
        Debug.Log("CLICKED!");
        StartGame();
    }

    private void StartGame()
    {
        GameService.GetInstance().StartNewGame();
    }
}
