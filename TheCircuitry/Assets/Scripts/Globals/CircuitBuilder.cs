using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class CircuitBuilder : MonoBehaviour {

    public static Tower instance;

    private static string INPUT_NAME = "Input";

    public UnityEngine.UI.Button submit;
    public LayerMask Mask = new LayerMask();
    private GameObject redCircle;
    private GameObject redCircleHolder;
    private Collider2D selectedGateCollider;
    private GameObject heldGate;
    private List<GameObject> listOfGates;
    public GameObject[] inputs;
    public GameObject output;
    public CSC_523_Game.BooleanStringGenerator.Equation equation;
    public Function func;
    public DrawLines draw;

    internal string TestUserCircuit()
    {
        List<Collider2D[]> collidersConnectedToInputs = new List<Collider2D[]>();
        List<Collider2D> wires = new List<Collider2D>();
        for (int i = 0; i < inputs.Length; ++i)
        {
            if(draw.GetPairedColliders(inputs[i].GetComponent<Collider2D>()).Length != 0)
            {
                collidersConnectedToInputs.Add(draw.GetPairedColliders(inputs[i].GetComponent<Collider2D>()));
            }
        }
        if(collidersConnectedToInputs.Count == 0)
        {
            return "You haven't connected any inputs!";
        }
        Collider2D[][] wires2DArray = collidersConnectedToInputs.ToArray();
        foreach(Collider2D[] colArray in wires2DArray)
        {
            foreach(Collider2D col in colArray)
            {
                wires.Add(col);
            }
        }

        bool[] truthBools = func.getTruthResults();
        bool[] results = UserCircuitInspector(wires.ToArray());

        for(int i = 0; i < truthBools.Length; i++)
        {
            Debug.Log("Result " + (i + 1) + ": " + results[i]);
        }

        for (int i = 0; i < truthBools.Length; ++i)
        {
            if (truthBools[i] != results[i])
            {
                instance.GetComponent<Tower>().isBroken = true;
                GameManager.appendToScore(-2);
                return "Sorry, your answer isn't correct! Make sure there are no extra gates and all wires are properly connected.";
            }
                
        }

        instance.GetComponent<Tower>().isBroken = false;
        GameManager.appendToScore(10);
        return "Answer is correct!";
    }
    
    private bool[] UserCircuitInspector(Collider2D[] wires)
    {
        int finalOutputsCounter = 0;
        bool[] values, finalOutputs = new bool[(int)Mathf.Pow(2, equation.UniqueVars.Length)];
        if (equation.UniqueVars.Length == 2)
        {
            values = new bool[2] { false, false };
            for (int x = 0; x < 2; ++x)
            {
                for(int y = 0; y < 2; ++y)
                {
                    int totalLoopIndex = (2 * x) + (1 * y);
                    values[0] = System.Convert.ToBoolean(x);
                    values[1] = System.Convert.ToBoolean(y);
                    finalOutputs[totalLoopIndex] 
                        = DetermineOutputForTheseInputs(wires, values, finalOutputs, finalOutputsCounter, totalLoopIndex);
                }
            }
        }
        else if (equation.UniqueVars.Length == 3)
        {
            values = new bool[3] { false, false, false };
            for (int x = 0; x < 2; ++x)
            {
                for (int y = 0; y < 2; ++y)
                {
                    for (int z = 0; z < 2; ++z)
                    {
                        int totalLoopIndex = (4 * x) + (2 * y) + (1 * z);
                        values[0] = System.Convert.ToBoolean(x);
                        values[1] = System.Convert.ToBoolean(y);
                        values[2] = System.Convert.ToBoolean(z);
                        finalOutputs[totalLoopIndex]
                            = DetermineOutputForTheseInputs(wires, values, finalOutputs, finalOutputsCounter, totalLoopIndex);
                    }
                }
            }
        }
        else if (equation.UniqueVars.Length == 4)
        {
            values = new bool[4] { false, false, false, false };
            for (int x = 0; x < 2; ++x)
            {
                for (int y = 0; y < 2; ++y)
                {
                    for (int z = 0; z < 2; ++z)
                    {
                        for (int w = 0; w < 2; ++w)
                        {
                            int totalLoopIndex = (8 * x) + (4 * y) + (2 * z) + (1 * w);
                            values[0] = System.Convert.ToBoolean(x);
                            values[1] = System.Convert.ToBoolean(y);
                            values[2] = System.Convert.ToBoolean(z);
                            values[3] = System.Convert.ToBoolean(w);
                            finalOutputs[totalLoopIndex] 
                                = DetermineOutputForTheseInputs(wires, values, finalOutputs, finalOutputsCounter, totalLoopIndex);
                        }
                    }
                }
            }
        }
        
        return finalOutputs;
    }

    private bool DetermineOutputForTheseInputs(Collider2D[] wires, bool[] values, bool[] finalOutputs, int finalOutputsCounter
        , int totalLoopIndex)
    {
        Dictionary<Collider2D, bool> wireValues = new Dictionary<Collider2D, bool>();

        List<Collider2D> prevWires = new List<Collider2D>(wires);
        List<Collider2D> mergedWires = new List<Collider2D>(prevWires);
        List<GameObject> gates = new List<GameObject>(), gatesAtStartOfLoop = new List<GameObject>(gates);
        
        foreach (Collider2D col in wires)
        {
            for (int i = 0; i < equation.UniqueVars.Length; ++i)
            {
                if (draw.GetPairedCollider(col).GetComponentInChildren<TextMesh>().text.ToCharArray()[0] == func.getHeader().ToUpper()[i]  
                    && !wireValues.ContainsKey(col))
                {
                    wireValues.Add(col, values[i]);
                }
            }

        }

        bool endWasReached = false;
        int escapeCounter = 0;
        while (!endWasReached && escapeCounter < 10)
        {
            foreach (Collider2D wire in prevWires)
            {
                if (!gates.Contains(wire.gameObject) && !gatesAtStartOfLoop.Contains(wire.gameObject))
                {
                    if (wire.name.Contains("Inverter"))
                    {
                        wireValues = ApplyGateOperation(wireValues, wire, wire2: null);
                        mergedWires.Remove(wire);
                        mergedWires.Add(wire.GetComponent<DynamicGates>().Output);
                    }
                    else
                    {
                        gates.Add(wire.gameObject);
                    }
                }
                else
                {
                    if (mergedWires.Contains(wire.GetComponent<DynamicGates>().GetOtherInput(wire))
                        && wireValues.ContainsKey(wire.GetComponent<DynamicGates>().GetOtherInput(wire)))
                    {
                        gates.Remove(wire.gameObject);
                        wireValues = ApplyGateOperation(wireValues, wire, wire.GetComponent<DynamicGates>().GetOtherInput(wire));
                        mergedWires.Remove(wire.GetComponent<DynamicGates>().GetOtherInput(wire));
                        mergedWires.Remove(wire);
                        mergedWires.Add(wire.GetComponent<DynamicGates>().Output);
                    }

                }
            }
            gatesAtStartOfLoop = new List<GameObject>(gates);
            mergedWires = GetInputsForNextGates(mergedWires, gates);
            prevWires = new List<Collider2D>(mergedWires);
            wireValues = GetInputsValuesForNextGates(wireValues, gates);
            if (mergedWires.ToArray()[0].name.Contains("Output"))
            {
                Debug.Log("I'm in!");
                escapeCounter = 0;
                endWasReached = true;
                finalOutputs[totalLoopIndex] = wireValues[mergedWires.ToArray()[0].GetComponent<Collider2D>()];
                prevWires = new List<Collider2D>(wires);
                mergedWires = new List<Collider2D>(prevWires);
                gates = new List<GameObject>();
                gatesAtStartOfLoop = new List<GameObject>(gates);
                wireValues = new Dictionary<Collider2D, bool>();
            }
            else
            {
                escapeCounter++;
            }
        }
        return finalOutputs[totalLoopIndex];
    }
    private Dictionary<Collider2D, bool> ApplyGateOperation(Dictionary<Collider2D, bool> wireVals, Collider2D wire1, Collider2D wire2)
    {
        bool newVal;
        if (!wire2)
        {
            if (wire1.name.Contains("Inverter"))
            {
                newVal = !wireVals[wire1];
                wireVals.Remove(wire1);
                wireVals.Add(wire1.gameObject.GetComponent<DynamicGates>().Output, newVal);
            }
            else
            {

            }
        }
        else
        {
            
            if (wire2.name.Contains("AND") && !wire2.name.Contains("NAND"))
            {
                newVal = wireVals[wire1] & wireVals[wire2];
                wireVals.Remove(wire1);
                wireVals.Remove(wire2);
                wireVals.Add(wire2.gameObject.GetComponent<DynamicGates>().Output, newVal);
            }
            else if (wire2.name.Contains("OR") && !wire2.name.Contains("XOR") && !wire2.name.Contains("NOR") && !wire2.name.Contains("XNOR"))
            {
                newVal = wireVals[wire1] | wireVals[wire2];
                wireVals.Remove(wire1);
                wireVals.Remove(wire2);
                wireVals.Add(wire2.gameObject.GetComponent<DynamicGates>().Output, newVal);
            }
            else if (wire2.name.Contains("XOR"))
            {
                newVal = wireVals[wire1] ^ wireVals[wire2];
                wireVals.Remove(wire1);
                wireVals.Remove(wire2);
                wireVals.Add(wire2.gameObject.GetComponent<DynamicGates>().Output, newVal);
            }
            else if (wire2.name.Contains("NAND"))
            {
                newVal = !(wireVals[wire1] & wireVals[wire2]);
                wireVals.Remove(wire1);
                wireVals.Remove(wire2);
                wireVals.Add(wire2.gameObject.GetComponent<DynamicGates>().Output, newVal);
            }
            else if (wire2.name.Contains("NOR") && !wire2.name.Contains("XNOR"))
            {
                newVal = !(wireVals[wire1] | wireVals[wire2]);
                wireVals.Remove(wire1);
                wireVals.Remove(wire2);
                wireVals.Add(wire2.gameObject.GetComponent<DynamicGates>().Output, newVal);
            }
            else if (wire2.name.Contains("XNOR"))
            {
                newVal = !(wireVals[wire1] ^ wireVals[wire2]);
                wireVals.Remove(wire1);
                wireVals.Remove(wire2);
                wireVals.Add(wire2.gameObject.GetComponent<DynamicGates>().Output, newVal);
            }
        }
        return wireVals;
    }

    private List<Collider2D> GetInputsForNextGates(List<Collider2D> wireList, List<GameObject> gates)
    {
        List<Collider2D> copyList = new List<Collider2D>(wireList);
        foreach (Collider2D col in wireList)
        {
            if (!gates.Contains(col.gameObject))
            {
                copyList.Remove(col);
                copyList.Add(draw.GetPairedCollider(col));
            }        
        }
        return copyList;
    }

    private Dictionary<Collider2D, bool> GetInputsValuesForNextGates(Dictionary<Collider2D, bool> wireVals, List<GameObject> gates)
    {
        Dictionary<Collider2D, bool> copyDict = new Dictionary<Collider2D, bool>(wireVals);
        foreach(Collider2D col in wireVals.Keys)
        {
            if (!gates.Contains(col.gameObject))
            {
                copyDict.Add(draw.GetPairedCollider(col), wireVals[col]);
                copyDict.Remove(col);
            }        
        }
        return copyDict;
    }
    public void InitializeSceneParameters()
    {
        GameManager.Manager.activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        submit = GameObject.Find("SubmitButton").GetComponent<UnityEngine.UI.Button>();
        submit.onClick.AddListener(() => {
            GameObject panel = Instantiate(Resources.Load<GameObject>("UIItems/ResultsPanel"));
            panel.transform.parent = GameObject.Find("Canvas").transform;

            panel.GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(() => {
                panel.SetActive(false);
                Destroy(panel);
            });

            panel.GetComponent<RectTransform>().offsetMax = new Vector3(100, 100, 0);
            panel.GetComponent<RectTransform>().offsetMin = new Vector3(100, 100, 0);
            string result = TestUserCircuit();
            panel.GetComponentInChildren<UnityEngine.UI.Text>().text = result;
            panel.GetComponentInChildren<UnityEngine.UI.Text>().fontSize = 35;

            if (result == "Answer is correct!")
            {
                panel.GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(() => {
                    panel.SetActive(false);
                    Destroy(panel);
                    new ButtonHandler().LoadScene("level1");
                });
            }
        });
        draw = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<DrawLines>();
        listOfGates = new List<GameObject>();
        Mask.value = 1 << LayerMask.NameToLayer("Default");
        int diff = 2;
        if (GameManager.score >= 50 && GameManager.score < 100) diff = 3;
        else if (GameManager.score >= 100) diff = 4;
        equation = CSC_523_Game.BooleanStringGenerator.generateBooleanString(diff);
        func = new Function(equation.StackString);
        func.viewTruthTable();
        bool[] stringArr = func.getTruthResults();
        char[] charArr = new char[stringArr.Length];
        for (int i = 0; i < stringArr.Length; ++i)
        {
            Debug.Log(stringArr[i]);
        }
        GameObject.FindGameObjectWithTag("Function").GetComponent<UnityEngine.UI.Text>().text = "Function: \n"
            + equation.BooleanFunction;
        InitializeInputsAndOutput();
    }
    public void InitializeInputsAndOutput()
    {
        List<char> sorter = new List<char>(equation.UniqueVars);
        sorter.Sort();
        char[] sortedVars = sorter.ToArray();
        inputs = new GameObject[equation.UniqueVars.Length];
        for (int i = 0; i < equation.UniqueVars.Length; ++i)
        {
            inputs[i] = Instantiate(Resources.Load<GameObject>("Prefabs/Input"));
            inputs[i].GetComponentInChildren<TextMesh>().text = sortedVars[i].ToString();
        }
        if (inputs.Length == 2)
        {
            inputs[0].transform.position = new Vector3(inputs[0].transform.position.x,
                inputs[0].transform.position.y - 96, 0);    // Remember, 96 x 96 is size for inputs and dynamic gates
            inputs[1].transform.position = new Vector3(inputs[1].transform.position.x,
                inputs[1].transform.position.y - (3 * 96), 0);
        }
        else if (inputs.Length == 3)
        {
            inputs[1].transform.position = new Vector3(inputs[1].transform.position.x,
                inputs[1].transform.position.y - (2 * 96), 0);
            inputs[2].transform.position = new Vector3(inputs[2].transform.position.x,
                inputs[2].transform.position.y - (4 * 96), 0);
        }
        else if (inputs.Length == 4)
        {
            inputs[1].transform.position = new Vector3(inputs[1].transform.position.x,
                inputs[1].transform.position.y - (1 * 96), 0);
            inputs[2].transform.position = new Vector3(inputs[2].transform.position.x,
                inputs[2].transform.position.y - (3 * 96), 0);
            inputs[3].transform.position = new Vector3(inputs[3].transform.position.x,
                inputs[3].transform.position.y - (4 * 96), 0);
        }
        output = Instantiate(Resources.Load<GameObject>("Prefabs/Output"));
    }
    public void AddGateToList(GameObject gate)
    {
        listOfGates.Add(gate);
    }

    public void RemoveGateFromList(GameObject gate)
    {
        listOfGates.Remove(gate);
        RemoveGateCollidersFromList(gate);
    }

    public void RemoveGateCollidersFromList(GameObject gate)
    {
        // Unity doesn't allow modification of collections while iterating... so I make a new list and delete the old one.
        List<Collider2D[]> newCollidersToDraw = new List<Collider2D[]>();
        foreach (Collider2D[] endpointColliders in draw.collidersToDraw)
        {
            if (endpointColliders[0].gameObject != gate && endpointColliders[1].gameObject != gate)
            {
                newCollidersToDraw.Add(endpointColliders);
            }
        }
        draw.collidersToDraw = newCollidersToDraw;
    }

    public void RemoveColliderAndItsPairsFromList(Collider2D col)
    {
        // Unity doesn't allow modification of collections while iterating... so I make a new list and delete the old one.
        List<Collider2D[]> newCollidersToDraw = new List<Collider2D[]>();
        foreach (Collider2D[] endpointColliders in draw.collidersToDraw)
        {
            if (endpointColliders[0] != col && endpointColliders[1] != col)
            {
                newCollidersToDraw.Add(endpointColliders);
            }
        }
        draw.collidersToDraw = newCollidersToDraw;
    }

    internal void SetWirePoint(Collider2D col)
    {

        if (redCircle != null)   // There is a red circle indicator already
        {
            Destroy(redCircle);
            redCircle = null;
        }

        // The gate just clicked is the same as the previous gate with the red circle indicator  
        if (redCircleHolder == col.gameObject)
        {
            InstantiateRedCircle(col);
            selectedGateCollider = col;
        }
        // There was a previous red circle and the gate that contained it is not the one just clicked
        else if (redCircleHolder != null)
        {
            // If the x offsets don't equal, then the pairing of the each gate's side is a valid input-output pair
            if (col.offset.x != selectedGateCollider.offset.x)
            {
                // Multiple lines can connect from/to an input ONLY
                bool canDrawLine = true;
                if (selectedGateCollider.name.Contains(INPUT_NAME) ^ col.name.Contains(INPUT_NAME))
                {
                    if (draw.ContainsCollidersInEitherOrder(selectedGateCollider, col))
                    {
                        canDrawLine = false;
                    }

                    if (selectedGateCollider.name.Contains(INPUT_NAME) && draw.ContainsCollider(col))
                    {
                        canDrawLine = false;
                    }
                    else if (col.name.Contains(INPUT_NAME) && draw.ContainsCollider(selectedGateCollider))
                    {
                        canDrawLine = false;
                    }
                }
                else   // This loop is just done to check that there is not already a line attached to this collider
                {
                    if (draw.ContainsAtLeastOneCollider(selectedGateCollider, col))
                    {
                        canDrawLine = false;
                    }
                }

                if (canDrawLine)
                {
                    Collider2D[] endpointColliders = new Collider2D[] { selectedGateCollider, col };
                    draw.collidersToDraw.Add(endpointColliders);
                    redCircleHolder = null;
                    selectedGateCollider = null;
                }
                else
                {
                    InstantiateRedCircle(col);
                    redCircleHolder = col.gameObject;
                    selectedGateCollider = col;
                }

            }
            else   // The selections are either both inputs or both outputs, so just set a new red circle instead
            {
                InstantiateRedCircle(col);
                redCircleHolder = col.gameObject;
                selectedGateCollider = col;
            }

        }
        else   // No gate had a red circle indicator before (that we're interested in). Also the initial case
        {
            InstantiateRedCircle(col);
            redCircleHolder = col.gameObject;
            selectedGateCollider = col;
        }
    }

    internal void CarryGate(GameObject gate)
    {
        gate.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gate.transform.position = new Vector3(gate.transform.position.x, gate.transform.position.y, 0);
        gate.GetComponent<DynamicGates>().isBeingHeld = true;
        gate.GetComponent<GridSnapper>().enabled = false;
        heldGate = gate;
        if (redCircleHolder == gate)
        {
            Vector3 newRedCirclePos = DetermineCollider2DPosition(selectedGateCollider);
            redCircle.transform.position = newRedCirclePos;
        }
    }

    internal void UpdateCarriedGate(GameObject gate)
    {
        gate.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gate.transform.position = new Vector3(gate.transform.position.x, gate.transform.position.y, 0);
        if (redCircleHolder == gate)
        {
            Vector3 newRedCirclePos = DetermineCollider2DPosition(selectedGateCollider);
            redCircle.transform.position = newRedCirclePos;
        }
    }

    internal void DropGateIfPossible(GameObject gate)
    {
        gate.GetComponent<DynamicGates>().isBeingHeld = false;
        gate.GetComponent<GridSnapper>().enabled = true;
        gate.GetComponent<GridSnapper>().ImmediateSnap(); // Snap immediately so we can check for other colliders.
        heldGate = null;
        // Radius = 48 because each gate is a 96 x 96 sprite. 47 is used because 48 picked up an unexpected collider.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(gate.transform.position, 47);
        if (colliders.Length > 4)    // More than 4 colliders means that there was a gate in the spot already.
        {
            CarryGate(gate);
        }
        else if (gate.transform.name == "Inverter_dynamic" && colliders.Length > 3)   // Since inverter has one less collider
        {
            CarryGate(gate);
        }
        else if (redCircleHolder == gate)
        {
            Vector3 newRedCirclePos = DetermineCollider2DPosition(selectedGateCollider);
            redCircle.transform.position = newRedCirclePos;
        }
    }

    internal void InstantiateGate(string gateType)
    {
        GameObject gate = Instantiate(Resources.Load<GameObject>("Prefabs/" + gateType + "_dynamic"));
        AddGateToList(gate);
        CarryGate(gate);
    }

    internal void InstantiateRedCircle(Collider2D col)
    {
        redCircle = Instantiate(Resources.Load<GameObject>("Prefabs/red-circle"));
        Vector3 redCirclePos = DetermineCollider2DPosition(col);
        redCircle.transform.position = redCirclePos;
    }

    internal Vector3 DetermineCollider2DPosition(Collider2D col)
    {
        // Offset is scaled by 75 because the red-circle sprite is also scaled by 75
        return new Vector3(col.gameObject.transform.position.x + col.offset.x * 75,
                col.gameObject.transform.position.y + col.offset.y * 75, 0);
    }

    public List<GameObject> ListOfGates
    {
        get { return listOfGates; }
        set { listOfGates = value; }
    }

    public GameObject RedCircleHolder
    {
        get { return redCircleHolder; }
        set { redCircleHolder = value; }
    }
    public GameObject RedCircle
    {
        get { return redCircle; }
        set { redCircle = value; }
    }

    public Collider2D SelectedGateCollider
    {
        get { return selectedGateCollider; }
        set { selectedGateCollider = value; }
    }
    public GameObject HeldGate
    {
        get { return heldGate; }
        set { heldGate = value; }
    }

    private void Awake()
    {
        
    }
    // Use this for initialization
    void Start () {
        draw = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<DrawLines>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "circuitBuilderScene")
            InitializeSceneParameters();
    }

    // Update is called once per frame
    void Update () {
        
    }

    private void OnEnable()
    {
       SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
