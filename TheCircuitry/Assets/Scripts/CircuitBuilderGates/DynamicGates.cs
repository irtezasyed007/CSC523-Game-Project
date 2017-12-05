using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicGates : MonoBehaviour {
    public bool isBeingHeld;
    public LayerMask mask;  // Value set through the Inspector
    // Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, mask);    // mask is set to Default

            if (hit.collider != null && hit.collider.transform == this.transform)
            {
                if(hit.collider.offset.x == 0)
                {
                    if (!isBeingHeld && GameManager.CircuitBuilder.HeldGate == null)
                    {
                        GameManager.CarryGate(gameObject);
                    }
                    else if(isBeingHeld && GameManager.CircuitBuilder.HeldGate == gameObject)
                    {
                        GameManager.DropGateIfPossible(gameObject);
                    }
                    
                }
                else // One of the "wire" colliders was clicked
                {
                    GameManager.SetWirePoint(hit.collider);
                }
            }
        }
        if (isBeingHeld)
        {
            GameManager.UpdateCarriedGate(gameObject);
        }
    }

}
