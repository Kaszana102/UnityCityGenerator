using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCheckPoint : MonoBehaviour
{
    public List<RoadCheckPoint> next;

    bool isStart,isEnd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }    

    public RoadCheckPoint GetNext()
    {
        int index;
        index = Random.Range(0, next.Count);        
        return next[index];
    }


}
