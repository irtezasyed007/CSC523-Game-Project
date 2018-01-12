using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicGates : MonoBehaviour {
    public bool isBeingHeld;
    public LayerMask mask;  // Value set through the Inspector
    private Collider2D[] inputs;
    private Collider2D output;
    private string gateName;

    public Collider2D[] Inputs
    {
        get { return inputs; }
    }
    public Collider2D Output
    {
        get { return output; }
    }
    public string GateName
    {
        get { return gateName; }
    }
    // Use this for initialization
	void Start () {
        gateName = gameObject.name.Split('_')[0];
        List<Collider2D> cols = new List<Collider2D>();
        foreach(Collider2D col in GetComponents<Collider2D>())
        {
            if(col.offset.x > 0)
            {
                output = col;
            }
            else if(col.offset.x < 0)
            {
                cols.Add(col);
            }
        }
        inputs = cols.ToArray();
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
                    if (!isBeingHeld && GameManager.Manager.circuitBuilder.HeldGate == null)
                    {
                        GameManager.Manager.circuitBuilder.CarryGate(gameObject);
                    }
                    else if(isBeingHeld && GameManager.Manager.circuitBuilder.HeldGate == gameObject)
                    {
                        GameManager.Manager.circuitBuilder.DropGateIfPossible(gameObject);
                    }
                    
                }
                else // One of the "wire" colliders was clicked
                {
                    GameManager.Manager.circuitBuilder.SetWirePoint(hit.collider);
                }
            }
        }
        if (isBeingHeld)
        {
            GameManager.Manager.circuitBuilder.UpdateCarriedGate(gameObject);
        }
    }
    
    internal bool CollidersAreInputs(Collider2D col1, Collider2D col2)
    {
        List<Collider2D> cols = new List<Collider2D>(inputs);
        if(cols.Contains(col1) && cols.Contains(col2))
        {
            return true;
        }
        return false;
    }

    internal Collider2D GetOtherInput(Collider2D col)
    {
        if(!gateName.Contains("Inverter") && !gateName.Contains("Input"))
        {
            if(inputs[0] == col)
            {
                return inputs[1];
            }
            else if(inputs[1] == col)
            {
                return inputs[0];
            }
        }
        return null;
    }

}
