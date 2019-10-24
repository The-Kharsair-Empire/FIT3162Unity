using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcTemplate;
using nodeTemplate;
using VRTK.Examples;

///////////////////////////////////////////////////////////////////////////////////////////
//FileName: DNodeInteraction.cs
//FileType: visual C# Source File
//Author: Stark C
//Description: This applies functionalities to the Node on the Destination state map pop-up.
//Last modified on: 23/10/19
//////////////////////////////////////////////////////////////////////////////////////////


public class DNodeInteraction : MonoBehaviour
{

    private Dictionary<string, GameObject> slic_nodeInfo_Pair; // A dictionary that contains key-value pairs of SLIC Code in String and the reference to the Actual GameObject of the Node on the map, will be needed later to find the game object using its slic code.
    private GameObject Global; // A empty game object that stores the monitor.cs which stores some global variables and functions
    public Instruction inScript; //Instruction.cs that store the instruction text, will change it based on the current program state.


    /// <summary>
	/// This is the function called when stateInteraction applies this script to each node on the destination state pop-up, it acts as the initializing funcion
	/// of this script, receiving and setting the information needed later for the bezier curve
	/// </summary>
	/// <param name="slic_node"></param>
	/// <param name="Global"></param>
	/// <param name="inScript"></param>
    public void activateNodeInteraction(Dictionary<string, GameObject> slic_node, GameObject Global, Instruction inScript)
    {

        this.slic_nodeInfo_Pair = slic_node;
        this.Global = Global;
        gameObject.AddComponent<checkLabel>().setLabel("DStateNode"); //apply a label for pointerlistener to distinguish the object

        this.inScript = inScript;
    }

    /// <summary>
	/// determine what happens when you click on the a destination node, mainly hides all the arcs from the selected origin except the one that is coming to this clicked destination node
	/// </summary>
    public void myInteract()
    {
        inScript.changeText(4);//change text on the instruction board after selecting the destination node.
        GameObject o_node = Global.GetComponent<Monitor>().o_node;


        GameObject cor_node = gameObject.GetComponent<Node>().cor_node;
        Global.GetComponent<Monitor>().d_node = cor_node;
        GameObject d_node = Global.GetComponent<Monitor>().d_node;

        string d_slic = d_node.GetComponent<Node>().slic_code;
        foreach (Transform bezierContainer in o_node.transform) //hide all arcs except the arc coming to this node.
        {

            if (bezierContainer.name != d_slic)
            {
                bezierContainer.GetComponent<bezier>().turnOff();
            }
            else
            {
                bezierContainer.GetComponent<bezier>().turnOn();
            }
        }



    }
}
