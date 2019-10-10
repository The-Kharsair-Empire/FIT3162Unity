using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//using statePivotPos;
using nodeTemplate;
using ArcTemplate;
using VRTK.Examples;

//attach this to a empty gameobject named proprocessor: parent of all country nodes;

public class Preprocessor : MonoBehaviour
{
    public TextAsset node_data; //lambert with state and slic info, node_info.csv
    public TextAsset arc_data; //arc.csv
    public GameObject nodePrefab;
    public Transform spawnPos_o;
    public Transform spawnPos_d;
    
    public Dictionary<string, List<GameObject>> stateAndItsNodes;
    public Dictionary<string, GameObject> slic_nodeInfo_Pair;//country
    

    public Dictionary<string, List<Arc>> arc_info;
    public Dictionary<string, List<string>> path_info;

    public GameObject instructionCanvas;
    private Instruction instructionScript;

    public GameObject FloatingTextPrefab;

    //Dictionary<string, Vector3> statePos;
    // Start is called before the first frame update
    void Start()
    {
        string[] rows = node_data.text.Split(new char[] { '\n' });
        string[] row;

        instructionScript = instructionCanvas.GetComponent<Instruction>();

        stateAndItsNodes = new Dictionary<string, List<GameObject>>();
        arc_info = new Dictionary<string, List<Arc>>();
        slic_nodeInfo_Pair = new Dictionary<string, GameObject>();
        path_info = new Dictionary<string, List<string>>();
        Transform US = GameObject.Find("/America").transform;
       

        for (int i = 1; i < rows.Length - 1; i++) //create all nodes on the country map
        {
           
            row = rows[i].Split(new char[] { ',' });
            GameObject a_node = Instantiate(nodePrefab, new Vector3(float.Parse(row[9]) * 100, float.Parse(row[10]) * 100, 0f), Quaternion.identity, transform);
            //a_node.tag = row[3];
            a_node.GetComponent<MeshRenderer>().enabled = false;
            a_node.transform.localScale += new Vector3(1f, 1f, 1f);



            a_node.AddComponent<Node>().setNodeInfo(row[0].Replace("\"", ""), row[1].Replace("\"", ""), row[2].Replace("\"", ""), row[3].Replace("\"", ""), row[4].Replace("\"", ""), row[5].Replace("\"", ""), int.Parse(row[6]), float.Parse(row[7]), float.Parse(row[8]), FloatingTextPrefab);
            //nodeList.Add(a_node);

            List<GameObject> nodesInState;
            if (stateAndItsNodes.TryGetValue(row[3].Replace("\"", ""), out nodesInState))
            {
                nodesInState.Add(a_node);
                //need reAdd it into the dictionary?
            }
            else
            {
                nodesInState = new List<GameObject>();
                nodesInState.Add(a_node);
                stateAndItsNodes.Add(row[3].Replace("\"", ""), nodesInState);
            }

            if (!slic_nodeInfo_Pair.ContainsKey(row[0].Replace("\"", "")))
            {
                slic_nodeInfo_Pair.Add(row[0].Replace("\"", ""), a_node);
            }

            
        }


        transform.position = new Vector3(-4.97f, 1.43f, -2.9608f);
        transform.rotation = Quaternion.Euler(0, 180f, 49.105f);
        transform.localScale = new Vector3(0.01799799f, 0.01812159f, 0.01773624f);

        //transform.position = new Vector3(-21.22162f, 2.137f, -14.93f);
        //transform.rotation = Quaternion.Euler(0, 180f, 49.105f);
        //transform.localScale = new Vector3(0.07155591f, 0.0720473f, 0.07051525f);


        while (transform.childCount > 0)
        {
            Transform a_node = transform.GetChild(0);
            string state_name = a_node.GetComponent<Node>().slic_state;

            a_node.parent = US.Find(state_name.Replace("\"", ""));
        }
        
     

        string[] arcs = arc_data.text.Split(new char[] { '\n' });
        string[] arc;

        for(int i = 1; i < arcs.Length - 1; i++)
        {
            arc = arcs[i].Split(new char[] { ',' });
            List<Arc> destList;
            if (arc_info.TryGetValue(arc[0].Replace("\"", ""), out destList))
            {
                destList.Add(new Arc(arc[0].Replace("\"", ""), arc[1].Replace("\"", ""), float.Parse(arc[2])));
            }
            else
            {
                destList = new List<Arc>();
                Arc a_arc = new Arc(arc[0].Replace("\"", ""), arc[1].Replace("\"", ""), float.Parse(arc[2]));
                destList.Add(a_arc);
                arc_info.Add(arc[0].Replace("\"", ""), destList);
            }

            string OD = arc[0].Replace("\"", "") + "-" + arc[1].Replace("\"", "");

            List<string> pth = new List<string>(); 
            for (int j = 6; j < arc.Length; j++)
            {
                if (arc[j].Replace("\"", "") != "NA") {
                    pth.Add(arc[j].Replace("\"", ""));
                }
                
            }
            path_info.Add(OD, pth);
        }

        //State_pos stateInfoManager = new State_pos();
        //stateInfoManager.getStatePos();
        //List<string> stateList = stateInfoManager.stateList;
        gameObject.GetComponent<Monitor>().activate(slic_nodeInfo_Pair, path_info);

        foreach(Transform each_state in US)
        {
            each_state.gameObject.AddComponent<StateInteraction>().activateStateInteraction(gameObject, spawnPos_o, spawnPos_d, arc_info, slic_nodeInfo_Pair, instructionScript);

            //add extra component to this state
        }

        //gameObject.GetComponent<checkData>().activate(slic_nodeInfo_Pair, arc_info);


        //statePos = stateInfoManager.statePos;
        //string stateName;
        //Vector3 stateVector3;
        //List<GameObject> nodes;
        //Transform US = GameObject.Find("/America").transform;

        //foreach(KeyValuePair<string, Vector3> i in statePos) //calculate pos difference between state pivot and its nodes' pivots
        //{
        //    stateName = i.Key;
        //    stateVector3 = i.Value;
        //    if (stateAndItsNodes.TryGetValue(stateName, out nodes))
        //    {
        //        foreach (GameObject n in nodes)
        //        {
        //            Vector3 diff = n.transform.position - stateVector3;
        //            n.GetComponent<NodeInfoContainer>().getNodeInfo().distFromStatePivot = diff;
        //        }
        //    }
        //    else //test, del later
        //    {
        //        throw new System.Exception("a state map has no node list");
        //    }

        //    US.Find(stateName); //continue find each state and attach stateinteraction script to them
        //}


    }

}
