using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraClamp : MonoBehaviour {

    public Boundary camboundary;

    public GameObject player;
    
    // Use this for initialization
    void Start () {
        camboundary = player.GetComponent<PlayerScript>().boundary;
	}

    // Update is called once per frame
    void FixedUpdate()
    {

        //Debug.Log(GetComponent<Transform>().position.x);

        /*
        if(transform.position.x < camboundary.xMin)
        {
            transform.position = new Vector3(camboundary.xMin, transform.position.y,-1);
        }
        if (transform.position.x > camboundary.xMax)
        {
            transform.position = new Vector3(camboundary.xMax, transform.position.y, -1);
        }
        if (transform.position.y < camboundary.yMin)
        {
            transform.position = new Vector3(transform.position.x, camboundary.yMin, -1);
        }
        if (transform.position.y > camboundary.yMax)
        {
            transform.position = new Vector3(transform.position.x, camboundary.yMax,-1);
        }
        */

        
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -1);
        

    }

    void LateUpdate()
    {
        

        Vector3 v3 = transform.position;

        float yOffset = GetComponent<Camera>().orthographicSize;
        float xOffset = yOffset * GetComponent<Camera>().aspect;


        v3.x = Mathf.Clamp(v3.x, camboundary.xMin + xOffset, camboundary.xMax -xOffset);
        v3.y = Mathf.Clamp(v3.y, camboundary.yMin + yOffset, camboundary.yMax -yOffset);
        transform.position = v3;
    }
}
