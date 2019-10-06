//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.IO;
////attach this to country map
//namespace statePivotPos
//{
//    public class State_pos : MonoBehaviour
//    {
//        // Start is called before the first frame update
//        //private StreamWriter file;
//        //private string fileName;
//        public Dictionary<string, Vector3> statePos;
//        public List<string> stateList;

//        public void getStatePos()

//        {
//            //fileName = "Assets/data/state_absolute_pos.csv";
//            //file = new StreamWriter(@fileName, false);
//            statePos = new Dictionary<string, Vector3>();
//            stateList = new List<string>();

//            foreach (Transform state in transform)
//            {
//                //var localpos = state.transform.localPosition;
//                //state.transform.localPosition = new Vector3(localpos.x * 100f, localpos.y * 100f, localpos.z * 100f);

//                var pos = state.transform.position;
//                //var rot = state.transform.rotation;
//                //var scale = state.transform.localScale;
//                //state.transform.localScale = new Vector3(scale.x * 100f, scale.y * 100f, scale.z * 100f);
//                string stateName = state.name;

//                statePos.Add(stateName, pos);
//                stateList.Add(stateName);
//                //file.WriteLine(pos.x.ToString() + "," + pos.y.ToString() + "," + pos.z.ToString() + "," + rot.x.ToString() + "," + rot.y.ToString() + "," + rot.z.ToString() + "," + scale.x.ToString() + "," + scale.y.ToString() + "," + scale.z.ToString());

//            }

//            //file.Close();
//        }

//        // Update is called once per frame
//        //void Update()
//        //{
//        //}
//    }
//}
