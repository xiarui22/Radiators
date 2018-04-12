using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour {
    public int health;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(int dam) {
        health -= dam;
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}
