using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class volumeInfo : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject FloatingTextPrefab;
    private GameObject textObject;
    private float volume;
    private GameObject textPos;
    public void activate(float volume, GameObject FloatingTextPrefab, GameObject textPos)
    {
        this.FloatingTextPrefab = FloatingTextPrefab;
        this.volume = volume;
        this.textPos = textPos;
        showFloatingText();
    }

    public GameObject showFloatingText()
    {
        textObject = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity);
        //textObject.transform.SetParent(null);
        textObject.transform.position = textPos.transform.position;
        textObject.GetComponent<TextMesh>().text = "Arc volume: " + volume;// replace with TextAsset text_file
        textObject.GetComponent<TextMesh>().color = Color.black;
        textObject.GetComponent<TextMesh>().fontSize = 100;
        textObject.transform.localScale = new Vector3(0.007f, 0.007f, 0.007f);
        textObject.transform.SetParent(transform);  //follows state-map rotation
        return textObject;
    }

    public void Update()
    {
        if (textObject != null)
        {
            textObject.transform.position = transform.position;
            LookAtBackwards(Camera.main.transform.position);
          
        }

    }

    public void LookAtBackwards(Vector3 targetPos)
    {
        Vector3 offset = textObject.transform.position - targetPos;
        textObject.transform.LookAt(textObject.transform.position + offset);
    }
}
