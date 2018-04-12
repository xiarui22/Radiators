using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralPeople : Characters {

    bool onHealHp;
    bool onHealPain;
    
    public int attack;

    public int speed;

    private Vector3 targetPos;

    public GameObject highlight;

	// Use this for initialization
	new void Start () {
        base.Start();
		team = 0;
        onHealHp = false;
        onHealPain = false;
        
        speed = 5;
        //hp = 50;
        GetComponent<Brain>().SetTarget(transform.position);
    }
	
	// Update is called once per frame
	void Update () {
          
    }

    public void HighLight(bool on) {
        if (highlight != null)
        {
            if (on)
            {
                highlight.SetActive(true);
                
            }
            else
            {

                highlight.SetActive(false);
            }
        }
    }

    public override int GetMaxHP()
    {
        return 100;
    }

    public override int GetMaxPAIN()
    {
        return 100;
    }

    public void heal()
    {
        if (hp < GetMaxHP() && onHealHp == false)
        {
            onHealHp = true;
            StartCoroutine("HealHp");
        }
        if (pain > 0 && onHealPain == false)
        {
            onHealPain = true;
            StartCoroutine("HealPain");
        }
    }

    public void Move(Vector3 targetPos) {
        GetComponent<Brain>().SetTarget(targetPos);
    }

    public void MeleeAttack(Characters target) {
        target.GetComponent<Characters>().GetDamage(attack);
    }

    IEnumerator HealHp()
    {
        hp += 1;
        yield return new WaitForSeconds(2);
        onHealHp = false;
    }

    IEnumerator HealPain()
    {
        pain -= 1;
        yield return new WaitForSeconds(5);
        onHealPain = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Fire" && gameObject.tag == "civilian")
        {
            other.gameObject.GetComponent<Fire>().getDamaged();
        }
    }
}
