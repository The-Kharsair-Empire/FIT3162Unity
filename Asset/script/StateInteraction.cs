///////////////////////////////////////////////////////////////////////////////////////////
//FileName: StateInteraction.cs
//FileType: visual C# Source File
//Author: Stark C.
//Description: applies functionalities to states, on both country map and on pop-up states
//Last modified on: 23/10/19
//////////////////////////////////////////////////////////////////////////////////////////

namespace VRTK.Examples
{
    using UnityEngine;
    using VRTK.Highlighters;
    using System.Collections;
    using System.Collections.Generic;
    using nodeTemplate;
    using ArcTemplate;

    public class StateInteraction : VRTK_InteractableObject
    {
        public Transform spawnPos_o, spawnPos_d; // positions that origin and destination states will spawn at by default
        private bool isOrigin;  // determines whether current state pop-up is origin or destination
        public Dictionary<string, List<Arc>> arc_info;  // dictionary containing all information of the arcs that come from state this script is attached to
        public Dictionary<string, GameObject> slic_nodeInfo_Pair; // dictionary containing list of SLIC, gameObject pairs where gameObject is node on country map
        public GameObject Global; // empty game object that stores the monitor script
        public string state_name;   // name of state this script is attached to 
        public bool interactable;   // determines whether this state is interactable
        public Instruction inScript;    // instruction script that will update the text on the instruction panel as the user interacts with the program

        /// <summary>
        /// activates this script when it is attached to state objects
        /// </summary>
        /// <param name="Global"> empty game object that stores the monitor script. </param>
        /// <param name="spawnPos_o"> default spawn position of origin state pop-up. </param>
        /// <param name="spawnPos_d"> default spawn position of destination state pop-up. </param>
        /// <param name="arc_info"> dicitonary of info of arcs starting in state this script is attached to. </param>
        /// <param name="slic_nodeInfo_Pair"> dictionary containing list of SLIC, gameObject pairs where gameObject is node on country map. </param>
        /// <param name="inScript"> instruction script that will update the text on the instruction panel as the user interacts with the program. </param>
        public void activateStateInteraction(GameObject Global, Transform spawnPos_o, Transform spawnPos_d, Dictionary<string, List<Arc>> arc_info, Dictionary<string, GameObject> slic_nodeInfo_Pair, Instruction inScript)
        {
            this.spawnPos_o = spawnPos_o;
            this.spawnPos_d = spawnPos_d;
            this.arc_info = arc_info;
            this.Global = Global;
            this.slic_nodeInfo_Pair = slic_nodeInfo_Pair;
            state_name = gameObject.name;
            gameObject.AddComponent<checkLabel>().setLabel("Country");
            interactable = true;
            this.inScript = inScript;
        }

        /// <summary>
        /// determines what happens when you click on a state
        /// </summary>
        public void myInteract()
        {
            GameObject o_state = Global.GetComponent<Monitor>().origin;
            GameObject d_state = Global.GetComponent<Monitor>().destination;
            GameObject o_node = Global.GetComponent<Monitor>().o_node;
            GameObject d_node = Global.GetComponent<Monitor>().d_node;

            // if this is the first state pop-up (origin state)
            if (o_node == null)
            {
                inScript.changeText(1);
                
                if (o_state != null)
                {
                    Destroy(o_state);
                }
                GameObject O = Instantiate(gameObject, spawnPos_o.position, Quaternion.identity); 
                O.transform.localScale = transform.localScale;
                O.transform.localScale *= transform.parent.localScale.x * 3;
                O.transform.rotation = Quaternion.Euler(0f, -50f, 0f);
                Destroy(O.GetComponent<StateInteraction>());
                Global.GetComponent<Monitor>().origin = O;

                int childnum = O.transform.childCount; 

                // create nodes on the pop-up
                for (int i = 0; i < childnum-1; ++i)
                {
                    Transform child = O.transform.GetChild(i);
                    child.gameObject.GetComponent<MeshRenderer>().enabled = true; 
                    child.GetComponent<Node>().cor_node = transform.GetChild(i).gameObject;
                    child.localScale /= 1.5f; // node size

                    child.gameObject.AddComponent<ONodeInteraction>().activateNodeInteraction(slic_nodeInfo_Pair, Global, inScript);
                    List<Arc> destinations;
                    if (arc_info.TryGetValue(child.GetComponent<Node>().slic_code, out destinations))
                    {
                        child.gameObject.GetComponent<ONodeInteraction>().addDestInfo(destinations);
                    }
                }

                O.GetComponent<checkLabel>().setLabel("O_State");
                Destroy(O.transform.GetChild(O.transform.childCount - 1).gameObject);
                Destroy(O.GetComponent<VRTK_OutlineObjectCopyHighlighter>());
                O.AddComponent<VRTK_InteractableObject>().isGrabbable = true;
                O.GetComponent<MeshCollider>().convex = true;
                myHighlight();
            }

            // working on second state pop-up (destination state)
            else if(d_node == null)
            {
                inScript.changeText(3);
                if (d_state != null)
                {
                    Destroy(d_state);
                }
                Global.GetComponent<Monitor>().o_node.SetActive(false);
                GameObject D = Instantiate(gameObject, spawnPos_d.position, Quaternion.identity); 
                D.transform.localScale = transform.localScale;
                D.transform.localScale *= transform.parent.localScale.x * 3;
                D.transform.rotation = Quaternion.Euler(0f, 50f, 0f);
                Destroy(D.GetComponent<StateInteraction>());
                Global.GetComponent<Monitor>().destination = D;

                Global.GetComponent<Monitor>().o_node.SetActive(true);

                int childnum = D.transform.childCount; 

                // creating nodes on pop-up map
                for (int i = 0; i < childnum - 1; ++i)
                {
                    Transform child = D.transform.GetChild(i);
                    child.GetComponent<Node>().cor_node = transform.GetChild(i).gameObject;
                    child.localScale /= 1.5f; // node size
                    child.gameObject.AddComponent<DNodeInteraction>().activateNodeInteraction(slic_nodeInfo_Pair, Global, inScript);
                   
                }

                D.GetComponent<checkLabel>().setLabel("D_State");
                Destroy(D.transform.GetChild(D.transform.childCount - 1).gameObject);
                Destroy(D.GetComponent<VRTK_OutlineObjectCopyHighlighter>());
                D.AddComponent<VRTK_InteractableObject>().isGrabbable = true;
                D.GetComponent<MeshCollider>().convex = true;
                myHighlight();
            }
            
        }

        
        /// <summary>
        /// highlights hovered state on the counrty map
        /// </summary>
        protected void myHighlight()
        {
            VRTK_BaseHighlighter highligher = transform.GetComponentInChildren<VRTK_BaseHighlighter>();
            highligher.Initialise();
            highligher.Highlight(Color.yellow);
        }

        /// <summary>
        /// implemented by VRTK library
        /// </summary>
        protected override void Update()
        {
            base.Update();
        }
    }
}

