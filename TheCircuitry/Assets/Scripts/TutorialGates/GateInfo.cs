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
            if (gate != "Inverter")
            {
                dict.Add(gate, "The " + gate + " gate, shown below, takes in two inputs and outputs the values" +
                      " shown in the truth table to the right.");
            }
            else
            {
                dict.Add(gate, "The inverter, shown below, takes in one input and inverts the value" +
                    " as shown in the truth table to the right");
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
