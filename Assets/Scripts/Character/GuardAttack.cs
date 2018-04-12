using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAttack : MonoBehaviour
{
    public GeneralRadiator target;

    public int attack;

    public float attackCD;

    public float attackRange;

    public float reloadTime;

    private float cdTimer;

    private float lineTimer;

    private int ammoCount;

    private bool isReloading;

    public bool IsReadyToAttack
    {
        get
        {
            return (cdTimer <= 0 && ammoCount != 0);
        }
    }

    // Use this for initialization
    void Start()
    {
        ammoCount = 4;
        isReloading = false;
        cdTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (cdTimer > 0)
        {
            cdTimer -= Time.deltaTime;
            if (cdTimer <= 0 && isReloading) {
                isReloading = false;
                ammoCount = 4;
            }
        }

        if (lineTimer > 0)
        {
            lineTimer -= Time.deltaTime;
            if (lineTimer <= 0)
            {
                GetComponent<LineRenderer>().positionCount = 0;
            }
        }
    }

    void SetTarget(GeneralRadiator tar)
    {
        target = tar;

    }

    public bool Attack()
    {
        //attack the target
        if (target != null && Vector3.Distance(transform.position, target.transform.position) <= attackRange)
        {
            target.GetDamage(attack);
            //inst a line from the guard to radiator
            LineRenderer line = GetComponent<LineRenderer>();
            line.positionCount = 2;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, target.transform.position);
            cdTimer = attackCD;
            lineTimer = 0.5f;
            ammoCount--;
            if (ammoCount == 0)
            {
                isReloading = true;
                cdTimer = reloadTime;
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
