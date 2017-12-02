using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicGates : MonoBehaviour {
    // Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null && hit.collider.transform == this.transform)
            {
                if(hit.collider.offset.x == 0)
                {

                }
                else // One of the "wire" colliders was clicked
                {
                    GameManager.SetWirePoint(hit.collider);
                }
            }
        }
    }

}
