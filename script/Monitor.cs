/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// FileName: Monitor.cs
// FileType: C#
// Description: This file is attached to an object called 'Global' which acts as the monitor of the whole system. It provides 
//              each unit in the system with multiple functions and global variables.
// Last modified on: 23/10/2019
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace VRTK.Examples
{

    using System.Collections.Generic;
    using UnityEngine;
    using ArcTemplate;
    using nodeTemplate;
  


    public class Monitor : MonoBehaviour
    {
        public GameObject FloatingTextPrefab;// textbox prefab
        public GameObject origin = null; // reference to the origin state pop-up object
        public GameObject destination = null; // reference to the destination state pop-up object

        public GameObject o_node = null; // reference to the origin node pop-up object
        public GameObject d_node = null; // reference to the destination node pop-up object
        public Dictionary<string, GameObject> slic_nodeInfo_Pair; // Dictionaty contains the information of nodes with their slic code as the key
        public Dictionary<string, List<string>> path_info; // Dictionary contains the information of the path
        public GameObject USprefab; // A copy of the country map prefab
        private string copy_d_node = null; // reference to the copy of the origin node pop-up object on the minimap
        private string copy_o_node = null; // reference to the copy of the destination node pop-up object on the minimap
        public GameObject origin_text = null;  // reference to the textbox attached to the origin node
        public GameObject destination_text = null; // reference to the textbox attached to the destination node
        private GameObject colorLegend = null; // reference to the color legend textbox

        private float volume; // the volume of the OD link 
        private GameObject textPos; // the position of the textbox

        /// <summary>
        /// activate the Monitor object and its script, read in two dictionaries to provide data with other objects, create a color legend
        /// as the reference but hide it initially
        /// </summary>
        /// <param name="slic_nodeInfo_Pair">Dictionaty contains the information of nodes with their slic code as the key</param>
        /// <param name="path_info">Dictionary contains the information of the path</param>
        public void activate(Dictionary<string, GameObject> slic_nodeInfo_Pair, Dictionary<string, List<string>> path_info)
        {
            this.slic_nodeInfo_Pair = slic_nodeInfo_Pair;
            this.path_info = path_info;
            colorLegend = GameObject.Find("/Colour_legend");
            colorLegend.SetActive(false);
        }

        /// <summary>
        /// draw arcs from the orgin node to all its destination nodes on the country map
        /// </summary>
        /// <param name="cor_node">the corresponding node on the country map of the selected origin node</param>
        /// <param name="destinations">a list containing all arcs and their destination from the corresponding node</param>
        public void drawArcs(GameObject cor_node, List<Arc> destinations)
        {
            colorLegend.SetActive(true);
            if (destinations != null)
            {
                // check if some destinations exist
                foreach (Arc a_arc in destinations)
                {
                    // for each arc from the corresponding node
                    string d = a_arc.d; // find the destination node
                    GameObject d_node;

                    if (slic_nodeInfo_Pair.TryGetValue(d, out d_node))
                    {
                        // generate the OD link
                        GameObject bezierContainer = new GameObject();
                        bezierContainer.transform.parent = cor_node.transform;
                        bezierContainer.name = d;
                        bezierContainer.AddComponent<LineRenderer>();
                        bezierContainer.AddComponent<bezier>().activate(d_node, a_arc.vol, Color.blue, Color.red, -1f);
                        d_node.GetComponent<MeshRenderer>().enabled = true;
                    }

                }
            }
        }

        /// <summary>
        /// show all the arcs which start from the corresponding node
        /// </summary>
        /// <param name="cor_node">the corresponding node on the country map of the selected origin node</param>
        public void showArcs(GameObject cor_node)
        {
            // show all the arcs which start from the corresponding node
            foreach (Transform bezierContainer in cor_node.transform)
            {
                bezierContainer.GetComponent<bezier>().turnOn();
            }
        }

        /// <summary>
        /// hide all the arcs which start from the corresponding node
        /// </summary>
        /// <param name="cor_node">the corresponding node on the country map of the selected origin node</param>
        public void hideArcs(GameObject cor_node)
        {
            foreach (Transform bezierContainer in cor_node.transform)
            {
                bezierContainer.GetComponent<bezier>().turnOff();
            }
        }

        /// <summary>
        /// Copy the current orgin node, destination node, and their corresponding OD link to a copy of the country map and put them 
        /// on the controller. This function describes how the minimap is created
        /// </summary>
        /// <param name="controllerPosition">the position of the controller</param>
        public void copyArc(Vector3 controllerPosition)
        {

            if (d_node != null && (copy_d_node != d_node.GetComponent<Node>().slic_code || copy_o_node != o_node.GetComponent<Node>().slic_code))
            {
                // make sure only one arc exists and is different from the last arc copy. We want to avoid repeating copying one arc for multiple
                // times by mistake
                GameObject smallUS = Instantiate(USprefab);
                // instantiate the copy of the country map prefab
                Vector3 diff = d_node.transform.position - o_node.transform.position;
                string OD = o_node.GetComponent<Node>().slic_code + "-" + d_node.GetComponent<Node>().slic_code;
                List<string> pth;
                List<GameObject> nodesInPath = new List<GameObject>();
                GameObject a_node;
                GameObject cpyNode;
                if (path_info.TryGetValue(OD, out pth))
                    // find corresponding inter path
                {
                    for (int i = 0; i < pth.Count; i++)
                    {
                        // copy all nodes on the inter path and adjust their positions and sizes
                        if (slic_nodeInfo_Pair.TryGetValue(pth[i], out a_node)){
                            cpyNode = Instantiate(a_node, a_node.transform.position, a_node.transform.rotation);
                            cpyNode.transform.localScale = new Vector3(0.02137621f, 0.02137621f, 0.02137621f);
                            cpyNode.GetComponent<MeshRenderer>().enabled = true;
                            cpyNode.transform.parent = smallUS.transform;
                            nodesInPath.Add(cpyNode);

                            if (i > 0)
                            {
                                // create OD links between these nodes as the inter path
                                GameObject start = nodesInPath[i - 1];
                                GameObject bezierContainer = new GameObject();
                                bezierContainer.transform.parent = start.transform;
                                bezierContainer.AddComponent<LineRenderer>();
                                bezierContainer.AddComponent<bezier>().activate(cpyNode, 0.0f, Color.green, Color.yellow, 0.2f);
                                
                            }
                        }
                    }
                }

                GameObject newONode = Instantiate(o_node, o_node.transform.position, o_node.transform.rotation);
                GameObject newDNode = Instantiate(d_node, d_node.transform.position, d_node.transform.rotation);
                // copy the origin and destination node

                newONode.transform.localScale = new Vector3(0.02137621f, 0.02137621f, 0.02137621f);
                newDNode.transform.localScale = new Vector3(0.02137621f, 0.02137621f, 0.02137621f);
                // adjust the sizes of the nodes

                newONode.transform.parent = smallUS.transform;
                newDNode.transform.parent = smallUS.transform;
                // reset their parent to the minimap

                smallUS.transform.position = controllerPosition + new Vector3(0f, 0.5f, 0f);
                smallUS.transform.localScale *= 0.2f;
                smallUS.transform.rotation = Quaternion.Euler(-90f, 0, 0);
                // adjust the position, scale, and rotation of the minimap

                copy_d_node = d_node.GetComponent<Node>().slic_code;
                copy_o_node = o_node.GetComponent<Node>().slic_code;
                smallUS.AddComponent<MeshCollider>().convex = true;
                smallUS.AddComponent<Rigidbody>().useGravity = false;
                smallUS.GetComponent<Rigidbody>().isKinematic = true;
                smallUS.AddComponent<VRTK_InteractableObject>().isGrabbable = true;
                smallUS.GetComponent<VRTK_InteractableObject>().holdButtonToUse = false;
                smallUS.GetComponent<VRTK_InteractableObject>().isUsable = true;
                smallUS.GetComponent<VRTK_InteractableObject>().pointerActivatesUseAction = true;
                // attach necessary components to the minimap

                for (int i =0; i < o_node.transform.childCount; i++)
                {
                    if (o_node.transform.GetChild(i).name == copy_d_node)
                    {
                        this.volume = o_node.transform.GetChild(i).GetComponent<bezier>().volume;
                        this.textPos = newONode.transform.GetChild(i).GetComponent<bezier>().point1;
                    }
                }
                // find the corresponding arc on the country map and retrieve the information
                
                smallUS.AddComponent<checkLabel>().setLabel("Arc");
                smallUS.AddComponent<volumeInfo>().activate(volume, FloatingTextPrefab, textPos, newONode.GetComponent<Node>().info, newDNode.GetComponent<Node>().info);
                // set the label for the minimap and activate the minimap
            }
        }

        /// <summary>
        /// Reset the history of the arc copy
        /// </summary>
        public void clearArc()
        {
            copy_o_node = null;
            copy_d_node = null;
        }
    }

}
