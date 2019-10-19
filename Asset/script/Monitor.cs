
namespace VRTK.Examples
{

    using System.Collections.Generic;
    using UnityEngine;
    using ArcTemplate;
    using nodeTemplate;
  


    public class Monitor : MonoBehaviour
    {
        public GameObject FloatingTextPrefab;
        public GameObject origin = null; //o_state pop-up object
        public GameObject destination = null; // destination state pop-up object

        public GameObject o_node = null;
        public GameObject d_node = null;
        public Dictionary<string, GameObject> slic_nodeInfo_Pair;
        public Dictionary<string, List<string>> path_info;
        public GameObject USprefab;
        private string copy_d_node = null;
        private string copy_o_node = null;
        public GameObject origin_text = null;
        public GameObject destination_text = null;
        private GameObject colorLegend = null; 

        private float volume;
        private GameObject textPos;

        public void activate(Dictionary<string, GameObject> slic_nodeInfo_Pair, Dictionary<string, List<string>> path_info)
        {
            this.slic_nodeInfo_Pair = slic_nodeInfo_Pair;
            this.path_info = path_info;
            colorLegend = GameObject.Find("/Colour_legend");
            colorLegend.SetActive(false);
        }

        public void drawArcs(GameObject cor_node, List<Arc> destinations)
        {
            colorLegend.SetActive(true);
            if (destinations != null)
            {
                foreach (Arc a_arc in destinations)
                {
                    string d = a_arc.d;
                    GameObject d_node;

                    if (slic_nodeInfo_Pair.TryGetValue(d, out d_node))
                    {

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

        public void showArcs(GameObject cor_node)
        {
            foreach (Transform bezierContainer in cor_node.transform)
            {
                bezierContainer.GetComponent<bezier>().turnOn();
            }
        }

        public void hideArcs(GameObject cor_node)
        {
            foreach (Transform bezierContainer in cor_node.transform)
            {
                bezierContainer.GetComponent<bezier>().turnOff();
            }
        }

        public void copyArc(Vector3 controllerPosition)
        {

            if (d_node != null && (copy_d_node != d_node.GetComponent<Node>().slic_code || copy_o_node != o_node.GetComponent<Node>().slic_code))
            {
                GameObject smallUS = Instantiate(USprefab);
                Vector3 diff = d_node.transform.position - o_node.transform.position;
                string OD = o_node.GetComponent<Node>().slic_code + "-" + d_node.GetComponent<Node>().slic_code;
                List<string> pth;
                List<GameObject> nodesInPath = new List<GameObject>();
                GameObject a_node;
                GameObject cpyNode;
                if (path_info.TryGetValue(OD, out pth))
                {
                    for (int i = 0; i < pth.Count; i++)
                    {
                        if (slic_nodeInfo_Pair.TryGetValue(pth[i], out a_node)){
                            cpyNode = Instantiate(a_node, a_node.transform.position, a_node.transform.rotation);
                            cpyNode.transform.localScale = new Vector3(0.02137621f, 0.02137621f, 0.02137621f);
                            cpyNode.GetComponent<MeshRenderer>().enabled = true;
                            cpyNode.transform.parent = smallUS.transform;
                            nodesInPath.Add(cpyNode);

                            if (i > 0)
                            {
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

                newONode.transform.localScale = new Vector3(0.02137621f, 0.02137621f, 0.02137621f);
                newDNode.transform.localScale = new Vector3(0.02137621f, 0.02137621f, 0.02137621f);

                newONode.transform.parent = smallUS.transform;
                newDNode.transform.parent = smallUS.transform;


                smallUS.transform.position = controllerPosition + new Vector3(0f, 0.5f, 0f);
                smallUS.transform.localScale *= 0.2f;
                smallUS.transform.rotation = Quaternion.Euler(-90f, 0, 0);
            



                copy_d_node = d_node.GetComponent<Node>().slic_code;
                copy_o_node = o_node.GetComponent<Node>().slic_code;
                smallUS.AddComponent<MeshCollider>().convex = true;
                smallUS.AddComponent<Rigidbody>().useGravity = false;
                smallUS.GetComponent<Rigidbody>().isKinematic = true;
                smallUS.AddComponent<VRTK_InteractableObject>().isGrabbable = true;
                smallUS.GetComponent<VRTK_InteractableObject>().holdButtonToUse = false;
                smallUS.GetComponent<VRTK_InteractableObject>().isUsable = true;
                smallUS.GetComponent<VRTK_InteractableObject>().pointerActivatesUseAction = true;

                for (int i =0; i < o_node.transform.childCount; i++)
                {
                    if (o_node.transform.GetChild(i).name == copy_d_node)
                    {
                        this.volume = o_node.transform.GetChild(i).GetComponent<bezier>().volume;
                        this.textPos = newONode.transform.GetChild(i).GetComponent<bezier>().point1;
                    }
                }

               
                smallUS.AddComponent<checkLabel>().setLabel("Arc");

                smallUS.AddComponent<volumeInfo>().activate(volume, FloatingTextPrefab, textPos, newONode.GetComponent<Node>().info, newDNode.GetComponent<Node>().info);

            }
        }

        public void clearArc()
        {
            copy_o_node = null;
            copy_d_node = null;
        }
    }

}