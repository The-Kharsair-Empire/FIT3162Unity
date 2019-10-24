/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// FileName: PointerListener.cs
// FileType: C#
// Author: Hardy Pei
// Description: This file implements the listener on the controller together with its pointer to listen to user inputs
//              and call the corresponding interaction functions attached to the target of the pointer.
// Last modified on: 23/10/2019
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace VRTK.Examples
{
    using System;
    using UnityEngine;
    using VRTK.Highlighters;
    using nodeTemplate;

    public class PointerListener : MonoBehaviour
    {
        public VRTK_DestinationMarker pointer;
        // refers to the pointer (laser) of the controller
        public GameObject controller;
        // refers to the monitor object in the Unity environment
        public GameObject Global;
        // refers to the Monitor object in this project to access some global variables
        private VRTK_InteractGrab interact;
        // refers to the VRTK_InteractGrab component attached to the pointer, to listen to some specific events
        private VRTK_ControllerEvents controllerEvent;
        // refers to the VRTK_ControllerEvents component attached to the pointer, to listen to some specific events
        public Color hoverColor = Color.cyan;
        // specific color defined, which will be applied to the object which is pointed to
        public Color selectColor = Color.yellow;
        // specific color defined, which will be applied to the object which is selected
        public bool logEnterEvent = true;
        // boolean variable determining if the pointer enters any objects
        public bool logHoverEvent = false;
        // boolean variable determining if the pointer hovers on any objects
        public bool logExitEvent = true;
        // boolean variable determining if the pointer touches any objects
        public bool logSetEvent = true;
        // boolean variable determining if the pointer triggers any events
        private VRTK_ControllerReference controllerReference;
        // reference to the current controller, which records the velocity of the controller


        private StateInteraction StateInteraction;
        // script attached to the states on the country map
        private GameObject theTarget;
        // the target of the pointer
        private Rigidbody rigi;
        // rigidbody component attached to the target
        private Vector3 controllerSpeed;
        // the speed of the controller as a vector
        private float impactMagnifier = 120f;
        // magnifier of the speed
        private float speedMagnitude = 0f;
        // magnitude of the speed
        private string targetLabel;
        // the label of the target, worked as a tag
        private Monitor GlobalMonitor;
        // script attached to the monitor in the system
        public GameObject instructionCanvas;
        // canvas object worked as textbox to show the instructions
        private Instruction instructionScript;
        // script attached to the canvas to change the description in the textbox

        /// <summary>
        /// Enable the listeners on both controller and pointer to all events
        /// </summary>
        protected virtual void OnEnable()
        {
            pointer = (pointer == null ? GetComponent<VRTK_DestinationMarker>() : pointer);
            // find the activated pointer
            if (pointer != null)
            {
                controllerEvent = pointer.GetComponent<VRTK_ControllerEvents>();
                GlobalMonitor = Global.GetComponent<Monitor>();
                controllerEvent.TouchpadPressed += ControllerEvent_TouchpadPressed;
                // event that touchpad is pressed
                controllerEvent.GripClicked += ControllerEvent_GripClicked;
                // event that grip button is pressed
                interact = pointer.GetComponent<VRTK_InteractGrab>();
                interact.GrabButtonReleased += Interact_GrabButtonReleased;
                // event that grab button is released
                pointer.DestinationMarkerEnter += DestinationMarkerEnter;
                // event that pointer enters an object
                pointer.DestinationMarkerHover += DestinationMarkerHover;
                // event that pointer hovers on an object
                pointer.DestinationMarkerExit += DestinationMarkerExit;
                // event that pointer exits an object
                pointer.DestinationMarkerSet += DestinationMarkerSet;
                // event that the destination marker is active in the scene to determine the last destination 
                instructionScript = instructionCanvas.GetComponent<Instruction>();
            }
            else
            {
                // print out the error if the pointer cannot be found
                VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTKExample_PointerObjectHighlighterActivator", "VRTK_DestinationMarker", "the Controller Alias"));
            }
        }

        /// <summary>
        /// Disable all the listeners on the pointer to their events
        /// </summary>
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

        /// <summary>
        /// when users click the grip button, call the copyArc function attached to system monitor to generate the minimap with the arc
        /// </summary>
        /// <param name="sender">VRTK predefined variable, not used in this project</param>
        /// <param name="e">VRTK predefined variable, not used in this project</param>
        private void ControllerEvent_GripClicked(object sender, ControllerInteractionEventArgs e)
        {

            GlobalMonitor.copyArc(controller.transform.position);
        }

        /// <summary>
        /// This function is called when users release the grab button, it will destroy the target object if the speed of the controller
        /// is high enough
        /// </summary>
        /// <param name="sender">VRTK predefined variable, not used in this project</param>
        /// <param name="e">VRTK predefined variable, not used in this project</param>
        private void Interact_GrabButtonReleased(object sender, ControllerInteractionEventArgs e)
        {
            if (targetLabel == "O_State" || targetLabel == "D_State" || targetLabel == "Arc")
               // deletion only applied to origin state pop-up, destination state pop-up, or miniArc pop-up
            {
                controllerSpeed = VRTK_DeviceFinder.GetControllerVelocity(controllerReference) * impactMagnifier;
                speedMagnitude = controllerSpeed.magnitude;
                // get the speed of the controller
                rigi = theTarget.GetComponent<Rigidbody>();
                //Debug.Log(speedMagnitude);
                if (speedMagnitude > 80)
                {
                    // if large enough
                    rigi.isKinematic = false;
                    rigi.useGravity = true;
                    rigi.AddForce(controllerSpeed);
                    // change the state so the object maintain the gravity and speed after being released
                    Destroy(theTarget, 2);
                    if (targetLabel == "O_State")
                    {
                        // if the origin state is deleted
                        instructionScript.changeText(0);
                        // change the descriptions of the instruction

                        // clear OD links attached to Origin node, and turn off nodes on country map
                        GlobalMonitor.hideArcs(GlobalMonitor.o_node);
                        GlobalMonitor.o_node.GetComponent<MeshRenderer>().enabled = false;
                        
                        GlobalMonitor.origin = null;
                        GlobalMonitor.o_node = null;
                        // clear the global variables in Monitor
                        if (GlobalMonitor.d_node != null)
                        {
                            // delete the destination node if it exists
                            GlobalMonitor.d_node.GetComponent<MeshRenderer>().enabled = false;
                            GlobalMonitor.d_node = null;
                        }
                        if (GlobalMonitor.destination != null)
                        {
                            // throw away and delete the destination state if it exists
                            rigi = GlobalMonitor.destination.GetComponent<Rigidbody>();
                            rigi.isKinematic = false;
                            rigi.useGravity = true;
                            Destroy(GlobalMonitor.destination, 2);
                            GlobalMonitor.destination = null;
                        }
                        GlobalMonitor.origin_text = null;
                        GlobalMonitor.destination_text = null;
                        // clear the textboxes attached to the origin and destination nodes
                    }
                    else if (targetLabel == "D_State")
                    {
                        // if the destination state is deleted
                        // change the descriptions of the instruction
                        instructionScript.changeText(2);
                        Transform o = GlobalMonitor.o_node.transform;
                        if (GlobalMonitor.d_node != null)
                        {
                            // delete the destination node if it exists

                            GlobalMonitor.d_node.GetComponent<MeshRenderer>().enabled = false;
                            GlobalMonitor.d_node = null;
                        }
                      
                        GlobalMonitor.destination = null;

                        foreach(Transform bezierContainer in o)
                        {
                            // turn on all arcs from the origin node
                            bezierContainer.GetComponent<bezier>().turnOn();
                        }

                        GlobalMonitor.destination_text = null;
                        // clear the textbox attched to the destination node
                        // turn on all bezier curves on origin node, and all destination nodes on country map
                    }
                    else if (targetLabel == "Arc")
                    {
                        // if the minimap is deleted
                        GlobalMonitor.clearArc();
                    }

                    Transform US = GameObject.Find("/America").transform;

                    if (targetLabel == "O_State" || targetLabel == "D_State")
                    {
                        foreach (Transform eachState in US)
                        {
                            // make states on the country map interactable again
                            eachState.GetComponent<StateInteraction>().interactable = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The function is called when users press the touchpad button. This function will trigger the country map, origin node,
        /// or destination node interaction
        /// </summary>
        /// <param name="sender">VRTK predefined variable, not used in this project</param>
        /// <param name="e">VRTK predefined variable, not used in this project</param>
        private void ControllerEvent_TouchpadPressed(object sender, ControllerInteractionEventArgs e)
        {
            if (theTarget != null)
            {
                // make sure the target exists
                if (targetLabel == "Country")
                {
                    // when the target is a state on the country map
                    // trigger the interaction of the country map
                    StateInteraction = theTarget.GetComponent<StateInteraction>();
                    StateInteraction.myInteract();
                }

                if(targetLabel == "OStateNode" && theTarget.GetComponent<MeshRenderer>().enabled == true)
                {
                    // if the target is a valid node on the origin state
                    if (GlobalMonitor.origin_text != null)
                    {
                        Destroy(GlobalMonitor.origin_text);
                    }
                    // destroy the old textbox
                    // trigger the interaction
                    theTarget.GetComponent<ONodeInteraction>().myInteract();
                    // create a new textbox
                    GlobalMonitor.origin_text = theTarget.GetComponent<Node>().showFloatingText( "origin_info");
                }

                if(targetLabel == "DStateNode" && theTarget.GetComponent<MeshRenderer>().enabled == true)
                {
                    // if the target is a valid node on the destination state
                    if (GlobalMonitor.destination_text != null)
                    {
                        Destroy(GlobalMonitor.destination_text);
                    }
                    // destroy the old textbox
                    // trigger the interaction
                    theTarget.GetComponent<DNodeInteraction>().myInteract();
                    Transform US = GameObject.Find("/America").transform;
                    // create a new textbox
                    GlobalMonitor.destination_text = theTarget.GetComponent<Node>().showFloatingText( "destination_info");

                    // disable all states on the country map
                    foreach (Transform eachState in US)
                    {
                        eachState.GetComponent<StateInteraction>().interactable = false;
                    }
                }
            }
        }

        /// <summary>
        /// Emitted when a collision with another game object has occurred.
        /// </summary>
        /// <param name="sender">VRTK predefined variable, not used in this project</param>
        /// <param name="e">event arguments contain a reference to the target</param>
        protected virtual void DestinationMarkerEnter(object sender, DestinationMarkerEventArgs e)
        {
            controllerReference = VRTK_ControllerReference.GetControllerReference(controller);
            theTarget = e.target.gameObject;
            // store the target as a variable
            checkLabel labelComponent = theTarget.GetComponent<checkLabel>();
            if (labelComponent != null)
            {
                targetLabel = labelComponent.getLabel();
            }
            // get the label if it has
            ToggleHighlight(e.target, hoverColor);
            // apply the color to the target
        }

        /// <summary>
        /// Emitted when a collision with a game object is kept.
        /// </summary>
        /// <param name="sender">VRTK predefined variable, not used in this project</param>
        /// <param name="e">VRTK predefined variable, not used in this project</param>
        private void DestinationMarkerHover(object sender, DestinationMarkerEventArgs e)    // unknown trigger
        {
        }

        /// <summary>
        /// Emitted when the collision with the other game object finishes.
        /// </summary>
        /// <param name="sender">VRTK predefined variable, not used in this project</param>
        /// <param name="e">event arguments contain a reference to the target</param>
        protected virtual void DestinationMarkerExit(object sender, DestinationMarkerEventArgs e)
        {
            // clear the color of the target
            ToggleHighlight(e.target, Color.clear);
            controllerReference = null;
            theTarget = null;
            targetLabel = null;
            // clear the reference to the target
        }

        /// <summary>
        /// Emitted when the destination marker is active in the scene to determine the last destination position(useful for selecting and teleporting).        /// </summary>
        /// <param name="sender">VRTK predefined variable, not used in this project</param>
        /// <param name="e">event arguments contain a reference to the target</param>
        protected virtual void DestinationMarkerSet(object sender, DestinationMarkerEventArgs e)
        {
            // apply a different color to the target
            ToggleHighlight(e.target, selectColor);
        }

        /// <summary>
        /// apply "color" to "target" to highlight the target
        /// </summary>
        /// <param name="target"></param>
        /// <param name="color"></param>
        protected virtual void ToggleHighlight(Transform target, Color color)
        {
            if (targetLabel != "State" && (targetLabel == "Country" && theTarget.GetComponent<StateInteraction>().interactable == true))
            {
               // only highlight the states on the country map
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

        /// <summary>
        /// VRTK predefined function for testing. Not used in this project.
        /// </summary>
        protected virtual void DebugLogger(uint index, string action, Transform target, RaycastHit raycastHit, float distance, Vector3 tipPosition)
        {
            string targetName = (target ? target.name : "<NO VALID TARGET>");
            string colliderName = (raycastHit.collider ? raycastHit.collider.name : "<NO VALID COLLIDER>");
            VRTK_Logger.Info("Controller on index '" + index + "' is " + action + " at a distance of " + distance + " on object named [" + targetName + "] on the collider named [" + colliderName + "] - the pointer tip position is/was: " + tipPosition);
        }
    }
}