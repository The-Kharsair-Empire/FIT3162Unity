using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using nodeTemplate;
using ArcTemplate;
using VRTK.Examples;

//attach this to a empty gameobject named proprocessor: parent of all country nodes;

///////////////////////////////////////////////////////////////////////////////////////////
//FileName: Preprocessor.cs
//FileType: visual C# Source File
//Description: This is the script that reads in all the data,  preprocesses them and stores them into required data structure such as dictionary.
//Last modified on: 23/10/19
//////////////////////////////////////////////////////////////////////////////////////////


public class Preprocessor : MonoBehaviour
{
    public TextAsset node_data; //node_info.csv
    public TextAsset arc_data; //arc_info.csv
    public GameObject nodePrefab; //this is the prefab of the node (individual facitlity) gameobejct used to generate the node on the country.
    public Transform spawnPos_o; //position where the origin state map pop-up comes out
    public Transform spawnPos_d; //position where the destination state map pop-up comes out

	public Dictionary<string, List<GameObject>> stateAndItsNodes; //a dictionary of key-values pairs of state name in string and all the nodes gameobjects in that state
    public Dictionary<string, GameObject> slic_nodeInfo_Pair;//dictionary of one to one key-value pair of slic code in string and the corresponding node gameobject.
    

    public Dictionary<string, List<Arc>> arc_info;  // a dictionary of pairs of slic code in string and a list of information of all the arcs going from this slic node
    public Dictionary<string, List<string>> path_info; //a dictionary of paris of a OD slic code string (origin slic - destination slic) and all the internal nodes that it travels through on its path.

    public GameObject instructionCanvas; // instruction board game object
    private Instruction instructionScript; // instruction script, needed to initialize the text on the instruction board.

    public GameObject FloatingTextPrefab; //text pop-up prefab, need to be passed the the sub module


    /// <summary>
	///called at the start of the program, does all the data preprocessing, and stores them into data structures. initialize other component such as monitor, stateInteraction.
	/// </summary>
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


        transform.position = new Vector3(-4.97f, 1.43f, -2.9608f); //adjust the node on the country map so the nodes are in the right positions
        transform.rotation = Quaternion.Euler(0, 180f, 49.105f);
        transform.localScale = new Vector3(0.01799799f, 0.01812159f, 0.01773624f);

        while (transform.childCount > 0)
        {
            Transform a_node = transform.GetChild(0);
            string state_name = a_node.GetComponent<Node>().slic_state;

            a_node.parent = US.Find(state_name.Replace("\"", "")); // assign the state gameobjects as parents of their corresponding nodes in that state
        }
        
     

        string[] arcs = arc_data.text.Split(new char[] { '\n' });
        string[] arc;

        for(int i = 1; i < arcs.Length - 1; i++) //read in all arc info and store into data structure
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
            for (int j = 6; j < arc.Length; j++) //read in internal paths
            {
                if (arc[j].Replace("\"", "") != "NA") {
                    pth.Add(arc[j].Replace("\"", ""));
                }
                
            }
            path_info.Add(OD, pth);
        }

        gameObject.GetComponent<Monitor>().activate(slic_nodeInfo_Pair, path_info);//activate the monitor script

        foreach(Transform each_state in US)
        {
            each_state.gameObject.AddComponent<StateInteraction>().activateStateInteraction(gameObject, spawnPos_o, spawnPos_d, arc_info, slic_nodeInfo_Pair, instructionScript); 
            //apply stateInteraction script to each state game object.
            //add extra component to this state
        }


    }

}
