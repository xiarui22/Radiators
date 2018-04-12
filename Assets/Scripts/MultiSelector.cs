using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera), typeof(LineRenderer))]
public class MultiSelector : MonoBehaviour
{
    public LayerMask terrain;
    public float defaultPlaneHeight = 1.0f;

    public List<GameObject> selected = new List<GameObject>();

    private Camera cam;
    private LineRenderer line;

    private Vector3 startPoint;
    private Vector3 endPoint;

    private Vector3[] rectPoints = new Vector3[4];

    private PlayerController pctrl;

    LayerMask mask = 0xFFFF;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        line = GetComponent<LineRenderer>();
        pctrl = GetComponent<PlayerController>();
        line.loop = true;
        line.useWorldSpace = true;
        line.positionCount = 4;
        line.enabled = false;

        rectPoints[0].y = defaultPlaneHeight;
        rectPoints[1].y = defaultPlaneHeight;
        rectPoints[2].y = defaultPlaneHeight;
        rectPoints[3].y = defaultPlaneHeight;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray r = cam.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit hit;
        Vector3 curWorldPos = Vector3.zero;

        if (Physics.Raycast(r, out hit, float.MaxValue, terrain))
        {
            curWorldPos = hit.point;
        }
        else
        {
            float t = (defaultPlaneHeight - r.origin.y) / r.direction.y;
            curWorldPos = r.origin + r.direction * t;
        }

        if (Input.GetMouseButtonDown(0))
        {
            startPoint = curWorldPos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            line.enabled = false;
            pctrl.SelectCharacter(selected);
        }
        else if (Input.GetMouseButton(0))
        {
            endPoint = curWorldPos;
            Select();
        }

    }

    void Select()
    {
        selected.Clear();

        if (Vector2.Distance(startPoint, endPoint) <= 0.5)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                if (hit.collider.tag == "civilian" || hit.collider.tag == "technician" || hit.collider.tag == "guard")
                {
                    selected.Add(hit.collider.gameObject);
                }
            }
        }
        else
        {
            GeneralPeople[] people = FindObjectsOfType<GeneralPeople>();
            Vector2 st = new Vector2(Mathf.Min(startPoint.x, endPoint.x), Mathf.Min(startPoint.z, endPoint.z));
            Vector2 ed = new Vector2(Mathf.Max(startPoint.x, endPoint.x), Mathf.Max(startPoint.z, endPoint.z));

            rectPoints[0].x = st.x;
            rectPoints[0].z = st.y;

            rectPoints[1].x = st.x;
            rectPoints[1].z = ed.y;

            rectPoints[2].x = ed.x;
            rectPoints[2].z = ed.y;

            rectPoints[3].x = ed.x;
            rectPoints[3].z = st.y;

            line.SetPositions(rectPoints);
            line.enabled = true;

            foreach (GeneralPeople p in people)
            {
                Vector3 pos = p.transform.position;
                if (pos.x <= ed.x && pos.x >= st.x && pos.z <= ed.y && pos.z >= st.y)
                {
                    selected.Add(p.gameObject);
                }
            }
        }        
    }
}
