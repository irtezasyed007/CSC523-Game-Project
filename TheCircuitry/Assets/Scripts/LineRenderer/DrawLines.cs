using UnityEngine;
using System.Collections;

// Put this script on a Camera
public class DrawLines : MonoBehaviour
{

    // Fill/drag these in from the editor

    // Choose the Unlit/Color shader in the Material Settings
    // You can change that color, to change the color of the connecting lines
    public Material lineMat;
    public GameObject point1;
    public GameObject point2;


    // Connect all of the `points` to the `mainPoint`
    internal void DrawConnectingLines()
    {
        if (point1 && point2)
        {
            // Loop through each point to connect to the mainPoint
            //foreach (GameObject point in points)
            //{
                Vector3 point1Pos = point1.transform.position;
                Vector3 point2Pos = point2.transform.position;

                GL.Begin(GL.LINES);
                lineMat.SetPass(0);
                GL.Color(new Color(lineMat.color.r, lineMat.color.g, lineMat.color.b, lineMat.color.a));
                GL.Vertex3(point1Pos.x, point1Pos.y, point1Pos.z);
                GL.Vertex3(point2Pos.x, point2Pos.y, point2Pos.z);
                GL.End();
            //}
        }
    }

    // To show the lines in the game window whne it is running
    void OnPostRender()
    {
        DrawConnectingLines();
    }

    // To show the lines in the editor
    void OnDrawGizmos()
    {
        DrawConnectingLines();
    }
}