using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ROADTYPE
{
    /// <summary>
    /// ->
    /// </summary>
    ONE_DIRECTIONAL_SINGLE_LANE,

    /// <summary>
    /// <br>-></br>
    /// <br>-></br>
    /// </summary>
    ONE_DIRECTIONAL_DOUBLE_LANE,

    /// <summary>
    /// <br> &lt;- </br>
    /// <br> -> </br>
    /// </summary>
    BI_DIRECTIONAL_SINGLE_LANE,

    /// <summary>
    /// <br> &lt;- </br>
    /// <br> &lt;- </br>
    /// <br> -> </br>
    /// <br> -> </br>
    /// </summary>
    BI_DIRECTIONAL_DOUBLE_LANE,

    /// <summary>
    /// <br> &lt;- </br>    
    /// <br> -> </br>
    /// <br> -> </br>
    /// </summary>
    BI_DIRECTIONAL_SINGLE_DOUBLE_LANE,

    ROADTYPESNUMBER
}



public class Road : MonoBehaviour
{

    protected ROADTYPE type;

    public List<RoadCheckPoint> starts;
    public List<RoadCheckPoint> ends;

    public Road(ROADTYPE type)
    {
        this.type = type;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public ROADTYPE GetRoadType()
    {
        return type;
    }


    private static bool IsDense(Tile tile)
    {
        if(tile.GetTileType() == TILETYPE.SKYSCRAPER)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// road type accorindly to its neighbours
    /// <br>a - left or upper tile </br>
    /// <br>b - right or lower tile</br>
    /// </summary>
    /// <param name="a"> left or upper tile</param>
    /// <param name="b"> right or lower tile</param>
    /// <returns></returns>
    public static ROADTYPE CalucalteRoadType(Tile a, Tile b)
    {

        if (a != null && b != null)
        {
            //if both dense
            if(IsDense(a) && IsDense(b))
            {
                return ROADTYPE.BI_DIRECTIONAL_DOUBLE_LANE;
            }
            //if one of them is dense
            else if (IsDense(a) || IsDense(b))
            {
                return ROADTYPE.BI_DIRECTIONAL_SINGLE_DOUBLE_LANE;
            }
            //no more cases at the moment
            else
            {
                return ROADTYPE.BI_DIRECTIONAL_SINGLE_LANE;
            }
        }
        else
        {
            if(a== null)
            {
                if (IsDense(b))
                {
                    return ROADTYPE.BI_DIRECTIONAL_DOUBLE_LANE;
                }
                else
                {
                    return ROADTYPE.BI_DIRECTIONAL_SINGLE_LANE;
                }
            }
            else
            {
                if (IsDense(a))
                {
                    return ROADTYPE.BI_DIRECTIONAL_DOUBLE_LANE;
                }
                else
                {
                    return ROADTYPE.BI_DIRECTIONAL_SINGLE_LANE;
                }
            }
        }        
    }



    /// <summary>
    /// Creates road accorindly to its neighbours
    /// <br>a - left or upper tile </br>
    /// <br>b - right or lower tile</br>
    /// </summary>
    /// <param name="a"> left or upper tile</param>
    /// <param name="b"> right or lower tile</param>
    /// <returns></returns>
    public static Road CreateRoad(Tile a, Tile b, bool vertical)
    {
        GameObject roadObject;
        Road road;
        
        switch (CalucalteRoadType(a,b))
        {
            case ROADTYPE.ONE_DIRECTIONAL_SINGLE_LANE:
                roadObject = Instantiate(Resources.Load("Roads/BI_DIRECTIONAL_SINGLE_LANE")) as GameObject;
                road = roadObject.GetComponent<Road>();
                road.type = ROADTYPE.ONE_DIRECTIONAL_SINGLE_LANE;
                break;
            case ROADTYPE.ONE_DIRECTIONAL_DOUBLE_LANE:
                roadObject = Instantiate(Resources.Load("Roads/BI_DIRECTIONAL_SINGLE_LANE")) as GameObject;
                road = roadObject.GetComponent<Road>();
                road.type = ROADTYPE.BI_DIRECTIONAL_SINGLE_LANE;
                break;
            case ROADTYPE.BI_DIRECTIONAL_SINGLE_LANE:
                roadObject = Instantiate(Resources.Load("Roads/BI_DIRECTIONAL_SINGLE_LANE")) as GameObject;
                road = roadObject.GetComponent<Road>();
                road.type = ROADTYPE.BI_DIRECTIONAL_SINGLE_LANE;
                break;
            case ROADTYPE.BI_DIRECTIONAL_DOUBLE_LANE:
                roadObject = Instantiate(Resources.Load("Roads/BI_DIRECTIONAL_DOUBLE_LANE")) as GameObject;
                road = roadObject.GetComponent<Road>();
                road.type = ROADTYPE.BI_DIRECTIONAL_DOUBLE_LANE;
                break;
            case ROADTYPE.BI_DIRECTIONAL_SINGLE_DOUBLE_LANE:
                roadObject = Instantiate(Resources.Load("Roads/BI_DIRECTIONAL_SINGLE_DOUBLE_LANE")) as GameObject;
                road = roadObject.GetComponent<Road>();
                road.type = ROADTYPE.BI_DIRECTIONAL_SINGLE_DOUBLE_LANE;
                break;
            default:
                roadObject = Instantiate(Resources.Load("Roads/BI_DIRECTIONAL_SINGLE_LANE")) as GameObject;
                road = roadObject.GetComponent<Road>();
                road.type = ROADTYPE.BI_DIRECTIONAL_SINGLE_LANE;
                break;

        }
        
        if (vertical)
        {
            if (road.type == ROADTYPE.BI_DIRECTIONAL_SINGLE_DOUBLE_LANE)
            {
                if (IsDense(a))
                {
                    roadObject.transform.rotation = Quaternion.Euler(0, 270, 0);
                }
                else
                {
                    roadObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                }
            }
            else
            {
                roadObject.transform.rotation = Quaternion.Euler(0, 90, 0);
            }

        }
        else
        {
            if (road.type == ROADTYPE.BI_DIRECTIONAL_SINGLE_DOUBLE_LANE)
            {

                if (IsDense(b))
                {
                    roadObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }            
        }
        

        return roadObject.GetComponent<Road>();
    }


    /// <summary>
    /// return nearest start to the pos point.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public RoadCheckPoint GetStart(Vector3 pos)
    {
        float minDistance = float.PositiveInfinity;
        RoadCheckPoint closest = starts[0];
        foreach (RoadCheckPoint start in starts)
        {                        
            if (Vector3.Distance(start.transform.position, pos) < minDistance)
            {
                minDistance = Vector3.Distance(pos, start.transform.position);
                closest = start;
            }                        
        }
        return closest;
    }

    /// <summary>
    /// return nearest end to the pos point.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public RoadCheckPoint GetEnd(Vector3 pos)
    {
        float minDistance = float.PositiveInfinity;
        RoadCheckPoint closest = ends[0];
        foreach (RoadCheckPoint end in ends)
        {
            if (Vector3.Distance(end.transform.position, pos) < minDistance)
            {
                minDistance = Vector3.Distance(pos, end.transform.position);
                closest = end;
            }
        }
        
        return closest;
    }

}
