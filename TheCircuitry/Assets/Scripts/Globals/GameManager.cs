using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Manager;
    internal struct CircuitBuilder
    {
        private static GameObject redCircle;
        private static GameObject redCircleHolder;
        public static DrawLines draw;

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
        CircuitBuilder.draw = GameObject.Find("Main Camera").GetComponent<DrawLines>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal static void SetWirePoint(Collider2D col)
    {
        if (CircuitBuilder.RedCircle != null)   // There is a red circle indicator already
        {
            Destroy(CircuitBuilder.RedCircle);
        }

        // The gate just clicked is the same as the previous gate with the red circle indicator  
        if (CircuitBuilder.RedCircleHolder == col.gameObject)
        {
            CircuitBuilder.RedCircle = Instantiate(Resources.Load<GameObject>("Prefabs/red-circle"));
            // Offset is scaled by 75 because the red-circle sprite is also scaled by 75
            Vector3 redCirclePos = new Vector3(col.gameObject.transform.position.x + col.offset.x * 75,
                col.gameObject.transform.position.y + col.offset.y * 75, 0);
            CircuitBuilder.RedCircle.transform.position = redCirclePos;
        }
        // There was a previous red circle and the gate that contained it is not the one just clicked
        else if (CircuitBuilder.RedCircleHolder != null)
        {
            Debug.Log(CircuitBuilder.draw);
            CircuitBuilder.draw.point1 = CircuitBuilder.RedCircleHolder;
            CircuitBuilder.draw.point2 = col.gameObject;
            CircuitBuilder.RedCircleHolder = null;
        }
        else   // No gate had a red circle indicator before (that we're interested in). Also the initial case
        {
            CircuitBuilder.RedCircle = Instantiate(Resources.Load<GameObject>("Prefabs/red-circle"));
            // Offset is scaled by 75 because the red-circle sprite is also scaled by 75
            Vector3 redCirclePos = new Vector3(col.gameObject.transform.position.x + col.offset.x * 75,
                col.gameObject.transform.position.y + col.offset.y * 75, 0);
            CircuitBuilder.RedCircle.transform.position = redCirclePos;
            CircuitBuilder.RedCircleHolder = col.gameObject;
        }
            
    }
}
