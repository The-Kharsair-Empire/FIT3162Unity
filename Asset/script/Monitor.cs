
namespace VRTK.Examples
{

    using System.Collections.Generic;
    using UnityEngine;
    using ArcTemplate;
    using nodeTemplate;
  


    public class Monitor : MonoBehaviour
    {
        public GameObject origin = null; //o_state pop-up object
        public GameObject destination = null; // destination state pop-up object

        public GameObject o_node = null;
        public GameObject d_node = null;
        public Dictionary<string, GameObject> slic_nodeInfo_Pair;
        private string copy_d_node = null;
        private string copy_o_node = null;

        public void activate(Dictionary<string, GameObject> slic_nodeInfo_Pair)
        {
            this.slic_nodeInfo_Pair = slic_nodeInfo_Pair;
        }

        public void drawArcs(GameObject cor_node, List<Arc> destinations)
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
                    bezierContainer.AddComponent<bezier>().activate(d_node, a_arc.vol);
                    d_node.GetComponent<MeshRenderer>().enabled = true;
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
                Vector3 diff = d_node.transform.position - o_node.transform.position;
                GameObject empty = new GameObject();

                GameObject newONode = Instantiate(o_node);
                GameObject newDNode = Instantiate(d_node);

                newONode.transform.localScale = new Vector3(0.02137621f, 0.02137621f, 0.02137621f);
                newDNode.transform.localScale = new Vector3(0.02137621f, 0.02137621f, 0.02137621f);

                newONode.transform.position = new Vector3(0f, 0f, 0f);
                newDNode.transform.position = new Vector3(0f, 0f, 0f);
                newDNode.transform.position += diff;

                newONode.transform.parent = empty.transform;
                newDNode.transform.parent = empty.transform;

                empty.transform.position = controllerPosition;

                empty.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);

                copy_d_node = d_node.GetComponent<Node>().slic_code;
                copy_o_node = o_node.GetComponent<Node>().slic_code;
                empty.AddComponent<BoxCollider>();
                empty.AddComponent<Rigidbody>().useGravity = false;
                empty.GetComponent<Rigidbody>().isKinematic = true;
                empty.AddComponent<VRTK_InteractableObject>().isGrabbable = true;
                empty.AddComponent<checkLabel>().setLabel("Arc");

            }
        }
    }

}