///////////////////////////////////////////////////////////////////////////////////////////
//FileName: bezier.cs
//FileType: visual C# Source File
//Author: Daniel S
//Description: This program creates curves based on the quadratic bezier function,
//              as unity only works with straight lines it turns 50 straight lines
//              into a curve based on the positions of 2 points (origin and destination)
//Last modified on: 23/10/19
//////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bezier : MonoBehaviour
{
    private LineRenderer lineRenderer; // LineRenderer is the object that creates lines, only makes straight lines
    private GameObject point0;  // the point specifying the origin, will be the parent of the object this script is attached to
    public GameObject point2;   // the point specifying the destination
    public GameObject point1;  // the point around which the curvature of the bezier curve is calculated

    private int numPoints = 50; // number of lines that make up the bezier curve
    private Vector3[] positions = new Vector3[50];  // create list of 50 points detailing start and end of each straight line, making up the curve
 
    public float volume;    // volume of packages that this curve represents (used in height and radius calculations) 
    private float volume_offset;    // offset volume to make sure there isnt too big a difference between smallest and largest
    private float height = 1f;  // default height our calculations scale off
    private float radius_offset = 0.01f;    // radius offset, as height and radius scale off same calculation but want radius << height
    private Color col_o;    // colour of bezier curve when at origin
    private Color col_d;    // colour of bezier curve when at destination
    private float width;    // default width calculations scale off

    /// <summary>
    /// function to turn off the bezier curve renderer when we no longer want it to be displayed
    /// curve is turned off instead of deleting so it doesnt need to be calculated later (caching)
    /// turning it off doesnt cause calculation overhead as changes in position are not calculated in unity unless it is being rendered
    /// </summary>
    public void turnOff()
    {
        lineRenderer.enabled = false;
        point2.GetComponent<MeshRenderer>().enabled = false;
    }

    /// <summary>
    /// turns renderer of the curve on so it is displayed again
    /// </summary>
    public void turnOn()
    {
        lineRenderer.enabled = true;
        point2.GetComponent<MeshRenderer>().enabled = true;
    }

    /// <summary>
    /// function to calculate bezier curve 
    /// </summary>
    /// <param name="point2"> destination position </param>
    /// <param name="volume"> volume of packages the flow represents </param>
    /// <param name="col_o"> origin colour </param>
    /// <param name="col_d"> destination colour </param>
    /// <param name="width"> width of the line to scale from </param>
    public void activate(GameObject point2, float volume, Color col_o,  Color col_d, float width)
    {
        
        this.point2 = point2;
        this.volume = volume;
        this.col_o = col_o;
        this.col_d = col_d;
        this.width = width;
        volume_offset = Maximum_rollOff(volume);
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        point0 = transform.parent.gameObject;    // origin location is the parents position

        // point1 is calculated based on origin and destination positions
        point1 = new GameObject();
        point1.transform.position = new Vector3((point0.transform.position.x+point2.transform.position.x) / 2, (point0.transform.position.y + point2.transform.position.y) / 2, volume_offset*height+ (point0.transform.position.z + point2.transform.position.z) / 2);

        lineRenderer.positionCount = numPoints;
        DrawQuadraticCurve();
    }

    /// <summary>
    /// calculation to make sure maximum and minimum values dont spread to far apart. Is based upon sigmoid function
    /// </summary>
    /// <param name="volume"> volume of packages that is being scaled. </param>
    /// <returns></returns>
    private float Maximum_rollOff(float volume)
    {
        //using sigmoid function (expanded by factor of 2.5 and shifted in positive direction)
        return 5/(1 + Mathf.Exp(-((volume/2.5f)-2)));
    }

    /// <summary>
    /// function to draw the curve
    /// </summary>
    private void DrawQuadraticCurve()
    {
        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            positions[i - 1] = CalculateQuadraticBezierPoint(t, point0.transform.position, point1.transform.position, point2.transform.position);
        }

        // set line size
        lineRenderer.SetPositions(positions);
        if (width == -1f) {
            lineRenderer.startWidth = volume_offset * radius_offset;
            lineRenderer.endWidth = volume_offset * radius_offset;
        }
        else
        {
            lineRenderer.startWidth = width * radius_offset;
            lineRenderer.endWidth = width * radius_offset;
        }
        

        // set gradient (defines start and end of OD link)
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(col_o, 0.0f), new GradientColorKey(col_d, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        lineRenderer.colorGradient = gradient;

    }

    /// <summary>
    /// function to calculate positions of each straight lines start and end point to create a bezier curve
    /// uses quadratic bezier function
    /// </summary>
    /// <param name="t"> which number point is being calculated </param>
    /// <param name="p0"> the origin point </param>
    /// <param name="p1"> the mid point that determines the curvature and height </param>
    /// <param name="p2"> the destination point </param>
    /// <returns> a point that determines the curve. all 50 point together with the lines between them create the curve </returns>
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