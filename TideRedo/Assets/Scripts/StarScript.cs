using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Player")
        {
            GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
            gameController.GetComponent<ControllerLevelOne>().getStar = true;

            GameObject hud = GameObject.FindGameObjectWithTag("Hud");
            hud.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            Destroy(gameObject);
        }
    }
}
