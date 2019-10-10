namespace VRTK.Examples
{
    using System;
    using UnityEngine;
    using VRTK.Highlighters;
    using nodeTemplate;

    public class PointerListener : MonoBehaviour
    {
        public VRTK_DestinationMarker pointer;
        public GameObject controller;
        public GameObject Global;
        private VRTK_InteractGrab interact;
        private VRTK_ControllerEvents controllerEvent;
        public Color hoverColor = Color.cyan;
        public Color selectColor = Color.yellow;
        public bool logEnterEvent = true;
        public bool logHoverEvent = false;
        public bool logExitEvent = true;
        public bool logSetEvent = true;
        private VRTK_ControllerReference controllerReference;
        //private second second;
        //private aa aa;
        private StateInteraction StateInteraction;
        private GameObject theTarget;
        private Rigidbody rigi;
        private Vector3 controllerSpeed;
        private float impactMagnifier = 120f;
        private float speedMagnitude = 0f;
        private string targetLabel;
        private Monitor GlobalMonitor;
        public GameObject instructionCanvas;
        private Instruction instructionScript;

        protected virtual void OnEnable()
        {
            pointer = (pointer == null ? GetComponent<VRTK_DestinationMarker>() : pointer);
            if (pointer != null)
            {
                controllerEvent = pointer.GetComponent<VRTK_ControllerEvents>();
                GlobalMonitor = Global.GetComponent<Monitor>();
                controllerEvent.TouchpadPressed += ControllerEvent_TouchpadPressed;
                controllerEvent.GripClicked += ControllerEvent_GripClicked;
                interact = pointer.GetComponent<VRTK_InteractGrab>();
                interact.GrabButtonReleased += Interact_GrabButtonReleased;
                pointer.DestinationMarkerEnter += DestinationMarkerEnter;
                pointer.DestinationMarkerHover += DestinationMarkerHover;
                pointer.DestinationMarkerExit += DestinationMarkerExit;
                pointer.DestinationMarkerSet += DestinationMarkerSet;
                instructionScript = instructionCanvas.GetComponent<Instruction>();
            }
            else
            {
                VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTKExample_PointerObjectHighlighterActivator", "VRTK_DestinationMarker", "the Controller Alias"));
            }
        }

        protected virtual void OnDisable()
        {
            if (pointer != null)
            {
                //controllerEvent.TouchpadPressed -= ControllerEvent_TouchpadPressed;
                interact.GrabButtonReleased -= Interact_GrabButtonReleased;
                pointer.DestinationMarkerEnter -= DestinationMarkerEnter;
                pointer.DestinationMarkerHover -= DestinationMarkerHover;
                pointer.DestinationMarkerExit -= DestinationMarkerExit;
                pointer.DestinationMarkerSet -= DestinationMarkerSet;
            }
        }

        private void ControllerEvent_GripClicked(object sender, ControllerInteractionEventArgs e)
        {

            GlobalMonitor.copyArc(controller.transform.position);
        }

        private void Interact_GrabButtonReleased(object sender, ControllerInteractionEventArgs e)
        {
            if (targetLabel == "O_State" || targetLabel == "D_State" || targetLabel == "Arc")
            {
                controllerSpeed = VRTK_DeviceFinder.GetControllerVelocity(controllerReference) * impactMagnifier;
                speedMagnitude = controllerSpeed.magnitude;
                rigi = theTarget.GetComponent<Rigidbody>();
                //Debug.Log(speedMagnitude);
                if (speedMagnitude > 80)
                {
                    rigi.isKinematic = false;
                    rigi.useGravity = true;
                    rigi.AddForce(controllerSpeed);
                    Destroy(theTarget, 2);
                    if (targetLabel == "O_State")
                    {
                        instructionScript.changeText(0);
                        // clear OD links attached to Origin node, and turn off nodes on country map
                        GlobalMonitor.hideArcs(GlobalMonitor.o_node);
                        GlobalMonitor.o_node.GetComponent<MeshRenderer>().enabled = false;
                        
                        GlobalMonitor.origin = null;
                        GlobalMonitor.o_node = null;
                        if (GlobalMonitor.d_node != null)
                        {
                            GlobalMonitor.d_node.GetComponent<MeshRenderer>().enabled = false;
                            GlobalMonitor.d_node = null;
                        }
                        if (GlobalMonitor.destination != null)
                        {
                            rigi = GlobalMonitor.destination.GetComponent<Rigidbody>();
                            rigi.isKinematic = false;
                            rigi.useGravity = true;
                            Destroy(GlobalMonitor.destination, 2);
                            GlobalMonitor.destination = null;
                        }
                        GlobalMonitor.origin_text = null;
                        GlobalMonitor.destination_text = null;
                    }
                    else if (targetLabel == "D_State")
                    {
                        instructionScript.changeText(2);
                        Transform o = GlobalMonitor.o_node.transform;
                        if (GlobalMonitor.d_node != null)
                        {
                            GlobalMonitor.d_node.GetComponent<MeshRenderer>().enabled = false;
                            GlobalMonitor.d_node = null;
                        }
                      
                        GlobalMonitor.destination = null;

                        foreach(Transform bezierContainer in o)
                        {
                            bezierContainer.GetComponent<bezier>().turnOn();
                        }

                        GlobalMonitor.destination_text = null;
                        // turn on all bezier curves on origin node, and all destination nodes on country map
                    }
                    else if (targetLabel == "Arc")
                    {
                        GlobalMonitor.clearArc();
                    }

                    Transform US = GameObject.Find("/America").transform;

                    foreach (Transform eachState in US)
                    {
                        eachState.GetComponent<StateInteraction>().interactable = true;
                    }


                }
            }
        }

        private void ControllerEvent_TouchpadPressed(object sender, ControllerInteractionEventArgs e)
        {
            if (theTarget != null)
            {
                //Debug.Log(theTarget.GetComponent<MeshRenderer>().enabled);
                
                if (targetLabel == "Country")
                {
                    StateInteraction = theTarget.GetComponent<StateInteraction>();
                    StateInteraction.myInteract();
                }

                if(targetLabel == "OStateNode" && theTarget.GetComponent<MeshRenderer>().enabled == true)
                {
                    if (GlobalMonitor.origin_text != null)
                    {
                        Destroy(GlobalMonitor.origin_text);
                    }
                    theTarget.GetComponent<ONodeInteraction>().myInteract();
                    GlobalMonitor.origin_text = theTarget.GetComponent<Node>().showFloatingText( "origin_info");
                }

                if(targetLabel == "DStateNode" && theTarget.GetComponent<MeshRenderer>().enabled == true)
                {
                    if (GlobalMonitor.destination_text != null)
                    {
                        Destroy(GlobalMonitor.destination_text);
                    }
                    theTarget.GetComponent<DNodeInteraction>().myInteract();

                    Transform US = GameObject.Find("/America").transform;
                    GlobalMonitor.destination_text = theTarget.GetComponent<Node>().showFloatingText( "destination_info");

                    foreach (Transform eachState in US)
                    {
                        eachState.GetComponent<StateInteraction>().interactable = false;
                    }
                }
            }
        }

        protected virtual void DestinationMarkerEnter(object sender, DestinationMarkerEventArgs e)
        {
            //if (logEnterEvent)
            //{
            //    DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER ENTER", e.target, e.raycastHit, e.distance, e.destinationPosition);
            //}
            
            controllerReference = VRTK_ControllerReference.GetControllerReference(controller);
            theTarget = e.target.gameObject;
            
            checkLabel labelComponent = theTarget.GetComponent<checkLabel>();
            if (labelComponent != null)
            {
                targetLabel = labelComponent.getLabel();
            }

            //if (targetLabel == "OStateNode") {
            //    theTarget.GetComponent<Node>().showFloatingText(controller.transform, "origin_info");
            //}

            //if (targetLabel == "DStateNode")
            //{
            //    theTarget.GetComponent<Node>().showFloatingText(controller.transform, "destination_info");
            //}




            ToggleHighlight(e.target, hoverColor);
        }

        private void DestinationMarkerHover(object sender, DestinationMarkerEventArgs e)    // unknown trigger
        {
            //if (logHoverEvent)
            //{
            //    DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER HOVER", e.target, e.raycastHit, e.distance, e.destinationPosition);
            //}
        }

        protected virtual void DestinationMarkerExit(object sender, DestinationMarkerEventArgs e)
        {

            ToggleHighlight(e.target, Color.clear);
            //if (logExitEvent)
            //{
            //    DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER EXIT", e.target, e.raycastHit, e.distance, e.destinationPosition);
            //}
            //if (targetLabel == "OStateNode" || targetLabel == "DStateNode")
            //{
            //    theTarget.GetComponent<Node>().del();
            //}
            controllerReference = null;
            theTarget = null;
            targetLabel = null;
        }

        protected virtual void DestinationMarkerSet(object sender, DestinationMarkerEventArgs e)
        {
            ToggleHighlight(e.target, selectColor);
            //if (logSetEvent)
            //{
            //    DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER SET", e.target, e.raycastHit, e.distance, e.destinationPosition);
            //}
        }

        protected virtual void ToggleHighlight(Transform target, Color color)
        {
            if (targetLabel != "State" && (targetLabel == "Country" && theTarget.GetComponent<StateInteraction>().interactable == true))
            {
               
                VRTK_BaseHighlighter highligher = (target != null ? target.GetComponentInChildren<VRTK_BaseHighlighter>() : null);
                if (highligher != null)
                {
                    highligher.Initialise();
                    if (color != Color.clear)
                    {
                        highligher.Highlight(color);
                    }
                    else
                    {
                        highligher.Unhighlight();
                    }
                }
            }
        }

        protected virtual void DebugLogger(uint index, string action, Transform target, RaycastHit raycastHit, float distance, Vector3 tipPosition)
        {
            string targetName = (target ? target.name : "<NO VALID TARGET>");
            string colliderName = (raycastHit.collider ? raycastHit.collider.name : "<NO VALID COLLIDER>");
            VRTK_Logger.Info("Controller on index '" + index + "' is " + action + " at a distance of " + distance + " on object named [" + targetName + "] on the collider named [" + colliderName + "] - the pointer tip position is/was: " + tipPosition);
        }
    }
}