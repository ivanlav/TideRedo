using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
        WaitForTen();
        Application.Quit();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Cancel"))
        {
            Application.Quit();
        }
	}

    IEnumerator WaitForTen()
    {
        yield return new WaitForSeconds(10);
        
    }

}
