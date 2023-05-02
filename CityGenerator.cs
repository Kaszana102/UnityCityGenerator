using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;




[ExecuteInEditMode]
public class CityGenerator : EditorWindow
{
    protected int citySize=5;
    protected int MINSIZE=1,MAXSIZE=10;
    protected float cityScale = 1f;
    Tile[,] tiles;


    /// <summary>
    /// liczone na koordynatach x,y!
    /// </summary>
    Road[,] horizontalRoads;
    /// <summary>
    /// liczone na koordynatach x,y!
    /// </summary>
    Road[,] verticalRoads;

    CrossRoad[,] crossRoads;

    GameObject City;
    NavMeshSurface navMeshSurface;
    protected float NoiseScale=1;
    protected float NoiseOffset=0;

    [MenuItem("Custom/Generate City %g")]
    public static void OpenWindow()
    {
        GetWindow<CityGenerator>();
    }

    void OnEnable()
    {
        
    }    

    void OnGUI()
    {        

        if (GUILayout.Button("Build City"))
        {
            BuildCity();
        }


        if (GUILayout.Button("test"))
        {
            // do the interesting thing.
        }

        SeedInput();


        CitySizeSlider();
        CityScaleInput();

    }

    private void CityScaleInput()
    {
        cityScale = EditorGUILayout.FloatField("CityScale", cityScale);        
    }

    void SeedInput()
    {

        NoiseScale = EditorGUILayout.FloatField("NoiseScale", NoiseScale);
        NoiseOffset = EditorGUILayout.FloatField("NoiseOffset", NoiseOffset);
        
    }


    void CitySizeSlider()
    {

        Rect position = EditorGUILayout.GetControlRect(false, 2 * EditorGUIUtility.singleLineHeight); // Get two lines for the control
        position.height *= 0.5f;
        citySize = EditorGUI.IntSlider(position, "City Size", citySize, MINSIZE, MAXSIZE);
        // Set the rect for the sub-labels
        position.y += position.height;
        position.x += EditorGUIUtility.labelWidth;
        position.width -= EditorGUIUtility.labelWidth + 54; //54 seems to be the width of the slider's float field
                                                            //sub-labels
        GUIStyle style = GUI.skin.label;
        style.alignment = TextAnchor.UpperLeft; EditorGUI.LabelField(position, "min", style);
        style.alignment = TextAnchor.UpperRight; EditorGUI.LabelField(position, "max", style);

    }



    void NewTileMatrix()
    {
        if (tiles != null)
        {
            foreach (var tile in tiles)
            {
                if (tile != null)
                {
                    if (tile.gameObject != null)
                    {
                        DestroyImmediate(tile.gameObject);
                        DestroyImmediate(tile);
                    }
                }
            }
        }


        tiles = new Tile[citySize,citySize];
    }

    void NewRoadMatrices()
    {
        if (verticalRoads != null)
        {
            foreach (var road in verticalRoads)
            {
                if (road != null)
                {
                    if (road.gameObject != null)
                    {
                        DestroyImmediate(road.gameObject);
                        DestroyImmediate(road);
                    }
                }
            }
        }
        verticalRoads = new Road[citySize +1 , citySize];


        if (horizontalRoads != null)
        {
            foreach (var road in horizontalRoads)
            {
                if (road != null)
                {
                    if (road.gameObject != null)
                    {
                        DestroyImmediate(road.gameObject);
                        DestroyImmediate(road);
                    }
                }
            }
        }
        horizontalRoads = new Road[citySize, citySize +1];

    }


    private void NewCrossroadMatrix()
    {
        if (crossRoads != null)
        {
            foreach (var tile in tiles)
            {
                if (tile != null)
                {
                    if (tile.gameObject != null)
                    {
                        DestroyImmediate(tile.gameObject);
                        DestroyImmediate(tile);
                    }
                }
            }
        }


        crossRoads = new CrossRoad[citySize + 1, citySize + 1];
    }



    bool MissingAnything()
    {
        //check if city size has changed
        if (tiles == null || tiles[0, 0] == null || tiles.Length != citySize * citySize)
        {
            return true;
        }
        else
        {
            //check tiles            
            foreach (Tile tile in tiles)
            {
                if (tile != null && tile.gameObject == null)
                {
                    return true;
                }
            }
           


            //check roads
            if (verticalRoads != null)
            {
                foreach (Road road in verticalRoads)
                {
                    if (road != null && road.gameObject == null)
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }


            if (horizontalRoads != null)
            {
                foreach (Road road in horizontalRoads)
                {
                    if (road != null && road.gameObject == null)
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }





            //check crossroads            
            foreach (CrossRoad crossRoad in crossRoads)
            {
                if (crossRoad != null && crossRoad.gameObject == null)
                {
                    return true;
                }
            }
        }
        return false;
    }



    /// <summary>
    /// x and y are in range from 0 to CitySize-1
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    float GenerateNoise(int x, int y)
    {
        float i, j;

        i = x;
        j = y;

        i /= citySize;
        j /= citySize;


        i = (i + NoiseOffset) * NoiseScale;
        j = (j + NoiseOffset) * NoiseScale;

        return Mathf.PerlinNoise(i, j);
    }


    void BuildTiles()
    {
        for (int i = 0; i < citySize; i++)
        {
            for (int j = 0; j < citySize; j++)
            {
                float textureVal = GenerateNoise(i, j);

                if (tiles[i, j] == null)
                {
                    tiles[i, j] = Tile.CreateTile(Tile.FloatToType(textureVal));                    
                    tiles[i, j].transform.parent = City.transform;
                    tiles[i, j].transform.position = new Vector3(i, 0, j);                    
                }
                else
                {
                    if (tiles[i, j].GetTileType() != Tile.FloatToType(textureVal))
                    {
                        DestroyImmediate(tiles[i, j].gameObject);
                        
                        //update to new tile
                        tiles[i, j] = Tile.CreateTile(Tile.FloatToType(textureVal));     //tu może być błąd!                   
                        tiles[i, j].transform.parent = City.transform;
                        tiles[i, j].transform.position = new Vector3(i, 0, j);                        
                    }
                }
            }
        }
    }


    Tile GetMaybeNullTile(int x, int y)
    {
        return (x < 0 || y < 0 || x>= citySize || y>=citySize) ? null : tiles[x, y];
    }


    Road GetMaybeNullRoad(bool vertical, int x, int y)
    {
        if (vertical)
        {
            return (x < 0 || y < 0 || x >= citySize + 1 || y >= citySize ) ? null : verticalRoads[x, y];
        }
        else
        {
            return (x < 0 || y < 0 || x >= citySize  || y >= citySize + 1) ? null : horizontalRoads[x, y];
        }        
    }

    void BuildRoads()
    {        
        //first vertical roads
        for (int i = 0; i < citySize+1; i++)
        {
            for (int j = 0; j < citySize; j++)
            {                
                if (verticalRoads[i, j] == null)
                {
                    verticalRoads[i, j] = Road.CreateRoad(GetMaybeNullTile(i-1,j), GetMaybeNullTile(i,j),true);
                    verticalRoads[i, j].transform.position = new Vector3(i - 0.5f, 0, j); ;
                    verticalRoads[i, j].transform.parent = City.transform;
                }
                else
                {
                    if (verticalRoads[i, j].GetRoadType() != Road.CalucalteRoadType(GetMaybeNullTile(i - 1, j), GetMaybeNullTile(i, j)))
                    {
                        DestroyImmediate(verticalRoads[i, j].gameObject);

                        //update to new tile
                        verticalRoads[i, j] = Road.CreateRoad(GetMaybeNullTile(i - 1, j), GetMaybeNullTile(i, j),true);
                        verticalRoads[i, j].transform.position = new Vector3(i - 0.5f, 0, j);
                        verticalRoads[i, j].transform.parent = City.transform;
                    }
                }
                
            }
        }



        //then horizontal roads
        for (int i = 0; i < citySize; i++)
        {
            for (int j = 0; j < citySize+1; j++)
            {                
                if (horizontalRoads[i, j] == null)
                {
                    horizontalRoads[i, j] = Road.CreateRoad(GetMaybeNullTile(i, j), GetMaybeNullTile(i, j-1),false);
                    horizontalRoads[i, j].transform.position = new Vector3(i, 0, j - 0.5f); ;
                    horizontalRoads[i, j].transform.parent = City.transform;
                }
                else
                {
                    if (horizontalRoads[i, j].GetRoadType() != Road.CalucalteRoadType(GetMaybeNullTile(i, j), GetMaybeNullTile(i, j-1)))
                    {
                        DestroyImmediate(horizontalRoads[i, j].gameObject);

                        //update to new tile
                        horizontalRoads[i, j] = Road.CreateRoad(GetMaybeNullTile(i, j), GetMaybeNullTile(i, j-1),false);
                        horizontalRoads[i, j].transform.position = new Vector3(i, 0, j - 0.5f);
                        horizontalRoads[i, j].transform.parent = City.transform;
                    }
                }

            }
        }
    }


    private void ConnectAllRoads()
    {
        for(int i=0; i < citySize+1; i++)
        {
            for (int j = 0; j < citySize + 1; j++) 
            {
                List<Road> roads = new List<Road>();
                //left road exists
                if (i - 1 >= 0)
                {
                    roads.Add(horizontalRoads[i - 1,j]);
                }


                //right road exists
                if (i < citySize)
                {                    
                    roads.Add(horizontalRoads[i, j]);
                }

                //upper road exists
                if (j < citySize)
                {
                    roads.Add(verticalRoads[i, j]);
                }


                //lower road exists
                if (j - 1 >= 0)
                {
                    roads.Add(verticalRoads[i, j-1]);
                }

                crossRoads[i, j].ConnectToRoads(roads);
            }
        }
    }



    private void BuildCrossroads()
    {
        for (int i = 0; i < citySize + 1; i++)
        {
            for (int j = 0; j < citySize + 1; j++)
            {
                if (crossRoads[i, j] == null)
                {
                    crossRoads[i, j] = CrossRoad.CreateCrossRoad(
                        GetMaybeNullTile(i, j),
                        GetMaybeNullTile(i, j-1),
                        GetMaybeNullTile(i-1, j-1),
                        GetMaybeNullTile(i-1, j)
                        );
                    crossRoads[i, j].transform.position = new Vector3(i -0.5f, 0, j-0.5f); ;
                    crossRoads[i, j].transform.parent = City.transform;
                }
                else
                {                    
                    if (crossRoads[i, j].GetCrossRoadType() != CrossRoad.CalucalteCrossRoadType(
                         GetMaybeNullTile(i, j),
                         GetMaybeNullTile(i, j - 1),
                         GetMaybeNullTile(i - 1, j - 1),
                         GetMaybeNullTile(i - 1, j)
                        ))
                    {
                        DestroyImmediate(crossRoads[i, j].gameObject);

                        //update to new crossRoad
                        crossRoads[i, j] = CrossRoad.CreateCrossRoad(
                        GetMaybeNullTile(i, j),
                        GetMaybeNullTile(i, j - 1),
                        GetMaybeNullTile(i - 1, j - 1),
                        GetMaybeNullTile(i - 1, j)
                        );
                        crossRoads[i, j].transform.position = new Vector3(i - 0.5f, 0, j - 0.5f); ;
                        crossRoads[i, j].transform.parent = City.transform;
                    }
                }
            }
        }
    }
 
    // UWAZAJ BO NIE WCZYTUJE PRZY PONOWNYM OTWARCIU PROJEKTU!!!!
    void BuildCity()
    {
        if( City == null || City.gameObject != null) //check if City exists
        {
            if(GameObject.Find("City") == null)
            {
                City = new GameObject("City");
                
            }
            else
            {
                City = GameObject.Find("City").gameObject;                
            }
            City.transform.position = Vector3.zero;

        }
        
        if (MissingAnything()) //check if any tile was deleted
        {            
            NewTileMatrix();
            NewRoadMatrices();
            NewCrossroadMatrix();
        }

        if(navMeshSurface == null)
        {            
            City.AddComponent<NavMeshSurface>();
            navMeshSurface = City.GetComponent<NavMeshSurface>();
        }


        City.transform.localScale = Vector3.one;
        BuildTiles();
        BuildRoads();
        BuildCrossroads();
        ConnectAllRoads();


        City.transform.localScale = Vector3.one * cityScale;        
        navMeshSurface.BuildNavMesh();
    }
   
}
