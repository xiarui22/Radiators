using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public GameObject[] RadiatorInLevel;

    public GameObject[] FireInLevel;

    int FireNum;

    public bool win;

    public Text GameResult;

    public GameObject CheckEscape;

    public static bool start = false;

    // Use this for initialization
    void Start () {
        win = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!start)
            return;

        int RadiatorNum = 0;

        foreach(GameObject Radiator in GameObject.FindGameObjectsWithTag("radiator"))
        {
            if (Radiator.GetComponent<Brain>().isFree)
                RadiatorNum++;
        }

        FireInLevel = GameObject.FindGameObjectsWithTag("Fire");
        FireNum = FireInLevel.Length;

        int brokenGenerator = 0;

        foreach (GameObject generator in GameObject.FindGameObjectsWithTag("powerSupply"))
        {
            if (generator.GetComponent<PowerSupply>().isBroken)
                brokenGenerator++;
        }

        if (RadiatorNum <= 0 && FireNum <= 0 && brokenGenerator <= 0)
        {
            win = true;
            ShowGameResult(win);
        }

        if (CheckEscape.GetComponent<CheckRaditors>().Escaped)
        {
            win = false;
            ShowGameResult(win);
        }
    }

    void ShowGameResult(bool win)
    {
        GameResult.gameObject.SetActive(true);
        if (win)
        {
            GameResult.GetComponent<Text>().text = "You win";
        }
        else
        {
            GameResult.GetComponent<Text>().text = "You lose";
        }
    }

    void CloseGameResult()
    {
        GameResult.gameObject.SetActive(false);
    }
}  
