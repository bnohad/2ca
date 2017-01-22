using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private GameService service;
    private Animator animator;
	// Use this for initialization
	void Start () {
        service = GameService.GetInstance();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if (service.IsAlive())
        {
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
        else
        {
            animator.SetTrigger("isDead");
        }
        
	}
}
