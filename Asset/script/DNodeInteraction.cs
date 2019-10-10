using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcTemplate;
using nodeTemplate;
using VRTK.Examples;

public class DNodeInteraction : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame


    private Dictionary<string, GameObject> slic_nodeInfo_Pair;
    private GameObject Global;
    public Instruction inScript;

    public void activateNodeInteraction(Dictionary<string, GameObject> slic_node, GameObject Global, Instruction inScript)
    {

        this.slic_nodeInfo_Pair = slic_node;
        this.Global = Global;
        gameObject.AddComponent<checkLabel>().setLabel("DStateNode");
        //Debug.Log(this.slic_nodeInfo_Pair);
        //Debug.Log(slic_nodeInfo_Pair);
        this.inScript = inScript;
    }

    public void myInteract()
    {
        inScript.changeText(4);
        GameObject o_node = Global.GetComponent<Monitor>().o_node;
        //GameObject last_d_node = preprocessor.GetComponent<Preprocessor>().d_node;


        GameObject cor_node = gameObject.GetComponent<Node>().cor_node;
        //cor_node.transform.localScale += new Vector3(1f, 1f, 1f);
        //preprocessor.GetComponent<Preprocessor>().o_node.transform.localScale -= new Vector3(1f, 1f, 1f);
        Global.GetComponent<Monitor>().d_node = cor_node;
        GameObject d_node = Global.GetComponent<Monitor>().d_node;
        //are we hightlighting it on state map?

        //gameObject.GetComponent<MeshRenderer>().enabled = true; //highlighting instead if enabing meshrenderer // state map? should already by visible --> is this supposed to highlight it?
        string d_slic = d_node.GetComponent<Node>().slic_code;
        foreach (Transform bezierContainer in o_node.transform)
        {
            //Debug.Log(bezierContainer.name);
            //Debug.Log(d_slic);
            //Debug.Log(bezierContainer.name == d_slic)

            if (bezierContainer.name != d_slic)
            {
                bezierContainer.GetComponent<bezier>().turnOff();
            }
            else
            {
                bezierContainer.GetComponent<bezier>().turnOn();
            }
        }



        // get destination nodes and turn on their renderers (on country map)
        // add bezier curves from origin to destinations on country map




        //void Update()
        //{
        //    if (Input.GetMouseButton(0))
        //    {
        //        if (isOrigin)
        //        {
        //            GameObject cor_node_on_country = transform.GetComponent<Node>().cor_node;
        //            GameObject.Find("proprocessor").GetComponent<Preprocessor>().o_node = cor_node_on_country;


        //            //attach line renderer to cor_node_on_country
        //            //pass info
        //            //GameObject i = gameObject.GetComponent<NodeInfoContainer>().getNodeInfo().cor_node;
        //        }

        //        else
        //        {

        //            GameObject cor_node_on_country = transform.GetComponent<Node>().cor_node;
        //            GameObject.Find("proprocessor").GetComponent<Preprocessor>().d_node = cor_node_on_country;

        //            //find origin node in preprocessor: o_node
        //            //using the line renderer in o_node, pass the pos of d_node, draw line
        //        }


        //    }
        //}




    }
}
