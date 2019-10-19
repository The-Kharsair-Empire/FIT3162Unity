using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Examples;

public class Instruction : MonoBehaviour
{
   
    public string[] instructions = {"Select origin state", "1) Select origin node or\n2) Re-select origin state", "1) Select destination state\n2) Re-select origin node",
    "1) Select destination node\n2) Re-select destination state", "1) Click Grip button to show information\n2) Re-select destination node", ""};

    // Start is called before the first frame update
    // Update is called once per frame

    private void Start()
    {
        transform.GetChild(1).GetComponent<TextMesh>().text = instructions[0];
    }

    public void changeText(int int_i)
    {
        transform.GetChild(1).GetComponent<TextMesh>().text = instructions[int_i];
    }

}
