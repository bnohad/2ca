using UnityEngine;
using System.Collections;

public class SceneMovement : MonoBehaviour {
    private static SceneMovement instance;

    private Vector2 sceneStartingPos;
    private Rigidbody2D rb2d;
    //private const float movingSpeed = 10f;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        InitInstance();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void InitInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static SceneMovement GetInstance()
    {
        return instance;
    }

    public void SetSpeed(float speed)
    {
        rb2d.velocity = new Vector2(0, -1 * speed);
    }
}
