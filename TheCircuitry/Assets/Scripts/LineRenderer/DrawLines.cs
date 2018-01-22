using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Put this script on a Camera
public class DrawLines : MonoBehaviour
{

    // Fill/drag these in from the editor

    // Choose the Unlit/Color shader in the Material Settings
    // You can change that color, to change the color of the connecting lines
    public Material lineMat;
    public GameObject point1;
    public GameObject point2;
    internal List<Collider2D[]> collidersToDraw;

    private void Awake()
    {
        lineMat = Resources.Load<Material>("Materials/Background");
        
    }

    private void Start()
    {
        if (GameManager.Manager.circuitBuilder == null) return;

        if (GameManager.Manager.circuitBuilder.drawColliders == null)
        {
            collidersToDraw = new List<Collider2D[]>();
        }
        else
        {
            collidersToDraw = new List<Collider2D[]>(GameManager.Manager.circuitBuilder.drawColliders);
        }
    }

    // Connect all of the `points` to the `mainPoint`
    internal void DrawConnectingLines()
    {
        if (point1 && point2)
        {
            Vector3 point1Pos = point1.transform.position;
            Vector3 point2Pos = point2.transform.position;

            Draw2pxWide(point1Pos, point2Pos);
        }
        if(collidersToDraw != null && collidersToDraw.Count != 0)
        {
            foreach (Collider2D[] endpointColliders in collidersToDraw)
            {
                // Look at GameManager.CircuitBuilder.DetermineCollider2DPosition(Collider2D col) for insight into this logic
                Vector3 startPoint = GameManager.Manager.circuitBuilder.DetermineCollider2DPosition(endpointColliders[0]);
                Vector3 endPoint = GameManager.Manager.circuitBuilder.DetermineCollider2DPosition(endpointColliders[1]);
                Draw2pxWide(startPoint, endPoint);
            }
        }
    }

    internal bool ContainsCollidersInEitherOrder(Collider2D col1, Collider2D col2)
    {
        foreach(Collider2D[] cols in collidersToDraw)
        {
            if(cols[0] == col1 && cols[1] == col2)
            {
                return true;
            }
            if(cols[0] == col2 && cols[1] == col1)
            {
                return true;
            }
        }
        return false;
    }

    internal bool ContainsAtLeastOneCollider(Collider2D col1, Collider2D col2)
    {
        foreach (Collider2D[] cols in collidersToDraw)
        {
            if (cols[0] == col1 || cols[0] == col2 || cols[1] == col1 || cols[1] == col2)
            {
                return true;
            }
        }
        return false;
    }

    internal bool ContainsCollider(Collider2D col)
    {
        foreach (Collider2D[] cols in collidersToDraw)
        {
            if (cols[0] == col || cols[1] == col)
            {
                return true;
            }
        }
        return false;
    }

    internal Collider2D[] GetPairedColliders(Collider2D col)
    {
        List<Collider2D> matchedColliders = new List<Collider2D>();
        foreach (Collider2D[] cols in collidersToDraw)
        {
            if(cols[0] == col)
            {
                matchedColliders.Add(cols[1]);
            }
            else if (cols[1] == col)
            {
                matchedColliders.Add(cols[0]);
            }
        }
        return matchedColliders.ToArray();
    }

    internal Collider2D GetPairedCollider(Collider2D col)
    {
        foreach (Collider2D[] cols in collidersToDraw)
        {
            if (cols[0] == col)
            {
                return cols[1];
            }
            else if (cols[1] == col)
            {
                return cols[0];
            }
        }
        return null;
    }

    // To show the lines in the game window whne it is running
    void OnRenderObject()
    {
        DrawGrid();
        DrawConnectingLines();
    }

    // To show the lines in the editor
    void OnDrawGizmos()
    {
        DrawGrid();
        DrawConnectingLines();
    }

    void DrawGrid()
    {
        // The "y-coordinate" of the bottom side of the background. +5 is for adjustment
        float backgroundBot = GameObject.FindGameObjectWithTag("Background").transform.position.y - 768/2 + 5;
        // The "top" value of the Grid bar. Not sure why Unity returns the value as a negative
        float gridTop = GameObject.FindGameObjectWithTag("GridTop").GetComponent<RectTransform>().offsetMax.y * -1 + backgroundBot + 384;
        // The "bot" value of the Grid bar
        float gridBot = GameObject.FindGameObjectWithTag("GridBot").GetComponent<RectTransform>().offsetMax.y * -1 + backgroundBot + 384;
        // The "x-coordinate" of the left side of the background.
        float backgroundLeft = GameObject.FindGameObjectWithTag("Background").transform.position.x - 1024/2;
        //
        float gridLeft = backgroundLeft;
        float gridRight = backgroundLeft + 1024;
        float movingX, movingY, numGridLines = 9;  
        for (int i = 0; i < numGridLines; i++)
        {
            movingX = backgroundLeft + 144 + (96 * i);
            Draw(movingX, gridTop, movingX, gridBot);
            if(i == 0)  // The left-most vertical grid line
            {
                Draw(movingX - 0.5f, gridTop, movingX - 0.5f, gridBot);
                Draw(movingX - 1, gridTop, movingX - 1, gridBot);
                Draw(movingX - 1.5f, gridTop, movingX - 1.5f, gridBot);
                Draw(movingX - 2, gridTop, movingX - 2, gridBot);
                Draw(movingX - 2.5f, gridTop, movingX - 2.5f, gridBot);
                Draw(movingX - 3, gridTop, movingX - 3, gridBot);
            }
            if(i == 8)  // The right-most vertical grid line
            {
                Draw(movingX + 1, gridTop, movingX + 1, gridBot);
                Draw(movingX + 2, gridTop, movingX + 2, gridBot);
                Draw(movingX + 3, gridTop, movingX + 3, gridBot);

                Draw(movingX + 0.5f, gridTop, movingX + 0.5f, gridBot);
                Draw(movingX + 1.5f, gridTop, movingX + 1.5f, gridBot);
                Draw(movingX + 2.5f, gridTop, movingX + 2.5f, gridBot);
            }
            if(i < 4)
            {
                // Sorry! 11 is a magic number, it adjusts the lines so they line up just right. I'm sure there's a reason
                // for it, just can't really bother with it right now
                movingY = backgroundBot + 1024/2 + 11 - (96 * (i));
                Draw(gridLeft, movingY, gridRight, movingY);
            }
        }

         
        
    }

    void Draw(float x1, float y1, float x2, float y2)
    {
        GL.Begin(GL.LINES);
        lineMat.SetPass(0);
        GL.Color(new Color(lineMat.color.r, lineMat.color.g, lineMat.color.b, lineMat.color.a));
        GL.Vertex3(x1, y1, 0);
        GL.Vertex3(x2, y2, 0);
        GL.End();
    }

    void Draw(Vector3 pointA, Vector3 pointB)
    {
        GL.Begin(GL.LINES);
        lineMat.SetPass(0);
        GL.Color(new Color(lineMat.color.r, lineMat.color.g, lineMat.color.b, lineMat.color.a));
        GL.Vertex3(pointA.x, pointA.y, pointA.z);
        GL.Vertex3(pointB.x, pointB.y, pointB.z);
        GL.End();
    }
    
    void Draw2pxWide(Vector3 pointA, Vector3 pointB)    // I draw a line, then another 1px above it or to the right
    {
        if(pointA.x == pointB.x)
        {
            Draw(pointA, pointB);
            Draw(pointA.x + (1/3f), pointA.y, pointB.x + (1/3f), pointB.y);
            Draw(pointA.x + (2/3f), pointA.y, pointB.x + (2/3f), pointB.y);
            Draw(pointA.x + 1, pointA.y, pointB.x + 1, pointB.y);
        }
        else
        {
            Draw(pointA, pointB);
            Draw(pointA.x, pointA.y + (1/3f), pointB.x, pointB.y + (1/3f));
            Draw(pointA.x, pointA.y + (2/3f), pointB.x, pointB.y + (2/3f));
            Draw(pointA.x, pointA.y + 1, pointB.x, pointB.y + 1);
        }   
    }

    void Draw2pxWide(float x1, float y1, float x2, float y2)    // I draw a line, then another 1px above it or to the right
    {
        if(x1 == x2)
        {
            Draw(x1, y1, x2, y2);
            Draw(x1 + (1/3f), y1, x2 + (1/3f), y2);
            Draw(x1 + (2/3f), y1, x2 + (2/3f), y2);
            Draw(x1 + 1, y1, x2 + 1, y2);
        }
        else
        {
            Draw(x1, y1, x2, y2);
            Draw(x1, y1 - (1/3f), x2, y2 - (1/3f));
            Draw(x1, y1 - (2/3f), x2, y2 - (2/3f));
            Draw(x1, y1 - 1, x2, y2 - 1);
        }  
    }
}