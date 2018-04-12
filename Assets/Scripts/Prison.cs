using System.Collections.Generic;
using UnityEngine;

public class Prison : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "radiator")
        {
            //GameObject[] fences = GameObject.FindGameObjectsWithTag("fence");
            //for(int i = 0; i < fences.Length; i++)
            //{
            //    Destroy(fences[i]);
            //}

            other.GetComponent<Brain>().bigRadState = BigRadState.HELPING_OTHERS;

        }
    }
}
