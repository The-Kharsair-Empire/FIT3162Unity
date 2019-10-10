using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showtext : MonoBehaviour
{
    public GameObject FloatingTextPrefab;
    private GameObject textObject;
    public TextAsset text_file;


    public void OnMouseEnter()
    {
        showFloatingText();
    }

    public void OnMouseExit()
    {
        Destroy(textObject);
        //text.GetComponent<TextMesh>().text = "";
    }

    void showFloatingText()
    {
        textObject = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
        textObject.transform.SetParent(null);
        textObject.GetComponent<TextMesh>().text = "reference text here" + "\n" + "more text"; // replace with TextAsset text_file
        textObject.GetComponent<TextMesh>().color = Color.black;
    }
}
