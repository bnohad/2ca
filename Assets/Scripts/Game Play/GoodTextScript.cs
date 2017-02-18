using UnityEngine;
using System.Collections;

public class GoodTextScript : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(wait());
	}

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.8f);
        gameObject.SetActive(false);
    }
}
