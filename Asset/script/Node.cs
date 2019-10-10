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

        public GameObject FloatingTextPrefab;
        private GameObject textObject;
        public string info;


        

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
                sort + "\n capacity: " + cap + "\n span: " + span + "\n start time: " + start;
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




        //public void OnMouseEnter()
        //{
        //    showFloatingText();
        //}


        public void del()
        {
            Destroy(textObject);
            //text.GetComponent<TextMesh>().text = "";
        }

        public GameObject showFloatingText(string name)
        {
            textObject = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity);
            //textObject.transform.SetParent(null);
            textObject.GetComponent<TextMesh>().text = info; // replace with TextAsset text_file
            textObject.GetComponent<TextMesh>().color = Color.black;
            textObject.GetComponent<TextMesh>().fontSize = 100;
            textObject.transform.localScale = new Vector3(0.007f, 0.007f, 0.007f);
            //textObject.transform.Rotate(0, 0, 0, Space.World);
            //textObject.transform.rotation = Quaternion.Euler(transform.parent.rotation.x, transform.parent.rotation.y + 180f, transform.parent.rotation.z);
            //new Vector3(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y - 180, transform.parent.eulerAngles.z);
            textObject.transform.SetParent(transform.parent);  //follows state-map rotation
            textObject.name = name;
            return textObject;
        }

        public void Update()
        {
            if (textObject != null)
            {
                textObject.transform.position = transform.position;
                LookAtBackwards(Camera.main.transform.position);
                //textObject.transform.LookAt(Camera.main.transform, Vector3.back);
                //textObject.transform.rotation = Quaternion.Euler(textObject.transform.rotation.x, textObject.transform.rotation.y + 180f, textObject.transform.rotation.z);


                //Debug.Log(transform.parent.rotation.x);
                //Debug.Log(transform.parent.rotation.y);
                //Debug.Log(transform.parent.rotation.z);
                //textObject.transform.rotation = transform.parent.rotation;

                //textObject.transform.eulerAngles = new Vector3(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y - 180, transform.parent.eulerAngles.z);
            }


        }

        public void LookAtBackwards(Vector3 targetPos)
        {
            Vector3 offset = textObject.transform.position - targetPos;
            textObject.transform.LookAt(textObject.transform.position + offset);
        }
    }


}
