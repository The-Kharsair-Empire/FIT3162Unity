///////////////////////////////////////////////////////////////////////////////////////////
//FileName: Node.cs
//FileType: visual C# Source File
//Description: stores info the nodes contains, script to attach to spherical object in unity to make them nodes (facility representations)
//Last modified on: 23/10/19
//////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcTemplate;


namespace nodeTemplate {
    public class Node: MonoBehaviour
    {
        public string slic_code;    // SLIC code (from UPS data)
        public string slic_name;    // name of facility 
        public string slic_type;    // facility type
        public string slic_state;   // state the facility is located in
        public string optyp;        // operation type done at this facility
        public string sort;         // sort type
        public int cap;             // capacity of the facility
        public float span;          // running hours (number of hours open after start time)
        public float start;         // time the facility opens each day
        public GameObject cor_node = null;  // node on the country map that corresponds to this node (assuming it is on a pop-up map)
        public List<Arc> destinations;  // list of all destinations that can be reached from this node
        public GameObject FloatingTextPrefab;   // the game object text is written upon 
        private GameObject textObject;  // the text game object
        public string info; // the info to be displayed in the text box


        
        /// <summary>
        /// attaches the info given to the node
        /// </summary>
        /// <param name="slic_code"> SLIC code</param>
        /// <param name="slic_name">facility name</param>
        /// <param name="slic_type">facility type</param>
        /// <param name="slic_state">state the facility is in</param>
        /// <param name="optyp">operation type</param>
        /// <param name="sort">sort type</param>
        /// <param name="cap">capacity</param>
        /// <param name="span">number of hours open after start</param>
        /// <param name="start">start time the facility opens</param>
        /// <param name="FloatingTextPrefab">object the text is attached to</param>
        public void setNodeInfo(string slic_code, string slic_name, string slic_type, string slic_state, string optyp, string sort, int cap, float span, float start, GameObject FloatingTextPrefab/*, bool isOnState*/)
        {
            this.slic_code = slic_code;
            this.slic_name = slic_name;
            this.slic_type = slic_type;
            this.slic_state = slic_state;
            this.optyp = optyp;
            this.sort = sort;
            this.cap = cap;
            this.span = span/100;
            this.start = start/100;
            this.FloatingTextPrefab = FloatingTextPrefab;
            this.info = "SLIC code: " + slic_code + "\n facility name: " + slic_name + "\n facility type: " + slic_type + "\n optyp: " + optyp + "\n sort: " +
                sort + "\n capacity: " + cap + "\n span: " + span + "\n start time: " + start +"\n";
        }

        /// <summary>
        /// sets the destinations of this node
        /// </summary>
        /// <param name="destinations">list of destinations that can be reached</param>
        public void setDest(List<Arc> destinations)
        {
            this.destinations = destinations;
        }

        /// <summary>
        /// gets a reference to the node equivalent to this one on the country map
        /// </summary>
        /// <returns>equivalent node on the country map</returns>
        public GameObject get_cor_node()
        {
            return cor_node;
        }

        /// <summary>
        /// sets the node on the country map that is equivalent to this one
        /// </summary>
        /// <param name="cor_node">the corresponding node on the country map</param>
        public void set_cor_node(GameObject cor_node)
        {
            this.cor_node = cor_node;
        }

        /// <summary>
        /// deletes this node
        /// </summary>
        public void del()
        {
            Destroy(textObject);
        }

        /// <summary>
        /// creates and displays a text box containing the information of the node/ facility
        /// </summary>
        /// <param name="name"> name of the node/ facility</param>
        /// <returns></returns>
        public GameObject showFloatingText(string name)
        {
            textObject = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity);
            textObject.GetComponent<TextMesh>().text = info; 
            textObject.GetComponent<TextMesh>().color = Color.grey;
            textObject.GetComponent<TextMesh>().fontSize = 100;
            textObject.transform.localScale = new Vector3(0.007f, 0.007f, 0.007f);
            textObject.transform.SetParent(transform.parent);  //follows state-map rotation
            textObject.name = name;
            return textObject;
        }

        /// <summary>
        /// makes sure the text always faces the user
        /// </summary>
        public void Update()
        {
            if (textObject != null)
            {
                textObject.transform.position = transform.position;
                LookAtBackwards(Camera.main.transform.position);
            }
        }

        /// <summary>
        /// reverses the text direction, as the forward vector of the parent object faces away from the camera
        /// </summary>
        /// <param name="targetPos">the object (camera) we want the text to face</param>
        public void LookAtBackwards(Vector3 targetPos)
        {
            Vector3 offset = textObject.transform.position - targetPos;
            textObject.transform.LookAt(textObject.transform.position + offset);
        }
    }


}
