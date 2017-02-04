using UnityEngine;
using System.Collections;

public class GunsScript : MonoBehaviour {

    private Animator animator;
    public bool isRight;
    public AudioSource gunShot;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        gunShot = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (GameService.GetInstance().IsAlive() && !GameService.GetInstance().GetIsPaused())
        {
            if (isRight && GameService.GetInstance().IsTapRight(Input.touches))
            {
                animator.SetTrigger("isFired");
                gunShot.Play();
                
            }
            else if (!isRight && GameService.GetInstance().IsTapLeft(Input.touches))
            {
                animator.SetTrigger("isFired");
                gunShot.Play();
            }
        }
	}
}
