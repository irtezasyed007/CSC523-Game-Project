using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicInfo : MonoBehaviour
{
    private Dictionary<string, string> dict;
    private string[] topics = { "AND", "OR", "Invert", "Parentheses", "XOR", "Inverted Gates", "De Morgan's Laws" };
    // Use this for initialization
    void Start()
    {
        dict = new Dictionary<string, string>();
        foreach (string logic in topics)
        {
            if (logic == "AND")
            {
                dict.Add(logic, "In this game, we use the '*' multiplication sign to indicate the AND operation because" +
                    " of their similarities. Using only 0's and 1's with multiplication, the only way to get 1 is 1 * 1, just like AND!");
            }
            else if (logic == "OR")
            {
                dict.Add(logic, "In this game, we use the '+' addition sign to indicate the OR operation because of their similarities" +
                    ". Using only 0's and 1's with addition, you can get with A + B = 1 as long as one of them is 1, just like OR!");
            }
            else if (logic == "Invert")
            {
                dict.Add(logic, "In this game, we use the apostrophe (') to indicate the \"invertion\" operation. This can " +
                    "also be thought of as a toggle. If the input is 1, the output is 0. If the input is 0, the output is 1.");
            }
            else if (logic == "Parentheses")
            {
                dict.Add(logic, "Parentheses are used to group together terms to indicate which order you should do the operations in." +
                    " When there are multiple parentheses, priority goes to the innermost parentheses, followed by going left to right.");
            }
            else if (logic == "XOR")
            {
                dict.Add(logic, "In this game, we use the symbol shown below to indicate 'XOR', also called the 'Exclusive OR'." +
                    " The output/answer will be 1 if one and only one of the inputs is a 1.");
            }
            //else if (logic == "XNOR")
            //{
            //    dict.Add(logic, "In this game, we use the symbol shown below to indicate 'XNOR', also called the 'Exclusive NOR'." +
            //        " The output/answer will be 1 if and only if both inputs are 1 or both are 0. This is the opposite of the XOR.");
            //}
            else if (logic == "Inverted Gates")
            {
                dict.Add(logic, "The NAND, NOR, and XNOR gates are the opposites of the AND, OR, and XOR gates, respectively. They " +
                    "are equivalent to putting an Inverter after the basic gates. You should use them because you get " +
                    "bonus points for using less gates!");
            }
            else if (logic == "De Morgan's Laws")
            {
                dict.Add(logic, "The two equations below are known as De Morgan's Laws. Basically, invert both variables, " +
                    "switch the operation from AND to OR or vice versa, and invert the final output. Note that two inverts will " +
                    "cancel each other out.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        string topic = this.name;
        Debug.Log(topic + " was clicked.");
        Text text = GameObject.FindGameObjectWithTag("TutorialPanelText").GetComponent<Text>();
        text.text = dict[topic];
        Text supportText = GameObject.Find("Canvas/Panel/SupportText").GetComponent<Text>();
        if (topic != "Inverted Gates")
        {      
            supportText.text = GameObject.Find("Canvas/" + topic + "/Text").GetComponent<Text>().text;
            if(topic == "De Morgan's Laws")
            {
                supportText.fontSize = 16;
            }
            else
            {
                supportText.fontSize = 80;
            }
        }
        else
        {
            supportText.text = "";
        }

        //Sprite tableImg = Resources.Load<Sprite>("GateSprites/" + topic + "_truth_table");
        //GameObject.Find("Canvas/Panel/TableImage").GetComponent<Image>().sprite = tableImg;
        //Sprite gateImg = Resources.Load<Sprite>("GateSprites/" + topic + (topic == "Inverter" ? "" : "_gate"));
        //GameObject.Find("Canvas/Panel/GateImage").GetComponent<Image>().sprite = gateImg;
        GameObject.FindWithTag("TutorialPanel").GetComponent<PanelBehavior>().ResetPosition();
    }

}
