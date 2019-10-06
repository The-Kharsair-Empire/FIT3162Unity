//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//public class DataLoader : MonoBehaviour
//{

//    public TextAsset node_pos; //absolute_node_pos
//    public TextAsset state_pos; //absolute_state_pos
//    public Dictionary<string, List<string[]>> nodeInfoByState;
//    //public Dictionary<string, Pair<float, float>> stateCenter;
//    // Start is called before the first frame update
//    void Start()
//    {
//        // TextAsset node_data = .load<TextAsset>("lambert"); 
//        string[] rows = node_pos.text.Split(new char[] {'\n'});
//        Debug.Log(rows.Length);
//        nodeInfoByState = new Dictionary<string, List<string[]>>();
//        //stateCenter = new Dictionary<string, Pair<float, float>>();
//        for (int i = 1; i < rows.Length-1; i++) {
//            string[] row = rows[i].Split(new char[] {','});
//            if (!nodeInfoByState.ContainsKey(row[2]))
//            {
//                nodeInfoByState.Add(row[2], new List<string[]>());
//                nodeInfoByState[row[2]].Add(row);
//            } else
//            {
//                nodeInfoByState[row[2]].Add(row);
//            }
            
//            // Debug.Log(node.x);
//            // Debug.Log(node.y);
//        }

//        // Pair<float, float> lat_long = new Pair<float, float>(float.Parse(row[0]), float.Parse(row[1]));


//    }

//}

//// public class Pair<T, U>
//// {
////     public Pair()
////     {
////     }

////     public Pair(T first, U second)
////     {
////         this.First = first;
////         this.Second = second;
////     }

////     public T First { get; set; }
////     public U Second { get; set; }
//// };
