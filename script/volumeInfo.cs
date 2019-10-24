///////////////////////////////////////////////////////////////////////////////////////////
//FileName: volumeInfo.cs
//FileType: visual C# Source File
//Author: Stark C. and Daniel S.
//Description: creates the text box to display node information
//Last modified on: 23/10/19
//////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class volumeInfo : MonoBehaviour
{
    public GameObject FloatingTextPrefab; // the prefab object that text is written upon
    private GameObject textObject;  // the actual text as an object
    private float volume;   // volume of packages 
    private GameObject textPos; // the default position of the text when it spawns

    string o_info;  // text defining the info of the origin node (facility)
    string d_info;  // text defining the info of the destination node (facility)

    /// <summary>
    /// creates the pop-up text
    /// </summary>
    /// <param name="volume"> volume that the bezier curve this is attached to represents </param>
    /// <param name="FloatingTextPrefab"> the object the text object is attached to </param>
    /// <param name="textPos"> the default spwan position of the text </param>
    /// <param name="o_info"> info of the origin node </param>
    /// <param name="d_info"> info of the destination node </param>
    public void activate(float volume, GameObject FloatingTextPrefab, GameObject textPos,  string o_info, string d_info)
    {
        this.FloatingTextPrefab = FloatingTextPrefab;
        this.volume = volume;
        this.textPos = textPos;

        this.o_info = o_info;
        this.d_info = d_info;
        showFloatingText();
    }

    /// <summary>
    /// turns the given raw text into a text object unity can display
    /// </summary>
    /// <returns> returns the text object to be displayed </returns>
    public GameObject showFloatingText()
    {
        textObject = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity);
        //textObject.transform.SetParent(null);
        textObject.transform.position = textPos.transform.position;
        textObject.GetComponent<TextMesh>().text = "\n\n" + "Arc volume: " + volume + "\n\nOrigin Information\n"+ o_info + "\nDestination Information\n" + d_info;// replace with TextAsset text_file
        textObject.GetComponent<TextMesh>().color = Color.grey;
        textObject.GetComponent<TextMesh>().fontSize = 100;
        textObject.transform.localScale = new Vector3(0.007f, 0.007f, 0.007f);
        textObject.transform.SetParent(transform);  //follows state-map rotation
        return textObject;
    }

    /// <summary>
    /// makes sure the text always faces the user even as they rotate the object it is bound to
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
    /// <param name="targetPos"> the object (camera) we want the text to face. </param>
    public void LookAtBackwards(Vector3 targetPos)
    {
        Vector3 offset = textObject.transform.position - targetPos;
        textObject.transform.LookAt(textObject.transform.position + offset);
    }
}
