using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    
    private Enemy instance;
    private Animator animator;
    private BoxCollider2D bc2d;
    private Rigidbody2D rb2d;

    private float speed;

    public bool isAlive;
    public bool hasEnteredView;

	// Use this for initialization
	void Start () {
        InitEnemy();
        SetSpeed(3f);
	}
	
	// Update is called once per frame
	void Update () {
        if (IsAlive() && hasEnteredView)
        {
            if (GameService.GetInstance().IsObjectInTapArea(transform.localPosition, Input.touches))
            {
                Debug.Log("Enemy Dead");
                IsAlive(false);
                animator.SetTrigger("isDead");
            }
        }
        else if (!GameService.GetInstance().IsObjectInWorldView(gameObject) && hasEnteredView)
        {
            Destroy(gameObject);
        }
        else if (!hasEnteredView && GameService.GetInstance().IsObjectInWorldView(gameObject))
        {
            hasEnteredView = true;
        }

        if (speed > 3f)
        {
            speed = GameService.GetInstance().GetGameSpeed();
            SetSpeed(speed);

            Debug.Log(string.Format("NEW SPEED {0}", speed));
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
        rb2d.velocity = new Vector2(0, -1 * speed);
    }

}
