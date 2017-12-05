using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Manager;
    internal struct CircuitBuilder
    {
        public static LayerMask Mask = new LayerMask();
        private static GameObject redCircle;
        private static GameObject redCircleHolder;
        private static Collider2D selectedGateCollider;
        private static GameObject heldGate;
        private static List<GameObject> listOfGates;
        public static DrawLines draw;

        public static List<GameObject> ListOfGates
        {
            get { return listOfGates; }
            set { listOfGates = value; }
        }

        public static void AddGateToList(GameObject gate)
        {
            listOfGates.Add(gate);
        }

        public static void RemoveGateFromList(GameObject gate)
        {
            listOfGates.Remove(gate);
            // Unity doesn't allow modification of collections while iterating... so I make a new list and delete the old one.
            List<Collider2D[]> newCollidersToDraw = new List<Collider2D[]>();
            foreach(Collider2D[] endpointColliders in draw.collidersToDraw)
            {
                if (endpointColliders[0].gameObject != gate && endpointColliders[1].gameObject != gate)
                {
                    newCollidersToDraw.Add(endpointColliders);
                }
            }
            draw.collidersToDraw = newCollidersToDraw;
        }

        public static GameObject RedCircleHolder
        {
            get { return redCircleHolder; }
            set { redCircleHolder = value; }
        }
        public static GameObject RedCircle
        {
            get { return redCircle; }
            set { redCircle = value; }
        }
        
        public static Collider2D SelectedGateCollider
        {
            get { return selectedGateCollider; }
            set { selectedGateCollider = value; }
        }
        public static GameObject HeldGate
        {
            get { return heldGate; }
            set { heldGate = value; }
        }
    }
    // Use this for initialization
    private void Awake()
    {
        if (Manager == null)
        {
            DontDestroyOnLoad(gameObject);
            Manager = this;
        }
        else if (Manager != null)
        {
            Destroy(gameObject);
        }
    }

    void Start () {
        if (SceneManager.GetActiveScene().name == "circuitBuilderScene")
        {
            CircuitBuilder.draw = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<DrawLines>();
            CircuitBuilder.ListOfGates = new List<GameObject>();
            CircuitBuilder.Mask.value = 1 << LayerMask.NameToLayer("Default");
        }
          
	}
	
	// Update is called once per frame
	void Update () {
        
        if(SceneManager.GetActiveScene().name == "circuitBuilderScene")
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, CircuitBuilder.Mask);
                //if (hit.collider != null)
                //{
                //    // To be completed later
                //}
            }
            else if (Input.GetMouseButtonDown(1) && CircuitBuilder.HeldGate == null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, CircuitBuilder.Mask);
                if (hit.collider != null && hit.transform.name.EndsWith("(Clone)"))
                {
                    CircuitBuilder.RemoveGateFromList(hit.collider.gameObject);
                    if (CircuitBuilder.RedCircle != null)
                    {
                        Destroy(CircuitBuilder.RedCircle);
                        CircuitBuilder.RedCircle = null;
                        CircuitBuilder.RedCircleHolder = null;
                        CircuitBuilder.SelectedGateCollider = null;
                    }
                    Destroy(hit.collider.gameObject);
                }
            }
        }
        
    }

    internal static void SetWirePoint(Collider2D col)
    {
        if (CircuitBuilder.RedCircle != null)   // There is a red circle indicator already
        {
            Destroy(CircuitBuilder.RedCircle);
            CircuitBuilder.RedCircle = null;
        }

        // The gate just clicked is the same as the previous gate with the red circle indicator  
        if (CircuitBuilder.RedCircleHolder == col.gameObject)
        {
            InstantiateRedCircle(col);
            CircuitBuilder.SelectedGateCollider = col;
        }
        // There was a previous red circle and the gate that contained it is not the one just clicked
        else if (CircuitBuilder.RedCircleHolder != null)
        {
            
            // If the x offsets don't equal, then the pairing of the each gate's side is a valid input-output pair
            if (col.offset.x != CircuitBuilder.SelectedGateCollider.offset.x)
            {
                Debug.Log(CircuitBuilder.draw);
                Collider2D[] endpointColliders = new Collider2D[] { CircuitBuilder.SelectedGateCollider, col};
                
                CircuitBuilder.draw.collidersToDraw.Add(endpointColliders);
                CircuitBuilder.RedCircleHolder = null;
                CircuitBuilder.SelectedGateCollider = null;
            }
            else   // The selections are either both inputs or both outputs, so just set a new red circle instead
            {
                InstantiateRedCircle(col);
                CircuitBuilder.RedCircleHolder = col.gameObject;
                CircuitBuilder.SelectedGateCollider = col;
            }
            
        }
        else   // No gate had a red circle indicator before (that we're interested in). Also the initial case
        {
            InstantiateRedCircle(col);
            CircuitBuilder.RedCircleHolder = col.gameObject;
            CircuitBuilder.SelectedGateCollider = col;
        }   
    }

    internal static void CarryGate(GameObject gate)
    {
        gate.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gate.transform.position = new Vector3(gate.transform.position.x, gate.transform.position.y, 0);
        gate.GetComponent<DynamicGates>().isBeingHeld = true;
        gate.GetComponent<GridSnapper>().enabled = false;
        CircuitBuilder.HeldGate = gate;
    }

    internal static void UpdateCarriedGate(GameObject gate)
    {
        gate.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gate.transform.position = new Vector3(gate.transform.position.x, gate.transform.position.y, 0);
    }

    internal static void DropGateIfPossible(GameObject gate)
    {
        gate.GetComponent<DynamicGates>().isBeingHeld = false;
        gate.GetComponent<GridSnapper>().enabled = true;
        gate.GetComponent<GridSnapper>().ImmediateSnap(); // Snap immediately to check for other colliders.
        CircuitBuilder.HeldGate = null;
        // Radius = 48 because each gate is a 96 x 96 sprite. 47 is used because 48 picked up an unexpected collider.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(gate.transform.position, 47);
        if (colliders.Length > 4)    // More than 4 colliders means that there was a gate in the spot already.
        {
            CarryGate(gate);
        }
        else if(gate.transform.name == "Inverter_dynamic" && colliders.Length > 3)   // Since inverter has one less collider
        {
            CarryGate(gate);
        }
    }

    internal static void InstantiateGate(string gateType)
    {
        GameObject gate = Instantiate(Resources.Load<GameObject>("Prefabs/" + gateType + "_dynamic"));
        CircuitBuilder.AddGateToList(gate);
        CarryGate(gate);
    }

    internal static void InstantiateRedCircle(Collider2D col)
    {
        CircuitBuilder.RedCircle = Instantiate(Resources.Load<GameObject>("Prefabs/red-circle"));
        // Offset is scaled by 75 because the red-circle sprite is also scaled by 75
        Vector3 redCirclePos = new Vector3(col.gameObject.transform.position.x + col.offset.x * 75,
            col.gameObject.transform.position.y + col.offset.y * 75, 0);
        CircuitBuilder.RedCircle.transform.position = redCirclePos;
    }
}
