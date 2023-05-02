using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TILETYPE
{
    SKYSCRAPER,
    HOME,
    PARK,
    PLAIN,
    TILETYPESNUMBER
}



public class Tile : MonoBehaviour
{
    protected TILETYPE type;
    //public GameObject PARK;
    //public GameObject PLAIN;
    



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public TILETYPE GetTileType()
    {
        return type;
    }

   private static GameObject GenerateTile(string address, TILETYPE type)
    {
        GameObject tile;
        tile = Instantiate(Resources.Load(address)) as GameObject;
        tile.transform.rotation = Quaternion.identity;
        tile.AddComponent<Tile>();
        tile.GetComponent<Tile>().type = type;

        return tile;
    }

   public static Tile CreateTile(TILETYPE type)
    {
        GameObject tile;

        switch (type)
        {
            case (TILETYPE.SKYSCRAPER):
                tile=GenerateTile("Buildings/Skyscraper", type);                
                break;
            case (TILETYPE.HOME):
                tile = GenerateTile("Buildings/Home", type);                
                break;
            case (TILETYPE.PARK):
                tile = GenerateTile("Buildings/Home", type);
                break;
            case (TILETYPE.PLAIN):
                tile = GenerateTile("Buildings/Home", type);
                break;
            default:
                tile = GenerateTile("Buildings/Home", type);
                break;
        }
        
        return tile.GetComponent<Tile>();
   }


    /// <summary>
    /// input is converting float <0,1> to enum TILETYPE
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static TILETYPE FloatToType(float f)
    {
        int typesNumber = (int)TILETYPE.TILETYPESNUMBER;


        f *= typesNumber; //now f is in <0,n>        

        TILETYPE type = (TILETYPE)Mathf.FloorToInt(f);

        if (type == TILETYPE.TILETYPESNUMBER)
        {
            type--;
        }

        return type;
    }
}
