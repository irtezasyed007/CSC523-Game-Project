using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemSorting : MonoBehaviour {

    public ParticleSystem gameObject;
	// Use this for initialization
	void Start () {
       gameObject.GetComponent<Renderer>().sortingLayerName = "Foreground";
    }

    // Update is called once per frame
    void Update () {
		
	}
}
