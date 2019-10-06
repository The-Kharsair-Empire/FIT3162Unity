using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcTemplate;
using nodeTemplate;

public class checkData : MonoBehaviour
{
    
    Dictionary<string, GameObject> slic_nodeInfo_Pair;
    Dictionary<string, List<Arc>> arc_info;


    // Start is called before the first frame update
    public void activate( Dictionary<string, GameObject> slic_nodeInfo_Pair, Dictionary<string, List<Arc>> arc_info)
    {
       
        this.slic_nodeInfo_Pair = slic_nodeInfo_Pair;
        this.arc_info = arc_info;
        foreach (var item in arc_info)
        {
            List<Arc> aList = item.Value;
            foreach(var i in aList)
            {
                GameObject node;
                if(slic_nodeInfo_Pair.TryGetValue(i.d, out node))
                {
                    Debug.Log(node.GetComponent<Node>().slic_state);
                }
            }
        }
    }

    // Update is called once per frame
  
}
