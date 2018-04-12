using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public List<GameObject> curChars;

    public GameObject cam;

    public GameObject hightlight;
    public GameObject moveHighlight;

    LayerMask mask = 0xFFFF;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        //    {
        //        if (hit.collider.tag == "civilian" || hit.collider.tag == "technician" || hit.collider.tag == "guard")
        //        {
        //            if (curChar != null)
        //            {
        //                curChar.GetComponentInChildren<TextMesh>().color = Color.black;
        //                hightlight.GetComponent<SpriteRenderer>().enabled = false;
        //                hightlight.GetComponent<CharHighlight>().target = null;
        //            }
        //            curChar = hit.collider.gameObject;
        //            curChar.GetComponentInChildren<TextMesh>().color = Color.white;
        //            hightlight.GetComponent<SpriteRenderer>().enabled = true;
        //            hightlight.GetComponent<CharHighlight>().target = curChar;
        //        }
        //        else
        //        {
        //            if (curChar != null)
        //            {
        //                curChar.GetComponentInChildren<TextMesh>().color = Color.black;
        //                hightlight.GetComponent<SpriteRenderer>().enabled = false;
        //                hightlight.GetComponent<CharHighlight>().target = null;
        //            }
        //            curChar = null;
        //        }

        //    }
        //}

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, 0xFFFF))
            {
                if (curChars.Count!=0) {
                    foreach (GameObject go in curChars) {
                        if (null != go && null != go.transform)
                        {
                            go.GetComponent<GeneralPeople>().Move(hit.point);
                            moveHighlight.GetComponent<MoveHighlight>().ChangePosition(hit.point);
                        }
                    }
                }
            }
        }// End if mouse down 1
    }

    public void SetTargetPosition(Vector3 pos)
    {
        //TODO
        //use AI to move
    }

    public void SelectCharacter(List<GameObject> targets)
    {
        if (curChars.Count != 0)
        {
            foreach (GameObject go in curChars) {
                if (null != go && null != go.transform)
                {
                    go.GetComponentInChildren<TextMesh>().color = Color.black;
                    go.GetComponent<GeneralPeople>().HighLight(false);
                    //hightlight.GetComponent<SpriteRenderer>().enabled = false;
                    // hightlight.GetComponent<CharHighlight>().target = null;
                }
            }            
        }
        curChars.Clear();
        foreach (GameObject go in targets) {
            curChars.Add(go);
        }
        if (curChars.Count != 0)
        {
            foreach (GameObject go in curChars) {
                if (null != go && null != go.transform)
                {
                    go.GetComponentInChildren<TextMesh>().color = Color.white;
                    go.GetComponent<GeneralPeople>().HighLight(true);
                }
            }
        }

    }
}
