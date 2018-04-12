using UnityEngine;

public abstract class Characters : MonoBehaviour {

    public int hp;

    public sbyte team;

    public int pain;


    // Use this for initialization
    protected void Start () {
        hp = GetMaxHP();
        pain = GetMaxPAIN();
    }
	
	// Update is called once per frame
	void Update () {
       // print(hp);
	}

    abstract public int GetMaxHP();


    public void GetDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    abstract public int GetMaxPAIN();

   

}
