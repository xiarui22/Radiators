using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerController : MonoBehaviour {

    Color defaultLight;

    // Use this for initialization
    void Start()
    {
        defaultLight = RenderSettings.ambientLight;
    }

    // Update is called once per frame
    void Update()
    {
        int brokenNumber = 0;

        foreach(GameObject temp in GameObject.FindGameObjectsWithTag("powerSupply"))
        {
            PowerSupply powerSupply = temp.GetComponent<PowerSupply>();

            if(powerSupply.isBroken)
            {
                brokenNumber++;
            }
        }

        if (brokenNumber > 0)
        {
            breakDown(brokenNumber);
        }
        else
        {
            turnOn();
        }

        //else if (hp > 0 && !fixedPower)
        //{
        //    turnOn();
        //}
        //else if (hp > 0 && fixedPower)
        //{
        //    breakDown();
        //    if (hp == GetMaxHP())
        //    {
        //        turnOn();
        //        fixedPower = false;
        //    }
        //}

        //print(hp);
    }

    void breakDown(int count)
    {
        RenderSettings.ambientLight = Color.black;

        foreach (GameObject light in GameObject.FindGameObjectsWithTag("light"))
        {
            light.GetComponent<Light>().intensity = 1 - count * 0.13f;
        }

        foreach (GameObject door in GameObject.FindGameObjectsWithTag("door"))
        {
            door.GetComponent<AutoDoor>().breakDown();
        }

        foreach (GameObject MedicalRoom in GameObject.FindGameObjectsWithTag("medical"))
        {
            MedicalRoom.GetComponent<MedicalRoom>().breakDown();
        }

        foreach (GameObject PersonalLight in GameObject.FindGameObjectsWithTag("personLight"))
        {
            PersonalLight.GetComponent<Light>().intensity = 5;
        }
    }

    public void turnOn()
    {
        RenderSettings.ambientLight = defaultLight;
        
        foreach (GameObject light in GameObject.FindGameObjectsWithTag("light"))
        {
            light.GetComponent<Light>().intensity = 1;
        }

        foreach (GameObject door in GameObject.FindGameObjectsWithTag("door"))
        {
            door.GetComponent<AutoDoor>().recover();
        }

        foreach (GameObject MedicalRoom in GameObject.FindGameObjectsWithTag("medical"))
        {
            MedicalRoom.GetComponent<MedicalRoom>().recover();
        }

        foreach (GameObject PersonalLight in GameObject.FindGameObjectsWithTag("personLight"))
        {
            PersonalLight.GetComponent<Light>().intensity = 0;
        }
    }

    //public void fix()
    //{
    //    if (hp < GetMaxHP() && onRegenHealth == false)
    //    {
    //        onRegenHealth = true;
    //        StartCoroutine("RegenHealth");
    //    }
    //    fixedPower = true;
    //}

}
