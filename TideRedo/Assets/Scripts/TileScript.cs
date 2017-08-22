using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[System.Serializable]
public class TileScript : MonoBehaviour {

    public Sprite[] spriteArray;
    private Sprite activeSprite;

    public int initialSize;

    //pointers to surrounding tiles
    public GameObject upTile = null;
    public GameObject downTile = null;
    public GameObject leftTile = null;
    public GameObject rightTile = null;


    private void Start()
    {
        SetSprite();    
    }
    
    void Update()
    {
        if (!Application.isPlaying) { 
            SetSprite();
        }      

    }

    //Set Sprite Image to Tag
    public void SetSprite()
    {

        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

        if (this.tag == "Dirt")
        {
            activeSprite = Resources.Load<Sprite>("DirtArt");
        }
        else if (this.tag == "Pebble")
        {
            activeSprite = Resources.Load<Sprite>("PebbleArt");
        }
        else if(this.tag == "Sand")
        {
            //Invisible in Play, visibile in Editor

            activeSprite = Resources.Load<Sprite>("DirtArt");
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .1f);

            if (Application.isPlaying)
            {
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
            }

        }else if(this.tag == "Undig")
        {
            //Undiggable invisible sand 
            activeSprite = Resources.Load<Sprite>("DirtArt");
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        }

        GetComponent<SpriteRenderer>().sprite = activeSprite;

        if (activeSprite != null)
        {
            ScaleSprite(initialSize);
        }

    }

    //Scales Sprite & Box Collider to same size
    public void ScaleSprite(float initialSize)
    {
        Vector2 spriteSize = activeSprite.bounds.size;
        GetComponent<BoxCollider2D>().size = GetComponent<SpriteRenderer>().sprite.bounds.size;
        transform.localScale = new Vector3(initialSize / spriteSize.x, initialSize / spriteSize.y);
        //Debug.Log(transform.localScale);
    }

    //sets pointers of surrounding tiles
    public void SetUp(GameObject x)
    {
        upTile = x;
    }
    public void SetDown(GameObject x)
    {
        downTile = x;
    }
    public void SetLeft(GameObject x)
    {
        leftTile = x;
    }
    public void SetRight(GameObject x)
    {
        rightTile = x;
    }

    
}
