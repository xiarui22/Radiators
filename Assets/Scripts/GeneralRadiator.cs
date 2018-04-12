using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralRadiator : Characters {

	// Use this for initialization
	new void Start () {
        base.Start();
        //StartCoroutine(getBigger());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator getBigger()
    {
        while (true)
        {
            hp += 1;
            yield return new WaitForSeconds(2);
        }
    }

    public override int GetMaxHP()
    {
        return 200;
    }

    public override int GetMaxPAIN()
    {
        return 150;
    }
}
