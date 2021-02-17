/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// FileName: PointerListener.cs
// FileType: C#
// Description: This file defines an attribute 'label' for objects as the identifiers and provides them with getter and setter 
//              functionalities.
// Last modified on: 23/10/2019
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkLabel : MonoBehaviour
{
    public string label;
    // the label defined for an object

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getLabel()
    {
        // get the label of the object
        return label;
    }

    public void setLabel(string l)
    {
        // set the label of the object to l
        label = l;
    }
}