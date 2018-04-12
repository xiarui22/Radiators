using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour {

    public GameObject mainEnemy;

    public GameObject lab;
    public GameObject labBeforeExplosion;

    public GameObject cam;
    

    // public GameObject fire;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "civilian")
        {
            Radiate(other.gameObject);
        }
    }

    public GameObject Radiate(GameObject civilian)
    {
        Destroy(civilian);

        Invoke("damagePower", 3); //open event

        return mainEnemy;
    }

    void damagePower()
    {
        GameManager.start = true;
        
        lab.SetActive(true);

        foreach (GameObject power in GameObject.FindGameObjectsWithTag("powerSupply"))
        {
            power.GetComponent<PowerSupply>().damage();
        }

        labBeforeExplosion.SetActive(false);
        mainEnemy.SetActive(true);
    }
}
