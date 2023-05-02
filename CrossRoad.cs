using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CROSSROADTYPE
{
    //corners
    CORNER_BI_DIRECTIONAL_SINGLE_TAPE,
    CORNER_BI_DIRECTIONAL_DOUBLE_TAPE,


    //  T crossroads      --.--
    //                    b | a
    //
    // a,b wymagana liczba pasów od tile'a
    // TCROSSROAD_a_b
    TCROSSROAD_I_I,
    TCROSSROAD_I_II,
    TCROSSROAD_II_I,   
    TCROSSROAD_II_II,

    //full crossroads
    //             | 
    //           d | a
    //         ---   ---
    //           c | b
    //             |
    //FULLCROSSROAD_a_b_c_d
    FULLCROSSROAD_I_I_I_I,
    FULLCROSSROAD_II_I_I_I,
    FULLCROSSROAD_II_II_I_I,
    FULLCROSSROAD_II_I_II_I,
    FULLCROSSROAD_II_II_II_I,
    FULLCROSSROAD_II_II_II_II,
    CROSSROADTYPESNUMBER
}



public class CrossRoad : MonoBehaviour
{

    protected CROSSROADTYPE type;


    public List<RoadCheckPoint> starts;
    public List<RoadCheckPoint> ends;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public CROSSROADTYPE GetCrossRoadType()
    {
        return type;
    }
    
    private static int CountNulls(Tile a, Tile b, Tile c, Tile d)
    {
        int counter=0;

        if(a == null)
        {
            counter++;        
        }

        if (b == null)
        {
            counter++;
        }

        if (c == null)
        {
            counter++;
        }

        if (d == null)
        {
            counter++;
        }
        return counter;
    }

        


    private static CROSSROADTYPE CalculateFullCrossRoadType(Tile a, Tile b, Tile c, Tile d)
    {
        int flags = 0; //holds crossRoadType in binary format 0babcd

        if (a.GetTileType() == TILETYPE.SKYSCRAPER)
        {
            flags |= 0b1000;
        }

        if (b.GetTileType() == TILETYPE.SKYSCRAPER)
        {
            flags |= 0b0100;
        }

        if (c.GetTileType() == TILETYPE.SKYSCRAPER)
        {
            flags |= 0b0010;
        }

        if (d.GetTileType() == TILETYPE.SKYSCRAPER)
        {
            flags |= 0b0001;
        }



        if(flags == 0b0000)
        {
            return CROSSROADTYPE.FULLCROSSROAD_I_I_I_I;
        }


        if (flags == 0b1000 || flags == 0b0100 || flags == 0b0010 || flags == 0b0001)
        {
            return CROSSROADTYPE.FULLCROSSROAD_II_I_I_I;
        }

        if (flags == 0b1100 || flags == 0b0110 || flags == 0b0011 || flags == 0b1001)
        {
            return CROSSROADTYPE.FULLCROSSROAD_II_II_I_I;
        }

        if (flags == 0b1010 || flags == 0b0101)
        {
            return CROSSROADTYPE.FULLCROSSROAD_II_I_II_I;
        }


        if (flags == 0b1110 || flags == 0b0111 || flags == 0b1011 || flags == 0b1101)
        {
            return CROSSROADTYPE.FULLCROSSROAD_II_II_II_I;
        }

        if (flags == 0b1111)
        {
            return CROSSROADTYPE.FULLCROSSROAD_II_II_II_II;
        }

        //never reaches
        return CROSSROADTYPE.FULLCROSSROAD_II_II_II_II;
    }


    private static CROSSROADTYPE CalcalateTCrossRoadType(Tile a, Tile b, Tile c, Tile d)
    {
        int flags = 0; //represent the Tcrossroad in binary format 0b00;

        if (a == null)
        {
            if (b == null)
            {
                //a nd b is null
                if (c.GetTileType() == TILETYPE.SKYSCRAPER)
                {
                    flags |= 0b10;
                }

                if (d.GetTileType() == TILETYPE.SKYSCRAPER)
                {
                    flags |= 0b01;
                }

            }
            else
            {
                // a and d is null

                if (b.GetTileType() == TILETYPE.SKYSCRAPER)
                {
                    flags |= 0b10;
                }

                if (c.GetTileType() == TILETYPE.SKYSCRAPER)
                {
                    flags |= 0b01;
                }
            }
        }
        else
        {
            //c is null
            if (b == null)
            {
                //c and b is null

                if (d.GetTileType() == TILETYPE.SKYSCRAPER)
                {
                    flags |= 0b10;
                }

                if (a.GetTileType() == TILETYPE.SKYSCRAPER)
                {
                    flags |= 0b01;
                }
            }
            else
            {
                //c and d is null

                if (a.GetTileType() == TILETYPE.SKYSCRAPER)
                {
                    flags |= 0b10;
                }

                if (b.GetTileType() == TILETYPE.SKYSCRAPER)
                {
                    flags |= 0b01;
                }
            }
        }


        
        switch (flags)
        {
            case 0b00:
                return CROSSROADTYPE.TCROSSROAD_I_I;
            case 0b10:
                return CROSSROADTYPE.TCROSSROAD_II_I;
            case 0b01:
                return CROSSROADTYPE.TCROSSROAD_I_II;
            case 0b11:
                return CROSSROADTYPE.TCROSSROAD_II_II;
            default:
                return CROSSROADTYPE.TCROSSROAD_I_I;
        }

    }


    private static CROSSROADTYPE CalcalateCornerCrossRoadType(Tile a, Tile b, Tile c, Tile d)
    {
        if(a != null)
        {
            if(a.GetTileType() == TILETYPE.SKYSCRAPER)
            {
                return CROSSROADTYPE.CORNER_BI_DIRECTIONAL_DOUBLE_TAPE;
            }
            else
            {
                return CROSSROADTYPE.CORNER_BI_DIRECTIONAL_SINGLE_TAPE;
            }
        }


        if (b != null)
        {
            if (b.GetTileType() == TILETYPE.SKYSCRAPER)
            {
                return CROSSROADTYPE.CORNER_BI_DIRECTIONAL_DOUBLE_TAPE;
            }
            else
            {
                return CROSSROADTYPE.CORNER_BI_DIRECTIONAL_SINGLE_TAPE;
            }
        }

        if (c != null)
        {
            if (c.GetTileType() == TILETYPE.SKYSCRAPER)
            {
                return CROSSROADTYPE.CORNER_BI_DIRECTIONAL_DOUBLE_TAPE;
            }
            else
            {
                return CROSSROADTYPE.CORNER_BI_DIRECTIONAL_SINGLE_TAPE;
            }
        }
        else
        {
            //d must be null)
            if (d.GetTileType() == TILETYPE.SKYSCRAPER)
            {
                return CROSSROADTYPE.CORNER_BI_DIRECTIONAL_DOUBLE_TAPE;
            }
            else
            {
                return CROSSROADTYPE.CORNER_BI_DIRECTIONAL_SINGLE_TAPE;
            }
        }
        
    }

    public static CROSSROADTYPE CalucalteCrossRoadType(Tile a, Tile b, Tile c, Tile d)
    {
        switch (CountNulls(a, b, c, d))
        {
            case 0:
                return CalculateFullCrossRoadType(a, b, c, d);
                
            case 2:
                return CalcalateTCrossRoadType(a, b, c, d);
                
            case 3:
                return CalcalateCornerCrossRoadType(a, b, c, d);
            default:
                return CROSSROADTYPE.FULLCROSSROAD_II_II_II_II;

        }
    }


    private static CrossRoad GenerateCrossRoad(string address, CROSSROADTYPE type)
    {
        GameObject crossRoad;
        crossRoad = Instantiate(Resources.Load(address)) as GameObject;
        crossRoad.transform.rotation = Quaternion.identity;
        crossRoad.GetComponent<CrossRoad>();
        crossRoad.GetComponent<CrossRoad>().type = type;

        return crossRoad.GetComponent<CrossRoad>();
    }



    private static void RotateFullCrossRoad(CrossRoad crossRoad, Tile a, Tile b, Tile c, Tile d)
    {
        int flags = 0; //holds crossRoadType in binary format 0babcd

        if (a.GetTileType() == TILETYPE.SKYSCRAPER)
        {
            flags |= 0b1000;
        }

        if (b.GetTileType() == TILETYPE.SKYSCRAPER)
        {
            flags |= 0b0100;
        }

        if (c.GetTileType() == TILETYPE.SKYSCRAPER)
        {
            flags |= 0b0010;
        }

        if (d.GetTileType() == TILETYPE.SKYSCRAPER)
        {
            flags |= 0b0001;
        }



        if (flags == 0b0000)
        {
            //no need to rotate
        }


        //if II I I I
        if (flags == 0b1000 || flags == 0b0100 || flags == 0b0010 || flags == 0b0001)
        {
            switch (flags)
            {
                case 0b1000:
                    crossRoad.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 0b0100:
                    crossRoad.transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case 0b0010:
                    crossRoad.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case 0b0001:
                    crossRoad.transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
            }
        }

        //if II II I I
        if (flags == 0b1100 || flags == 0b0110 || flags == 0b0011 || flags == 0b1001)
        {
            switch (flags)
            {
                case 0b1100:
                    crossRoad.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 0b0110:
                    crossRoad.transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case 0b0011:
                    crossRoad.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case 0b1001:
                    crossRoad.transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
            }
        }


        //if II I II I
        if (flags == 0b1010 || flags == 0b0101)
        {
            switch (flags)
            {
                case 0b1010:
                    crossRoad.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;                
                case 0b0101:
                    crossRoad.transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
            }
        }

        //if II II II I
        if (flags == 0b1110 || flags == 0b0111 || flags == 0b1011 || flags == 0b1101)
        {
            switch (flags)
            {
                case 0b1110:
                    crossRoad.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 0b0111:
                    crossRoad.transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case 0b1011:
                    crossRoad.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case 0b1101:
                    crossRoad.transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
            }
        }

        if (flags == 0b1111)
        {
            //no need to rotate
        }
    }
    private static void RotateTCrossRoad(CrossRoad crossRoad, Tile a, Tile b, Tile c, Tile d)
    {
        if (a == null)
        {
            if (b == null)
            {
                //a nd b is null
                crossRoad.transform.rotation = Quaternion.Euler(0, 90, 0);

            }
            else
            {
                // a and d is null

                crossRoad.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            
        }
        else
        {
            //c is null
            if (b == null)
            {
                //c and b is null

                crossRoad.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                //c and d is null

                crossRoad.transform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
    }
    private static void RotateCornerCrossRoad(CrossRoad crossRoad, Tile a, Tile b, Tile c, Tile d)
    {
        if (a != null)
        {
            crossRoad.transform.rotation = Quaternion.Euler(0, -90, 0);
        }

        if (b != null)
        {
            //no rotation needed
        }

        if (c != null)
        {
           crossRoad.transform.rotation = Quaternion.Euler(0,90,0);
        }

        if (d != null)
        {
            //if d is null
            crossRoad.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }


    private static void RotateCrossRoad(CrossRoad crossRoad, Tile a, Tile b, Tile c, Tile d)
    {
        if(CROSSROADTYPE.FULLCROSSROAD_I_I_I_I <= crossRoad.type && crossRoad.type <= CROSSROADTYPE.FULLCROSSROAD_II_II_II_II)
        {
            RotateFullCrossRoad(crossRoad,a,b,c,d);
        }

        if (CROSSROADTYPE.TCROSSROAD_I_I <= crossRoad.type && crossRoad.type <= CROSSROADTYPE.TCROSSROAD_II_II)
        {
            RotateTCrossRoad(crossRoad, a, b, c, d);
        }

        if (CROSSROADTYPE.CORNER_BI_DIRECTIONAL_SINGLE_TAPE <= crossRoad.type && crossRoad.type <= CROSSROADTYPE.CORNER_BI_DIRECTIONAL_DOUBLE_TAPE)
        {
            RotateCornerCrossRoad(crossRoad, a, b, c, d);
        }


    }

    public static CrossRoad CreateCrossRoad(Tile a, Tile b, Tile c, Tile d)
    {
        CrossRoad crossRoad;
        switch (CalucalteCrossRoadType(a, b, c, d))
        {

            case CROSSROADTYPE.CORNER_BI_DIRECTIONAL_SINGLE_TAPE:
                crossRoad = GenerateCrossRoad("CrossRoads/CORNER_BI_DIRECTIONAL_SINGLE_TAPE", CROSSROADTYPE.CORNER_BI_DIRECTIONAL_SINGLE_TAPE);                
                break;
            case CROSSROADTYPE.CORNER_BI_DIRECTIONAL_DOUBLE_TAPE:
                crossRoad = GenerateCrossRoad("CrossRoads/CORNER_BI_DIRECTIONAL_DOUBLE_TAPE", CROSSROADTYPE.CORNER_BI_DIRECTIONAL_DOUBLE_TAPE);
                break;
            case CROSSROADTYPE.TCROSSROAD_I_I:
                crossRoad = GenerateCrossRoad("CrossRoads/TCROSSROAD_I_I", CROSSROADTYPE.TCROSSROAD_I_I);
                break;
            case CROSSROADTYPE.TCROSSROAD_II_I:
                crossRoad = GenerateCrossRoad("CrossRoads/TCROSSROAD_II_I", CROSSROADTYPE.TCROSSROAD_II_I);
                break;
            case CROSSROADTYPE.TCROSSROAD_I_II:
                crossRoad = GenerateCrossRoad("CrossRoads/TCROSSROAD_I_II", CROSSROADTYPE.TCROSSROAD_I_II);
                break;
            case CROSSROADTYPE.TCROSSROAD_II_II:
                crossRoad = GenerateCrossRoad("CrossRoads/TCROSSROAD_II_II", CROSSROADTYPE.TCROSSROAD_II_II);
                break;
            case CROSSROADTYPE.FULLCROSSROAD_I_I_I_I:
                crossRoad = GenerateCrossRoad("CrossRoads/FULLCROSSROAD_I_I_I_I", CROSSROADTYPE.FULLCROSSROAD_I_I_I_I);
                break;
            case CROSSROADTYPE.FULLCROSSROAD_II_I_I_I:
                crossRoad = GenerateCrossRoad("CrossRoads/FULLCROSSROAD_II_I_I_I", CROSSROADTYPE.FULLCROSSROAD_II_I_I_I);
                break;
            case CROSSROADTYPE.FULLCROSSROAD_II_II_I_I:
                crossRoad = GenerateCrossRoad("CrossRoads/FULLCROSSROAD_II_II_I_I", CROSSROADTYPE.FULLCROSSROAD_II_II_I_I);
                break;
            case CROSSROADTYPE.FULLCROSSROAD_II_I_II_I:
                crossRoad = GenerateCrossRoad("CrossRoads/FULLCROSSROAD_II_I_II_I", CROSSROADTYPE.FULLCROSSROAD_II_I_II_I);
                break;
            case CROSSROADTYPE.FULLCROSSROAD_II_II_II_I:
                crossRoad = GenerateCrossRoad("CrossRoads/FULLCROSSROAD_II_II_II_I", CROSSROADTYPE.FULLCROSSROAD_II_II_II_I);
                break;
            case CROSSROADTYPE.FULLCROSSROAD_II_II_II_II:
                crossRoad = GenerateCrossRoad("CrossRoads/FULLCROSSROAD_II_II_II_II", CROSSROADTYPE.FULLCROSSROAD_II_II_II_II);
                break;
            default:
                crossRoad = GenerateCrossRoad("CrossRoads/FULLCROSSROAD_II_II_II_II", CROSSROADTYPE.FULLCROSSROAD_II_II_II_II);
                break;
        }

        //rotate correctly crossRoad

        RotateCrossRoad(crossRoad, a, b, c, d);

        return crossRoad;
        
    }


    /// <summary>
    /// input - all roads connected to the crossroad
    /// </summary>
    /// <param name="roads"></param>
    public void ConnectToRoads(List<Road> roads)
    {        

        foreach (RoadCheckPoint start in starts)
        {
            float minDistance = float.PositiveInfinity;
            RoadCheckPoint closest = roads[0].GetEnd(start.transform.position);
            foreach (Road road in roads)
            {
                if (Vector3.Distance(start.transform.position, road.GetEnd(start.transform.position).transform.position) < minDistance)
                {
                    minDistance = Vector3.Distance(start.transform.position, road.GetEnd(start.transform.position).transform.position);
                    closest = road.GetEnd(start.transform.position);
                }
            }            
            closest.next.Add(start);            
        }


        foreach (RoadCheckPoint end in ends)
        {
            float minDistance = float.PositiveInfinity;
            RoadCheckPoint closest = roads[0].GetStart(end.transform.position);
            foreach (Road road in roads)
            {
                if (Vector3.Distance(end.transform.position, road.GetStart(end.transform.position).transform.position) < minDistance)
                {
                    minDistance = Vector3.Distance(end.transform.position, road.GetStart(end.transform.position).transform.position);
                    closest = road.GetStart(end.transform.position);
                }
            }


            end.next.Add(closest);
        }
    }

}
