using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;


[ExecuteInEditMode]
[System.Serializable]
public class GridScript : MonoBehaviour {
    //tile
    public GameObject tilePrefab;
    //grid dimensions
    public int gridX;
    public int gridY;
    //tile spacing
    public float distBetweenTiles;
    //2D array that stores active tiles
    public GameObject[,] tileArray;
    
    //Must be public to work correctly
    public int loadcount = 0;

    // Use this for initialization
    void Awake()
    {
        if (loadcount == 0)
        {
            Setup();
        }
        loadcount++;
    }

    private void Setup()
    {

        //Default Sand Tag
        tilePrefab.tag = "Sand";

        //instantiate tile game objects and put them into array
        tileArray = CreateGrid(gridX, gridY, distBetweenTiles);

        //connectTiles
        ConnectTiles(tileArray);
    }

    GameObject[,] CreateGrid(int x, int y, float distBetweenTiles)
    {

        GameObject[,] gridArray = new GameObject[x, y];

        int gridX = gridArray.GetLength(0);
        int gridY = gridArray.GetLength(1);

        //float cornerCenterX = 0.0f;
        //float cornerCenterY = 0.0f;
        float xOffset = 0.0f;
        float yOffset = 0.0f;

        for (int i = 0; i < gridY; i++)
        {
            xOffset = 0.0f;
            for (int j = 0; j < gridX; j++)
            {
                GameObject newTile = Instantiate(tilePrefab, new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z), transform.rotation) as GameObject;
                newTile.transform.parent = gameObject.transform;
                gridArray[j, i] = newTile;

                xOffset += distBetweenTiles;
            }
            yOffset -= distBetweenTiles;
        }

        return gridArray;

    }

    void ConnectTiles(GameObject[,] gridArray)
    {
        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                GameObject tile = gridArray[i, j];

                int up = i - 1;
                int down = i + 1;
                int left = j - 1;
                int right = j + 1;

                //Debug.Log (up);
                //Debug.Log (down);
                //Debug.Log (left);
                //Debug.Log (right);

                if (up >= 0)
                {
                    tile.GetComponent<TileScript>().SetUp(gridArray[up, j]);
                }
                if (down < gridArray.GetLength(0))
                {
                    tile.GetComponent<TileScript>().SetDown(gridArray[down, j]);
                }
                if (left >= 0)
                {
                    tile.GetComponent<TileScript>().SetLeft(gridArray[i, left]);
                }
                if (right < gridArray.GetLength(1))
                {
                    tile.GetComponent<TileScript>().SetRight(gridArray[i, right]);
                }

                gridArray[i, j] = tile;
            }
        }



    }
}
