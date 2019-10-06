using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcTemplate;


namespace nodeTemplate {
    public class Node: MonoBehaviour
    {
        public string slic_code;
        public string slic_name;
        public string slic_type;
        public string slic_state;
        public string optyp;
        public string sort;
        public int cap;
        public float span;
        public float start;
        //public Vector3 distFromStatePivot;
        //public bool isOnState;
        public GameObject cor_node = null;
        public List<Arc> destinations;

        public void setNodeInfo(string slic_code, string slic_name, string slic_type, string slic_state, string optyp, string sort, int cap, float span, float start/*, bool isOnState*/)
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
            //this.isOnState = isOnState;
        }

        public void setDest(List<Arc> destinations)
        {
            this.destinations = destinations;
        }
        public GameObject get_cor_node()
        {
            return cor_node;
        }

        public void set_cor_node(GameObject cor_node)
        {
            this.cor_node = cor_node;
        }
    }
}
