using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerLevelOne : MonoBehaviour {

    public bool getStar = false;
    private GameObject wave;

	// Use this for initialization
	void Start () {
        wave = GameObject.FindGameObjectWithTag("Wave");
       
	}
	
	// Update is called once per frame
	void Update () {
        if (getStar)
        {
            wave.GetComponent<WaveScript>().maxHeight = wave.GetComponent<WaveScript>().starHeight;
        }
	}

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Player")
        {
            LoadLevel("lvl2");
        }
    }

    private void LoadLevel(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
