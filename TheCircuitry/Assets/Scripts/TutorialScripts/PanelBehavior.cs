using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBehavior : MonoBehaviour {
    private Vector3 initialPositionRegularScreen;
    private Vector3 initialPositionFullScreen;
    private Vector3 hidingPosition = new Vector3(2000, 2000);
    private bool waitOneClick;
    // Use this for initialization
    void Start() {
        initialPositionRegularScreen = this.transform.position;
        Debug.Log(initialPositionRegularScreen);
        this.transform.position = hidingPosition;
        waitOneClick = true;
    }

    // Update is called once per frame
    void Update() {
        if (Screen.fullScreen)
        {
            //GameObject.Find("Canvas/BackgroundImage").transform.position
        }
        if (this.transform.position == initialPositionRegularScreen && Input.GetMouseButtonDown(0))
        {
            if(waitOneClick)
            {
                waitOneClick = false;
            }
            else
            {
                this.transform.position = hidingPosition;
                waitOneClick = true;
            }
        }
    }

    public void ResetPosition()
    {
        this.transform.position = initialPositionRegularScreen;
    }

    private void OnMouseDown()
    {
        this.transform.position = hidingPosition;
    }
}
