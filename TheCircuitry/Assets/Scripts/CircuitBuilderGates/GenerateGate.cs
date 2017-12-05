using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGate : MonoBehaviour {
    public LayerMask mask;  // Value set through the inspector
	// Use this for initialization
	void Start () {
        mask.value = 1 << LayerMask.NameToLayer("Default");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && GameManager.CircuitBuilder.HeldGate == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, mask);    // mask is set to Default
            if(hit.collider != null && hit.collider.transform == this.transform)
            {
                GameManager.InstantiateGate(transform.name);
            }
        }
	}
}
