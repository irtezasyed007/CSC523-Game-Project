using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBehavior : MonoBehaviour {
    private Vector3 initialPosition;
    private Vector3 hidingPosition = new Vector3(2000, 2000);
    private bool waitOneClick;
    // Use this for initialization
    void Start() {
        initialPosition = this.transform.position;
        this.transform.position = hidingPosition;
        waitOneClick = true;
    }

    // Update is called once per frame
    void Update() {
        if (this.transform.position == initialPosition && Input.GetMouseButtonDown(0))
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
        this.transform.position = initialPosition;
    }

    private void OnMouseDown()
    {
        this.transform.position = hidingPosition;
    }
}
