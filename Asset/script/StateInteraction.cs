

namespace VRTK.Examples
{
    using UnityEngine;
    using VRTK.Highlighters;
    using System.Collections;
    using System.Collections.Generic;
    using nodeTemplate;
    using ArcTemplate;

    // checkLabel???
    //children list of state & country map


    public class StateInteraction : VRTK_InteractableObject
    {
        //private aa rotator;

        

        public Transform spawnPos_o, spawnPos_d;
        private bool isOrigin;
        public Dictionary<string, List<Arc>> arc_info;
        public Dictionary<string, GameObject> slic_nodeInfo_Pair;
        public GameObject Global;
        public string state_name;
        public bool interactable;

        public void activateStateInteraction(GameObject Global, Transform spawnPos_o, Transform spawnPos_d, Dictionary<string, List<Arc>> arc_info, Dictionary<string, GameObject> slic_nodeInfo_Pair)
        {
            this.spawnPos_o = spawnPos_o;
            this.spawnPos_d = spawnPos_d;
            this.arc_info = arc_info;
            this.Global = Global;
            this.slic_nodeInfo_Pair = slic_nodeInfo_Pair;
            state_name = gameObject.name;
            gameObject.AddComponent<checkLabel>().setLabel("Country");
            interactable = true;
        }


        public void myInteract()
        {
            GameObject o_state = Global.GetComponent<Monitor>().origin;
            GameObject d_state = Global.GetComponent<Monitor>().destination;
            GameObject o_node = Global.GetComponent<Monitor>().o_node;
            GameObject d_node = Global.GetComponent<Monitor>().d_node;


          

            if (o_node == null)
            {
                
                if (o_state != null)
                {
                    Destroy(o_state);
                }
                GameObject O = Instantiate(gameObject, spawnPos_o.position, Quaternion.identity); //change rotation if needed;
                O.transform.localScale = transform.parent.localScale;
                O.transform.localScale *= 2;
                O.transform.rotation = Quaternion.Euler(0f, -50f, 0f);
                Destroy(O.GetComponent<StateInteraction>());
                Global.GetComponent<Monitor>().origin = O;

                 //where is the highlighting features in the statemap - assuming it is the last child in the hierarchy

                int childnum = O.transform.childCount; //make sure there are only nodes in their children hierachy of both state of the country map and its corresponding state map

                for (int i = 0; i < childnum-1; ++i)
                {
                    Transform child = O.transform.GetChild(i);
                    child.gameObject.GetComponent<MeshRenderer>().enabled = true; //remember to setInactive when destroying the state.
  
                    child.GetComponent<Node>().cor_node = transform.GetChild(i).gameObject;
                    //transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
                    child.localScale /= 1.5f; //resize node to make it bigger, adjust it
                                                                 //child.gameObject.AddComponent<>(); add interaction component, rigid body etc.
                                                                 //add node interaction script

                    child.gameObject.AddComponent<ONodeInteraction>().activateNodeInteraction(slic_nodeInfo_Pair, Global);
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
            else if(d_node == null)
            {
                if (d_state != null)
                {
                    Destroy(d_state);
                }
                Global.GetComponent<Monitor>().o_node.SetActive(false);
                GameObject D = Instantiate(gameObject, spawnPos_d.position, Quaternion.identity); //change rotation if needed;
                D.transform.localScale = transform.parent.localScale;

                D.transform.localScale *= 2;
                D.transform.rotation = Quaternion.Euler(0f, 50f, 0f);
                Destroy(D.GetComponent<StateInteraction>());
                Global.GetComponent<Monitor>().destination = D;

                Global.GetComponent<Monitor>().o_node.SetActive(true);

                //where is the highlighting features in the statemap - assuming it is the last child in the hierarchy

                int childnum = D.transform.childCount; //make sure there are only nodes in their children hierachy of both state of the country map and its corresponding state map

                for (int i = 0; i < childnum - 1; ++i)
                {
                    Transform child = D.transform.GetChild(i);
                    
                    
                     //remember to setInactive when destroying the state.

                    child.GetComponent<Node>().cor_node = transform.GetChild(i).gameObject;
                    //transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
                    child.localScale /= 1.5f; //resize node to make it bigger, adjust it
                                                                 //child.gameObject.AddComponent<>(); add interaction component, rigid body etc.
                                                                 //add node interaction script

                    child.gameObject.AddComponent<DNodeInteraction>().activateNodeInteraction(slic_nodeInfo_Pair, Global);
                   
                }

                D.GetComponent<checkLabel>().setLabel("D_State");
                Destroy(D.transform.GetChild(D.transform.childCount - 1).gameObject);
                Destroy(D.GetComponent<VRTK_OutlineObjectCopyHighlighter>());
                D.AddComponent<VRTK_InteractableObject>().isGrabbable = true;
                D.GetComponent<MeshCollider>().convex = true;
                myHighlight();
            }
            
            //if (myState == null)
            //{
            //    myState = Instantiate(gameObject, new Vector3(5, 5, 0), Quaternion.identity);
            //    Destroy(myState.GetComponent<stateTest>());
            //    myState.GetComponent<checkLabel>().setLabel("State");
            //    Destroy(myState.transform.GetChild(myState.transform.childCount - 1).gameObject);

            //    Destroy(myState.GetComponent<VRTK_OutlineObjectCopyHighlighter>());
            //    //myState.AddComponent<aa>();
            //    myState.AddComponent<aa>().visibility();
            //    myState.AddComponent<VRTK_InteractableObject>().isGrabbable = true;
            //    myState.GetComponent<MeshCollider>().convex = true;
            //    myHighlight();
            //}
            //else
            //{
            //    //myState.GetComponent<aa>().visibility();
            //    Destroy(myState);
            //    myState = null;
            //    myHighlight();
            //}
        }

        //public override void StartUsing(VRTK_InteractUse currentUsingObject = null)
        //{
        //    base.StartUsing(currentUsingObject);
        //    myState = Instantiate(gameObject, new Vector3(0, 5, 0), Quaternion.identity);
        //    myState.AddComponent<checkLabel>().setLabel("State");
        //    myState.AddComponent<aa>().visibility();
        //    //if (rotator == null)
        //    //    rotator = GameObject.Find("TheCube").GetComponent<aa>();
        //    //            rotator.Going = !rotator.Going;
        //    myHighlight();

        //}

        //public override void StopUsing(VRTK_InteractUse previousUsingObject = null, bool resetUsingObjectState = true)
        //{
        //    base.StopUsing(previousUsingObject, resetUsingObjectState);
        //    //if (rotator == null)
        //    //    rotator = GameObject.Find("TheCube").GetComponent<aa>();
        //    //            rotator.Going = !rotator.Going;
        //    myState.GetComponent<aa>().visibility();
        //    Destroy(myState);
        //    myState = null;
        //    myHighlight();
        //}

        protected void myHighlight()
        {
            VRTK_BaseHighlighter highligher = transform.GetComponentInChildren<VRTK_BaseHighlighter>();
            highligher.Initialise();
            highligher.Highlight(Color.yellow);
        }


        protected override void Update()
        {
            base.Update();
        }
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using nodetemplate;
//using arctemplate;

//public class StateInteraction : MonoBehaviour
//{
//    public Transform spawnPos_o, spawnPos_d;
//    private bool isOrigin;
//    public Dictionary<string, List<Arc>> arc_info;
//    public Dictionary<string, GameObject> slic_nodeInfo_Pair;
//    public GameObject preprocessor;

//    public void activateStateInteraction(GameObject preprocessor,Transform spawnPos_o, Transform spawnPos_d, Dictionary<string, List<Arc>> arc_info, Dictionary<string, GameObject> slic_nodeInfo_Pair)
//    {
//        this.spawnPos_o = spawnPos_o;
//        this.spawnPos_d = spawnPos_d;
//        this.arc_info = arc_info;
//        this.preprocessor = preprocessor;
//    }

//    public void OnMouseDown()
//    {
//        GameObject origin = preprocessor.GetComponent<Preprocessor>().origin;
//        if (origin == null) //replace if there existed a state map
//        {
//            Destroy(origin);
//        }
//        GameObject O = Instantiate(gameObject, spawnPos_o.position, Quaternion.identity); //change rotation if needed

//        preprocessor.GetComponent<Preprocessor>().origin = O;

//        Destroy(O.GetComponent<StateInteraction>()); //remove stateClick component in the state map


//        int childnum = transform.childCount; //make sure there are only nodes in their children hierachy of both state of the country map and its corresponding state map

//        for (int i = 0; i < childnum; ++i)
//        {
//            Transform child = O.transform.GetChild(i);
//            child.GetComponent<NodeInfoContainer>().getNodeInfo().cor_node = transform.GetChild(i).gameObject;
//            child.localScale += new Vector3(0.1f, 0.1f, 0.1f); //resize node to make it bigger, adjust it
//                                                               //child.gameObject.AddComponent<>(); add interaction component, rigid body etc.
//                                                               //add node interaction script
//            child.gameObject.AddComponent<NodeInteraction>().Activate(0, slic_nodeInfo_Pair);
//            List<Arc> destinations;
//            if (arc_info.TryGetValue(child.GetComponent<NodeInfoContainer>().getNodeInfo().slic_code, out destinations))
//            {
//                child.gameObject.AddComponent<NodeInteraction>().addDestInfo(destinations);
//            }
//        }
//    }

//    public void Update()
//    {





//if (Input.GetMouseButton(1)) //right controller
//{

//    this.isOrigin = false;

//    GameObject dest = GameObject.Find("proprocessor").GetComponent<Preprocessor>().destination;
//    if (dest == null) //replace if there existed a state map
//    {
//        Destroy(dest);
//    }
//    GameObject D = Instantiate(gameObject, spawnPos_d.position, Quaternion.identity); //change rotation if needed

//    GameObject.Find("proprocessor").GetComponent<Preprocessor>().destination = D;

//    Destroy(D.GetComponent<StateInteraction>()); //remove stateClick component in the state map


//    int childnum = transform.childCount; //make sure there are only nodes in their children hierachy of both state of the country map and its corresponding state map

//    for (int i = 0; i < childnum; ++i)
//    {
//        Transform child = D.transform.GetChild(i);
//        child.GetComponent<NodeInfoContainer>().getNodeInfo().cor_node = transform.GetChild(i).gameObject;
//        child.localScale += new Vector3(0.1f, 0.1f, 0.1f); //resize node to make it bigger, adjust it
//        //child.gameObject.AddComponent<>(); add interaction component, rigid body etc.
//        //add node interaction script
//        child.gameObject.AddComponent<NodeInteraction>().Activate(1, slic_nodeInfo_Pair);


//    }
//}
//    }



//}
