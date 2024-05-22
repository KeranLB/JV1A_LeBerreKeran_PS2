using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    /*
    public Camera mainCamera;
    public LineRenderer _linerRenderer;
    public DistanceJoint2D _distanceJoint;
    */

    [SerializeField] LayerMask grapplableMask;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] float grappleSpeed = 10f;
    [SerializeField] float grappleShootSpeed = 20f;
    [SerializeField] GameObject Grab;
    [SerializeField] GameObject Body;

    [HideInInspector] Rigidbody2D rgbd;
    [HideInInspector] Vector2 PointGrab;
    [HideInInspector] bool isGrappling = false;
    [HideInInspector] bool grabLaunch = false;
    [HideInInspector] public bool retracting = false;

    Vector2 target;

    public bool isGrabing;

    public GameObject grappin;
    public DistanceJoint2D _distanceJoint;
    public float grappinSpeed;
    public LineRenderer line; 
    public Transform ShootPoint;
    Vector2 Direction;
    GameObject Target;
    void Start()
    {
        _distanceJoint.enabled = false;
        line.enabled = false;
        isGrabing = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 mousePos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _linerRenderer.SetPosition(0, mousePos);
            _linerRenderer.SetPosition(1, transform.position);
            _distanceJoint.connectedAnchor = mousePos;
            _distanceJoint.enabled = true;
            _linerRenderer.enabled = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _distanceJoint.enabled = false;
            _linerRenderer.enabled = false;
        }
        if (_distanceJoint.enabled)
        {
            _linerRenderer.SetPosition(1, transform.position);
        }
        */

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Direction = mousePos - (Vector2)transform.position;
        ShootPoint.right = Direction;
        if ((Input.GetMouseButtonDown(0)) && (isGrabing == false))
        {
            isGrabing = true;
            GameObject grappinIns = Instantiate(grappin, ShootPoint.position, Quaternion.identity);
            grappinIns.GetComponent<Rigidbody2D>().velocity = Direction * grappinSpeed;
            grappinIns.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg);
        }
        else if ((Input.GetMouseButtonDown(0)) && (isGrabing == true))
        {
            float tmp = _distanceJoint.distance;
        }
        if (Target != null)
        {
            line.SetPosition(0, ShootPoint.position);
            line.SetPosition(1, Target.transform.position);

        }
    }
    public void TargetHit(GameObject hit)
    {
        Target = hit;
        line.enabled = true;
        _distanceJoint.enabled = true;

        
    }











    IEnumerator Retract()
    {
        rgbd.gravityScale = 1;
        float t = 0;
        float time = 10;

        line.SetPosition(0, PointGrab);
        line.SetPosition(1, target);

        Vector2 newPos;

        for (; t < time; t += grappleShootSpeed * Time.deltaTime)
        {
            newPos = Vector2.Lerp(PointGrab, target, t / time);
            line.SetPosition(0, newPos);
            line.SetPosition(1, target);
            //distanceJoint.anchor = newPos;
            yield return null;
        }

        line.SetPosition(0, target);
        Grab.transform.position = PointGrab;
        retracting = false;
        isGrappling = false;
        line.enabled = false;
        grabLaunch = false;
    }
}

