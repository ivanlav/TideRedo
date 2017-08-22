using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[System.Serializable]
public class TileScriptBig : MonoBehaviour {

    public GameObject[] prefabArray;
    private GameObject activePrefab= null;

    public Material[] artArray;
    private Material activeMaterial;
    private Material startMaterial = null;

    public Sprite[] spriteArray;
    private Sprite activeSprite;

    public int initialSize;

    //pointers to surrounding tiles
    public GameObject upTile = null;
    public GameObject downTile = null;
    public GameObject leftTile = null;
    public GameObject rightTile = null;

    // Use this for initialization
    void Update()
    {
        /*
             SetPrefab();
             if (activePrefab != null)
             {
                 Draw();
             }
         
        SetMaterial();
        if (activeMaterial != null)
        {
            SetMesh();
        }
        */
        
        SetSprite();


    }

    public void SetPrefab()
    {

        if (this.tag == "Dirt")
        {
            activePrefab = prefabArray[0];
        }
        if (this.tag == "Pebble")
        {
            activePrefab = prefabArray[1];
        }
    }

    public void Draw()
    {
        
        if (this.transform.childCount != 0)
        {
            GameObject childObject = this.transform.GetChild(0).gameObject;
            DestroyImmediate(childObject);
        }
        GameObject artTile = Instantiate(activePrefab, this.transform) as GameObject;
        artTile.transform.parent = this.transform;

    }

    private void SetMaterial()
    {
        //Debug.Log(this.tag);
        if(this.tag == "Dirt")
        {
            activeMaterial = artArray[0];
        }
        else if(this.tag == "Pebble")
        {
            activeMaterial = artArray[1];
        }
        else
        {
            activeMaterial = startMaterial;
        }
    }

    private void SetMesh()
    {
        gameObject.GetComponent<MeshRenderer>().material = activeMaterial;
    }

    private void SetSprite()
    {
        if (this.tag == "Dirt")
        {
            activeSprite = spriteArray[0];
        }
        else if (this.tag == "Pebble")
        {
            activeSprite = spriteArray[1];
        }

        if (activeSprite != null) { 
        GetComponent<SpriteRenderer>().sprite = activeSprite;
        Vector2 spriteSize = GetComponent<SpriteRenderer>().size;

        GetComponent<BoxCollider2D>().size = GetComponent<SpriteRenderer>().bounds.size;
        transform.localScale = new Vector3(initialSize / spriteSize.x, initialSize/ spriteSize.y);
    }

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
