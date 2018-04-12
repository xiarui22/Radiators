using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRaditors : MonoBehaviour {

    //public GameObject manager;

    public bool Escaped;
	// Use this for initialization
	void Start () {
        Escaped = false;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        //print(other.tag);
        if(other.tag == "radiator")
        {
            Escaped = true;

        }
    }
}
