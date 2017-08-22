using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveScript : MonoBehaviour
{

    public float topWait;
    public float bottomWait;
    //public float waveWait;
    public float waveSpeed;
    public float maxHeight;
    public float minHeight;

    public float starHeight;

    // Use this for initialization
    void Start()
    {
        GameObject grid = GameObject.FindGameObjectWithTag("Grid");
        GameObject firstTile = grid.transform.GetChild(0).gameObject;
        maxHeight = firstTile.GetComponent<BoxCollider2D>().bounds.max.y;

        minHeight = GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>().bounds.min.y;

        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(bottomWait);

        // Debug.Log(transform.position.y);

        

        //Wave moves up and down forever
        while (true)
        {

            while (GetComponent<EdgeCollider2D>().bounds.center.y < maxHeight)
            {
               
                //      Debug.Log(transform.position.y);
                GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, waveSpeed);
                yield return new WaitForEndOfFrame();
            }
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);

            yield return new WaitForSeconds(topWait);

            while (GetComponent<EdgeCollider2D>().bounds.center.y > minHeight)
            {
                //Debug.Log(GetComponent<EdgeCollider2D>().bounds.center.y);
                //Debug.Log(minHeight);

                GetComponent<EdgeCollider2D>().isTrigger = true;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -waveSpeed);
                yield return new WaitForEndOfFrame();

            }
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
            yield return new WaitForSeconds(bottomWait);
            GetComponent<EdgeCollider2D>().isTrigger = false;
        }
    }

}