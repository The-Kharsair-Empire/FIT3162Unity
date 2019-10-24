/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// FileName: PointerListener.cs
// FileType: C#
// Author: Stark C.
// Description: This file is attached to all the nodes on the origin state pop-up to support clicking on the origin nodes.
// Last modified on: 23/10/2019
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcTemplate;
using nodeTemplate;
using VRTK.Examples;

public class ONodeInteraction : MonoBehaviour
{
   
    private List<Arc> destinations; // a list of arcs from this node to all its destinations
    private Dictionary<string, GameObject> slic_nodeInfo_Pair; // Dictionaty contains the information of nodes with their slic code as the key
    private GameObject Global; // refers to the Monitor object in this project to access some global variables
    public Instruction inScript; // script attached to the canvas to change the description in the textbox

    /// <summary>
    /// activate the orgin node object and its script, read in one dictionary to provide node information, and a reference to the script of
    /// the instruction textbox 
    /// </summary>
    /// <param name="slic_node">Dictionaty contains the information of nodes with their slic code as the key</param>
    /// <param name="Global">refers to the Monitor object in this project to access some global variables</param>
    /// <param name="inScript">script attached to the canvas to change the description in the textbox</param>
    public void activateNodeInteraction(Dictionary<string, GameObject> slic_node, GameObject Global, Instruction inScript)
    {
        
        this.slic_nodeInfo_Pair = slic_node;
        this.Global = Global;
        gameObject.AddComponent<checkLabel>().setLabel("OStateNode");
        //Debug.Log(this.slic_nodeInfo_Pair);
        //Debug.Log(slic_nodeInfo_Pair);
        this.inScript = inScript;
    }

    /// <summary>
    /// Interaction function called by the listener on the pointer when users press the touchpad button 
    /// </summary>
    public void myInteract()
    {
        inScript.changeText(2);
        // change the instructions
        GameObject o_node = Global.GetComponent<Monitor>().o_node;
        // get the old origin node
        if (Global.GetComponent<Monitor>().destination == null)
        {
            // new orgin node can only be selected when the destination state pop-up does not exist
            if (o_node == null)
            {
                // if the old origin node does not exist
                GameObject cor_node = gameObject.GetComponent<Node>().cor_node;
                // get the corresponding node on the country map
                Global.GetComponent<Monitor>().o_node = cor_node;
                // set it as the origin node
                cor_node.GetComponent<Node>().destinations = destinations;
                // attach the list of destinations to the corresponding node

                cor_node.GetComponent<MeshRenderer>().enabled = true;
                // enable the render of the corresponding node

                if (cor_node.transform.childCount == 0)
                {
                    Global.GetComponent<Monitor>().drawArcs(cor_node, destinations);
                }
                else
                {
                    Global.GetComponent<Monitor>().showArcs(cor_node);
                }
                // draw all arcs which start from the corresponding node, or show them if they exist


                // get destination nodes and turn on their renderers (on country map)
                // add bezier curves from origin to destinations on country map

            }
            else
            {
                // if the old origin node does not exist
                // if newly selected node is not the same as previously selected node:
                if (Global.GetComponent<Monitor>().o_node != gameObject.GetComponent<Node>().cor_node)
                {

                    Global.GetComponent<Monitor>().hideArcs(Global.GetComponent<Monitor>().o_node);
                    // use old origin node to turn off nodes on country map before replacing
                    Global.GetComponent<Monitor>().o_node.GetComponent<MeshRenderer>().enabled = false;
                    // update origin node.
                    GameObject cor_node = gameObject.GetComponent<Node>().cor_node;
                    // show all the arcs similarly
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

    /// <summary>
    /// read in a list of all OD links
    /// </summary>
    /// <param name="destinations">a list of all OD links</param>
    public void addDestInfo(List<Arc> destinations)
    {
        this.destinations = destinations;
    }
}