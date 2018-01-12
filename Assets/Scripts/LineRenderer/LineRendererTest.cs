using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererTest : MonoBehaviour {
    private LineRenderer line;
    private Vector3[] points = new Vector3[2];
	// Use this for initialization
	void Start () {
        line = this.gameObject.GetComponent<LineRenderer>();
        points[0] = this.gameObject.transform.position;
        Color color = new Color(0, 0, 0, 255);
        line.startWidth = 5;
        line.endWidth = 5;

        line.useWorldSpace = true;
        line.material = new Material(Shader.Find("Standard"));
        line.startColor = color;
    }
	
	// Update is called once per frame
	void Update () {
        points[1] = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        line.SetPositions(points);
	}
}
