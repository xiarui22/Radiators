using UnityEngine;

public class CharHighlight : MonoBehaviour
{
    public GameObject target;

    private Vector2 targetPos;

	// Use this for initialization
	void Start () {
        this.GetComponent<SpriteRenderer>().enabled = false;
        targetPos = Vector2.zero;

    }

    void Update()
    {
        if(target != null)
        {
            targetPos.x = target.transform.position.x;
            targetPos.y = target.transform.position.z;

            ChangePosition(targetPos);
        }
    }

    public void ChangePosition(Vector2 pos)
    {
        this.gameObject.transform.position = new Vector3(pos.x, transform.position.y, pos.y);
    }
}// End of Char Highlight