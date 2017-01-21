using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    
    private Enemy instance;
    private Animator animator;
    private BoxCollider2D bc2d;
    private Rigidbody2D rb2d;

    private float speed;

    public bool isAlive;
    private bool wasSpeedSet;
    public bool hasEnteredView;

	// Use this for initialization
	void Start () {
        InitEnemy();
        SetSpeed(3f);
	}
	
	// Update is called once per frame
	void Update () {
        if(GameService.GetInstance().IsAlive()) {
            if (IsAlive() && hasEnteredView)
            {
                if (GameService.GetInstance().IsObjectInTapArea(transform.localPosition, Input.touches))
                {
                    GameService.GetInstance().addToPoints(1);
                    Debug.Log("Enemy Dead");
                    IsAlive(false);
                    animator.SetTrigger("isDead");
                }
                else if (!GameService.GetInstance().IsObjectInWorldView(gameObject))
                {
                    Destroy(gameObject);
                    IsAlive(false);
                    GameService.GetInstance().takeHit();
                    Debug.Log("TOOK A HIT!");
                }
            }
            else if (!hasEnteredView && GameService.GetInstance().IsObjectInWorldView(gameObject))
            {
                Debug.Log("IN VIEW");
                hasEnteredView = true;
            }


            if (speed < GameService.GetInstance().GetGameSpeed())
            {
                wasSpeedSet = false;
                speed = GameService.GetInstance().GetGameSpeed() + 1f;
                SetSpeed(speed);
                Debug.Log(string.Format("NEW SPEED {0}", speed));
            }
        }
        else
        {
            //player dead
            animator.speed = 0.2f;
            wasSpeedSet = false;
            SetSpeed(0.2f);
        }
    }
        



    private void InitEnemy()
    {
        if (instance == null)
        {
            instance = this;
        }

        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        IsAlive(true);
        hasEnteredView = false;
    }

    public void IsAlive(bool state)
    {
        isAlive = state;
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public void SetSpeed(float speed)
    {
        if (!wasSpeedSet)
        {
            wasSpeedSet = true;
            rb2d.velocity = new Vector2(0, -1 * speed);
        }
    }

}
