using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bezier : MonoBehaviour
{

    private LineRenderer lineRenderer;
    private GameObject point0;
    public GameObject point2;
    public GameObject point1;
    public float p1x, p1y, p1z;

    private int numPoints = 50;
    private Vector3[] positions = new Vector3[50];

    private Color startColor = Color.blue;
    private Color endColor = Color.red;

    private Transform dest;
    public float volume;
    private float volume_offset;
    private float height = 1f;
    private float radius_offset = 0.01f;

    public void turnOff()
    {
        lineRenderer.enabled = false;
        point2.GetComponent<MeshRenderer>().enabled = false;
        //point2.transform.localScale -= new Vector3(1f, 1f, 1f);
    }

    public void turnOn()
    {
        lineRenderer.enabled = true;
        point2.GetComponent<MeshRenderer>().enabled = true;
        //point2.transform.localScale += new Vector3(1f, 1f, 1f);
    }

    // Start is called before the first frame update
    public void activate(GameObject point2, float volume)
    {
        
        this.point2 = point2;
        this.volume = volume;
        volume_offset = Maximum_rollOff(volume);
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        point0 = transform.parent.gameObject;    // works
                                                 // point2 = destination         use GameObject.Find("name") to get object named name. how to know name though? set during creation with .name = name
                                                 // point1 = calculate from point0 and point2

        point1 = new GameObject();

        point1.transform.position = new Vector3((point0.transform.position.x+point2.transform.position.x) / 2, (point0.transform.position.y + point2.transform.position.y) / 2, volume_offset*height+ (point0.transform.position.z + point2.transform.position.z) / 2);
        lineRenderer.positionCount = numPoints;
        DrawQuadraticCurve();
    }

    private float Maximum_rollOff(float volume)
    {
        //using sigmoid function (expanded by factor of 2.5 and shifted in positive direction)
        return 5/(1 + Mathf.Exp(-((volume/2.5f)-2)));
    }


    // Update is called once per frame

    private void DrawQuadraticCurve()
    {
        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            positions[i - 1] = CalculateQuadraticBezierPoint(t, point0.transform.position, point1.transform.position, point2.transform.position);
        }

        // set line size
        lineRenderer.SetPositions(positions);
        lineRenderer.startWidth = volume_offset*radius_offset;
        lineRenderer.endWidth = volume_offset*radius_offset;

        // set gradient (defines start and end of OD link)
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        lineRenderer.colorGradient = gradient;

    }

    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }
}