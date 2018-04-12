using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour {

    public void StartDemo()
    {
        SceneManager.LoadScene("Darren");
    }
}
