using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalRoom : MonoBehaviour {

    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   
    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<GeneralPeople>())
        {
            other.GetComponent<GeneralPeople>().heal();
            //print("G");
        }
        
    }

    public void breakDown()
    {
        gameObject.GetComponent<BoxCollider>().isTrigger = false;
        
    }

    public void recover()
    {
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }
}
