using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeThySelf : MonoBehaviour {

    public GameObject correspodingFence;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider Other)
    {

        Other.CompareTag("radiator");
        transform.gameObject.GetComponentInParent<Brain>().bigRadState = BigRadState.ESCAPING;
        gameObject.GetComponentInParent<Brain>().isFree = true;
        gameObject.GetComponentInParent<Brain>().bigRadState = BigRadState.ESCAPING;

        if ( null != correspodingFence && null != correspodingFence.transform )
        {
            Destroy(correspodingFence);
        }

        Destroy(gameObject);

    }
}
