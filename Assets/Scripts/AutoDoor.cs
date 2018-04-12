using UnityEngine;

public class AutoDoor : Characters, Interactions
{
    public bool keepOpen;

    Vector3 OrigiPos;

    [SerializeField]
    private bool isPowered;

	// Use this for initialization
	new void Start () {
        base.Start();
        OrigiPos = gameObject.transform.localPosition;

        if(keepOpen)
        {
            Open(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (hp <= 0)
            Destroy(gameObject);
    }

    #region interactions
    public void turnOn()
    {
        isPowered = true;
        //gameObject.transform.position = OrigiPos + new Vector3(1, 0, 0);
        //animation
    }
    public void turnOff()
    {
        isPowered = false;
        //gameObject.transform.position = OrigiPos;
        //animation
    }
    public void fix()
    {
       
    }
  
    #endregion
    
    public override int GetMaxHP()
    {
        return 100;
    }

    public void breakDown()
    {
        turnOff();

        //Destroy(gameObject.GetComponent<Rigidbody>());
    }

    public void recover()
    {
        turnOn();
    }

    void OnTriggerStay(Collider other)
    {
        if (keepOpen)
            return;

        //Debug.Log(other.tag);
        if (other.tag == "guard")
        {
            //turnOn();
            Open();
            Invoke("Close", 4);
        }
        else if (other.tag == "radiator")
        {
            GetDamage(1); //change according to skills
        }
        else if (other.tag == "technician")
        {
            Open(true);
        }
    }

    public override int GetMaxPAIN()
    {
        return 100;
    }

    /// <summary>
    /// Open the door is it is closed.
    /// </summary>
    protected void Open(bool overRide = false)
    {
        if (isPowered || overRide)
        {
            transform.localPosition = new Vector3(0.25f, 0, -2.5f);
        }
    }

    /// <summary>
    /// Close the door if it is open.
    /// </summary>
    public void Close()
    {
        if (isPowered)
        {
            transform.localPosition = OrigiPos;
        }
    }
}
