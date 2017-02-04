using UnityEngine;
using System.Collections;

public class BackgroundScript : MonoBehaviour {

    private static BackgroundScript instance;

    private float backgroundHeight;
    private float backgroundHeightOffset;
    private BoxCollider2D bc2d;
    private Vector2 repositionOffset;

	// Use this for initialization
	void Start () {
        InitObjectInstance();

        bc2d = GetComponent<BoxCollider2D>();

        backgroundHeight = bc2d.size.y;
        //backgroundHeightOffset = backgroundHeight / 2;

        repositionOffset = new Vector2(0, backgroundHeight * 2f);

        //Debug.Log(string.Format("bg_h:{0}", backgroundHeight));
	}
	
	// Update is called once per frame
	void Update () {
        float bgPos = transform.position.y;
        //Debug.Log(string.Format("bg_pos:{0}", bgPos));

        if(GameService.GetInstance().IsAlive()) { 
            if (bgPos <= -backgroundHeight)
            {
                Debug.Log("POS SMALLER THAN HEIGHT");

                repositionBackground(repositionOffset);
                //transform.Translate(0,1*Time.deltaTime,0);
            }
        }
        
	}

    private void InitObjectInstance()
    {
        instance = this;
    }

    public BackgroundScript GetInstance()
    {
        return instance;
    }

    public float GetHeight()
    {
        return backgroundHeight;
    }

    public void SetYPosition(float pos)
    {
        transform.position = new Vector2(0, pos);
    }

    private void repositionBackground(Vector2 offset)
    {
        transform.position = (Vector2)transform.position + offset;
    }
}
