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
        if (GameService.GetInstance().IsAlive())
        {
            ApplyGameSpeedToEnemy();

            if (IsAlive() && hasEnteredView && !GameService.GetInstance().IsObjectInWorldView(gameObject))
            {
                HitPlayer();
            }
            else if (!hasEnteredView && GameService.GetInstance().IsObjectInWorldView(gameObject))
            {
                hasEnteredView = true;
            }
            else if (!IsAlive() && hasEnteredView && !GameService.GetInstance().IsObjectInWorldView(gameObject))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            //player dead
            if (IsAlive())
            {
                animator.speed = 0.2f;
                wasSpeedSet = false;
                SetSpeed(0.2f);
            }
            else
            {
                //player & enemy dead
                //animator.speed = 0;
                wasSpeedSet = false;
                SetSpeed(0);
            }
        }
        //    if (IsAlive() && hasEnteredView)
        //    {
        //        if (GameService.GetInstance().IsObjectInTapArea(gameObject, Input.touches))
        //        {
        //            if (EnemiesController.GetInstance().IsNearestEnemy(gameObject))
        //            {
        //                if (!EnemiesController.GetInstance().DidLastShotHit(gameObject))
        //                {
        //                    EnemiesController.GetInstance().SetLastShotHit(gameObject, true);
        //                    IsAlive(false);
        //                    wasSpeedSet = false;
        //                    SetSpeed(GameService.GetInstance().GetSceneSpeed());
        //                    GameService.GetInstance().addToPoints(1);
        //                    Debug.Log("Enemy Dead");
        //                    animator.SetTrigger("isDead");
        //                    EnemiesController.GetInstance().SetLastShotHit(gameObject, false);
        //                }
        //            }
        //        }
        //        else if (!GameService.GetInstance().IsObjectInWorldView(gameObject))
        //        {
        //            Destroy(gameObject);
        //            IsAlive(false);
        //            GameService.GetInstance().takeHit();
        //            Debug.Log("TOOK A HIT!");
        //        }
        //    }
        //    else if (!hasEnteredView && GameService.GetInstance().IsObjectInWorldView(gameObject))
        //    {
        //        Debug.Log("IN VIEW");
        //        hasEnteredView = true;
        //    }
        //    else if (!GameService.GetInstance().IsObjectInWorldView(gameObject) && !IsAlive())
        //    {
        //        Destroy(gameObject);
        //    }


        //    if (speed < GameService.GetInstance().GetGameSpeed() && isAlive)
        //    {
        //        wasSpeedSet = false;
        //        speed = GameService.GetInstance().GetGameSpeed();
        //        SetSpeed(speed);
        //        Debug.Log(string.Format("NEW SPEED {0}", speed));
        //    }
        //}
        //else
        //{
        //    //player dead
        //    animator.speed = 0.2f;
        //    wasSpeedSet = false;
        //    SetSpeed(0.2f);
        //}
    }

    public void Kill()
    {
        IsAlive(false);
        wasSpeedSet = false;
        SetSpeed(GameService.GetInstance().GetSceneSpeed());
        //GameService.GetInstance().addToPoints(1);
        Debug.Log("Enemy Dead");
        animator.SetTrigger("isDead");
        GetComponent<SpriteRenderer>().sortingOrder = 0;
    }

    public void HitPlayer()
    {
        GameService.GetInstance().takeHit();
        IsAlive(false);
        Destroy(gameObject);
    }

    public void ApplyGameSpeedToEnemy()
    {
        if (speed < GameService.GetInstance().GetGameSpeed() && isAlive)
        {
            wasSpeedSet = false;
            speed = GameService.GetInstance().GetGameSpeed();
            SetSpeed(speed);
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
