using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSupply : Characters {

    //public GameObject[] lights;
    //public GameObject[] doors;

    List<GameObject> onFireObjects = new List<GameObject>();

    public bool isBroken
    {
        get
        {
            return hp <= 80;
        }
    }

    bool onRegenHealth = false;
    bool fixedPower;

    // Use this for initialization
    new void Start () {
        base.Start();

        StartCoroutine(fix());
    }
	
	// Update is called once per frame
	void Update () {
     
	}

    public override int GetMaxHP()
    {
        return 100;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            onFireObjects.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            onFireObjects.Remove(other.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "technician")
        {
            
        }
        if (other.tag == "radiator")
        {
           // damage();
        }
    }

    public void damage()
    {
        hp = Random.Range(0, 50);
    }

    public override int GetMaxPAIN()
    {
        return 100;
    }

    IEnumerator fix()
    { 
        while (true)
        {
            foreach (GameObject onFireObject in onFireObjects)
            {
                if (onFireObject.tag == "technician")
                {
                    hp = Mathf.Min(100, hp + 3);
                }
            }
            yield return new WaitForSeconds(1);
        }
    }
}
