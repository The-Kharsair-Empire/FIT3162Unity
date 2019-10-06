using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcTemplate;
using nodeTemplate;
using VRTK.Examples;

public class ONodeInteraction : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame

    
    private List<Arc> destinations;
    private Dictionary<string, GameObject> slic_nodeInfo_Pair;
    private GameObject Global;

    public void activateNodeInteraction(Dictionary<string, GameObject> slic_node, GameObject Global)
    {
        
        this.slic_nodeInfo_Pair = slic_node;
        this.Global = Global;
        gameObject.AddComponent<checkLabel>().setLabel("OStateNode");
        //Debug.Log(this.slic_nodeInfo_Pair);
        //Debug.Log(slic_nodeInfo_Pair);
    }

    public void myInteract()
    {
        GameObject o_node = Global.GetComponent<Monitor>().o_node;
        //GameObject d_node = preprocessor.GetComponent<Preprocessor>().d_node;
        if (Global.GetComponent<Monitor>().destination == null)
        {
            if (o_node == null)
            {
                GameObject cor_node = gameObject.GetComponent<Node>().cor_node;
                //cor_node.transform.localScale += new Vector3(1f, 1f, 1f);
                //preprocessor.GetComponent<Preprocessor>().o_node.transform.localScale -= new Vector3(1f, 1f, 1f);
                Global.GetComponent<Monitor>().o_node = cor_node;
                cor_node.GetComponent<Node>().destinations = destinations;
                //are we hightlighting it on state map?

                //gameObject.GetComponent<MeshRenderer>().enabled = true; //highlighting instead if enabing meshrenderer // state map? should already by visible --> is this supposed to highlight it?
                cor_node.GetComponent<MeshRenderer>().enabled = true;

                if (cor_node.transform.childCount == 0)
                {
                    Global.GetComponent<Monitor>().drawArcs(cor_node, destinations);
                }
                else
                {
                    Global.GetComponent<Monitor>().showArcs(cor_node);
                }


                // get destination nodes and turn on their renderers (on country map)
                // add bezier curves from origin to destinations on country map

            }
            else
            {
                // if newly selected node is not the same as previously selected node:
                if (Global.GetComponent<Monitor>().o_node != gameObject.GetComponent<Node>().cor_node)
                {

                    Global.GetComponent<Monitor>().hideArcs(Global.GetComponent<Monitor>().o_node);
                    // use old origin node to turn off nodes on country map before replacing
                    Global.GetComponent<Monitor>().o_node.GetComponent<MeshRenderer>().enabled = false;
                    // update origin node.
                    GameObject cor_node = gameObject.GetComponent<Node>().cor_node;
                    Global.GetComponent<Monitor>().o_node = cor_node;
                    cor_node.GetComponent<Node>().destinations = destinations;
                    cor_node.GetComponent<MeshRenderer>().enabled = true;

                    if (cor_node.transform.childCount == 0)
                    {
                        Global.GetComponent<Monitor>().drawArcs(cor_node, destinations);
                    }
                    else
                    {
                        Global.GetComponent<Monitor>().showArcs(cor_node);
                    }
                }

            }
        }

        

    }

    

    public void addDestInfo(List<Arc> destinations)
    {
        this.destinations = destinations;
    }

    


}
