//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class NodeTest : MonoBehaviour
//{
//    // Start is called before the first frame update
//    public TextAsset node_data;
//    public GameObject nodePrefab;
//    private bool checkStart = true;
//    private List<GameObject> nodeList = new List<GameObject>();
    
//    // Start is called before the first frame update
//    void Start()
//    {
//        Debug.Log(node_data);
//        string[] rows = node_data.text.Split(new char[] { '\n' });
//        for (int i = 1; i < rows.Length - 1; i++)
//        {
//            string[] row = rows[i].Split(new char[] { ',' });
//            GameObject a = Instantiate(nodePrefab, new Vector3(float.Parse(row[10])*100, float.Parse(row[11])*100, 0f), Quaternion.identity, transform);
//            nodeList.Add(a);
//        }
//    }

//    private void Update()
//    {
//        if(checkStart)
//        {
//            checkStart = false;
//            transform.position = new Vector3(-21.22162f, 2.137f, -14.99f);
//            //transform.position = new Vector3(-21.22162f, 2.137f, -6.2067f);
//            transform.rotation = Quaternion.Euler(0, 180, 49.105f);
//            transform.localScale = new Vector3(0.07155591f, 0.0720473f, 0.07051525f);
//            Debug.Log("Hello");
//            foreach (GameObject each in nodeList)
//            {
//                each.transform.parent = GameObject.Find("123123123").transform;
//            }
//            Debug.Log("original parent exploded");

//        }
            
        
//    }

//}
