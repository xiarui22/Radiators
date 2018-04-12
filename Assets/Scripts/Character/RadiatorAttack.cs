using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiatorAttack : MonoBehaviour {

    public GeneralPeople mainTarget;

    public GeneralPeople sideTarget;

    public int attack;

    public float pctToSide;

    public float attackCD;

    public float attackRange;

    private float cdTimer;

    private float lineTimer;

    int attackTime;

    public bool IsReadyToAttack
    {
        get
        {
            return (cdTimer <= 0);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (cdTimer > 0)
        {
            cdTimer -= Time.deltaTime;
        }

        if (lineTimer > 0) {
            lineTimer -= Time.deltaTime;
            if (lineTimer <= 0) {
                GetComponent<LineRenderer>().positionCount = 0;
            }
        }
    }

    public void SetTarget(GeneralPeople main, GeneralPeople side) {
        mainTarget = main;
        sideTarget = side;
    }

    public bool Attack() {
        if (mainTarget != null && Vector3.Distance(transform.position, mainTarget.transform.position) < attackRange)
        {
            attackTime = ++attackTime % 3;

            if (attackTime == 0)
            {
                int damage = (int)(attack * 2.5f);

                //attack main
                mainTarget.GetDamage(damage);
                LineRenderer line = GetComponent<LineRenderer>();

                //1 line;
                line.positionCount = 2;
                line.SetPosition(0, transform.position);
                line.SetPosition(1, mainTarget.transform.position);
                lineTimer = 0.5f;

                if (sideTarget != null && Vector3.Distance(mainTarget.transform.position, sideTarget.transform.position) < attackRange)
                {
                    sideTarget.GetDamage(Mathf.FloorToInt(damage * pctToSide));
                    //2 lines;
                    line.positionCount = 3;
                    line.SetPosition(2, sideTarget.transform.position);
                }
            } else
            {
                //attack main
                mainTarget.GetDamage(attack);
                LineRenderer line = GetComponent<LineRenderer>();

                //1 line;
                line.positionCount = 2;
                line.SetPosition(0, transform.position);
                line.SetPosition(1, mainTarget.transform.position);
                lineTimer = 0.5f;
            }

            cdTimer = attackCD;
            return true;
        }
        else {
            return false;
        }
    }

    public void Attack(Fence target)
    {
        target.TakeDamage(attack);
    }
}
