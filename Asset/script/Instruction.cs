///////////////////////////////////////////////////////////////////////////////////////////
//FileName: Instruction.cs
//FileType: visual C# Source File
//Author: Stark C.
//Description: sets text to be displayed in the instructions panel
//Last modified on: 23/10/19
//////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Examples;

public class Instruction : MonoBehaviour
{
   // list of all text to be displayed in the instruction panel. current display is pulled from this
    public string[] instructions = {"Select origin state", "1) Select origin node or\n2) Re-select origin state", "1) Select destination state\n2) Re-select origin node",
    "1) Select destination node\n2) Re-select destination state", "1) Click Grip button to show information\n2) Re-select destination node", ""};
    
    /// <summary>
    /// sets initial value of text to have in instruction box
    /// </summary>
    private void Start()
    {
        transform.GetChild(1).GetComponent<TextMesh>().text = instructions[0];
    }

    /// <summary>
    /// changes/ updates the text in the instruction box as the user progresses through the program
    /// </summary>
    /// <param name="int_i">determines the text to display, direct reference to instructions[] </param>
    public void changeText(int int_i)
    {
        transform.GetChild(1).GetComponent<TextMesh>().text = instructions[int_i];
    }

}
