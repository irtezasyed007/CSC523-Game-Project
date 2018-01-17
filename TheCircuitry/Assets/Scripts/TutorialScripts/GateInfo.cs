using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GateInfo : MonoBehaviour {
    private Dictionary<string, string> dict;
    private string[] gates = { "AND", "Inverter", "NAND", "NOR", "OR", "XNOR", "XOR" };
	// Use this for initialization
	void Start () {
        dict = new Dictionary<string, string>();
        foreach(string gate in gates)
        {
            if (gate == "AND")
            {
                dict.Add(gate, "The AND gate, shown below, outputs a 1 (true) only if both inputs are 1," +
                      " as shown in the truth table to the right. Otherwise, it outputs a 0 (false).");
            }
            else if(gate == "OR")
            {
                dict.Add(gate, "The OR gate, shown below, returns a 1 (true) only if at least one input is a 1," +
                      " as shown in the truth table to the right. Otherwise, it outputs a 0 (false).");
            }
            else if(gate == "XOR")
            {
                dict.Add(gate, "The XOR gate, shown below, outputs a 1 (true) only if exactly one input is a 1," +
                      " as shown in the truth table to the right. Otherwise, it outputs a 0 (false).");
            }
            else if(gate == "NAND")
            {
                dict.Add(gate, "The NAND gate, shown below, outputs a 1 (true) only if at least one input is a 0," +
                      " as shown in the truth table to the right. Otherwise, it outputs a 0 (false).");
            }
            else if(gate == "NOR")
            {
                dict.Add(gate, "The NOR gate, shown below, outputs a 1 (true) only if both inputs are 0," +
                      " as shown in the truth table to the right. Otherwise, it outputs a 0 (false).");
            }
            else if(gate == "XNOR")
            {
                dict.Add(gate, "The AND gate, shown below, outputs a 1 (true) only if boths inputs are 1 or both are 0," +
                      " as shown in the truth table to the right. Otherwise, it outputs a 0 (false).");
            }
            else if(gate == "Inverter")
            {
                dict.Add(gate, "The inverter, shown below, outputs a 1 (true) if the input is a 0 (false), and outputs a 0 " +
                    "if the input is a 1, as shown in the truth table to the right.");
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        string gate = this.name.Split('_')[0];
        Debug.Log(gate + " was clicked.");
        Text text = GameObject.FindGameObjectWithTag("TutorialPanelText").GetComponent<Text>();
        text.text = dict[gate];
        Sprite tableImg = Resources.Load<Sprite>("GateSprites/" + gate + "_truth_table");
        GameObject.Find("Canvas/Panel/TableImage").GetComponent<Image>().sprite = tableImg;
        Sprite gateImg = Resources.Load<Sprite>("GateSprites/" + gate + (gate == "Inverter" ? "" : "_gate"));
        GameObject.Find("Canvas/Panel/GateImage").GetComponent<Image>().sprite = gateImg;
        GameObject.FindWithTag("TutorialPanel").GetComponent<PanelBehavior>().ResetPosition();
    }

}
