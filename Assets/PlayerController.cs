using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Touch[] touchPoints = Input.touches;

        if (GameService.GetInstance().IsTapRight(touchPoints) && GameService.GetInstance().IsTapLeft(touchPoints))
        {
            Debug.Log("TOUCHED BOTH SIDES");
        }
        else if (GameService.GetInstance().IsTapRight(touchPoints))
        {
            Debug.Log("TOUCHED RIGHT SIDE");
        }
        else if (GameService.GetInstance().IsTapLeft(touchPoints))
        {
            Debug.Log("TOUCHED LEFT SIDE");

        }
        else
        {

        }
	}
}
