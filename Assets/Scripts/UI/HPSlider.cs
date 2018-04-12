using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class HPSlider : MonoBehaviour {

    public Slider HpBar;
    private Characters owner;

    // Use this for initialization
    void Start ()
    {
        owner = null;
        owner = GetComponent<Characters>();

        Image image = HpBar.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();

        if (owner.team == 1)
        {
            image.sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Arts/Sprites/GreenBloodBar.png", typeof(Sprite));
        }
        else if (owner.team == 2)
        {
            image.sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Arts/Sprites/bloodBar.png", typeof(Sprite));
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        HpBar.value = (float)owner.hp / (float)owner.GetMaxHP();

        // TODO

        //float offset = GetComponent<MeshFilter>().mesh.bounds.size.y;

        //HpBar.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0f, offset*10f, 0f) + Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, offset, 0f)));
        //HpBar.transform.position = Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(transform.position));
    }
}
