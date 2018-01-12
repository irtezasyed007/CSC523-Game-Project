//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GLTest : MonoBehaviour {
//    public Material mat;
//    private Vector3 startPos;
//    private Vector3 mousePos;
//	// Use this for initialization
//	void Start () {
//        mat = new Material(Shader.Find("Standard"));
//        Vector3 initialPos = this.gameObject.transform.position;
//        startPos = new Vector3(initialPos.x / Screen.width, initialPos.y / Screen.height, 0);  
//	}

//	// Update is called once per frame
//	void Update () {
//        mousePos = Input.mousePosition;
//    }

//    void OnPostRender()
//    {
//        GL.PushMatrix();
//        mat.SetPass(0);

//        GL.Begin(GL.LINES);
//        GL.Color(Color.black);
//        GL.Vertex(startPos);
//        GL.Vertex(new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height, 0));
//        GL.End();
//        GL.PopMatrix();
//    }
//}

using UnityEngine;
using System.Collections;

public class GLTest : MonoBehaviour
{
    public Material mat;
    private Vector3 startVertex;
    private Vector3 mousePos;
    void Update()
    {
        mousePos = Input.mousePosition;
        if (Input.GetKeyDown(KeyCode.Space))
            startVertex = new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height, 0);

    }
    void OnPostRender()
    {
        if (!mat)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }
        GL.PushMatrix();
        mat.SetPass(0);
        GL.LoadOrtho();
        GL.Begin(GL.LINES);
        GL.Color(Color.red);
        GL.Vertex(startVertex);
        GL.Vertex(new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height, 0));
        GL.End();
        GL.PopMatrix();
    }
    void Example()
    {
        startVertex = new Vector3(0, 0, 0);
    }
}