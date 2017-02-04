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

        if (service.IsAlive() && !service.GetIsPaused())
        {
            Touch[] touchPoints = Input.touches;

            if(animator.speed < 1)
            {
                animator.speed = 1;
            }

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
        else if(service.IsAlive() && service.GetIsPaused())
        {
            animator.speed = 0;
        }
        else
        {
            animator.SetTrigger("isDead");
        }
        
	}
}
