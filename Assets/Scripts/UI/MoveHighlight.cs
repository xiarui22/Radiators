using System;
using System.Collections;
using UnityEngine;

public class MoveHighlight : MonoBehaviour
{
    private Vector2 targetPos;

    // Use this for initialization
    void Start()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
        targetPos = Vector3.zero;
    }

    //void Update()
    //{
    //    if (target != null)
    //    {
    //        targetPos.x = target.transform.position.x;
    //        targetPos.y = target.transform.position.z;

    //        ChangePosition(targetPos);
    //    }
    //}

    public void ChangePosition(Vector3 pos)
    {
        this.gameObject.transform.position = new Vector3(pos.x, transform.position.y, pos.z);
        this.gameObject.transform.localScale = Vector3.zero;
        this.GetComponent<SpriteRenderer>().enabled = true;
        StartCoroutine(Swoosh());
    }


    IEnumerator Swoosh()
    {
        for (int i = 1; i < 21; i+=2)
        {
            this.gameObject.transform.localScale = new Vector3((float)i / 10.0f, (float)i / 10.0f, 0);
            yield return new WaitForSeconds(.1f);
        }

        this.GetComponent<SpriteRenderer>().enabled = false;
    }

}// End of Char Highlight
