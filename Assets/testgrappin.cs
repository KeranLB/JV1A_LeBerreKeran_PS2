using System.Collections;
using UnityEngine;
using Rewired;

public class testgrappin : MonoBehaviour
{
    #region rewired
    private Player player;
    private int playerId = 0;
    #endregion




    LineRenderer line;
    DistanceJoint2D distanceJoint;
    public float distanceGrappin = 20f;

    [SerializeField] LayerMask grapplableMask;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] float grappleSpeed = 10f;
    [SerializeField] float grappleShootSpeed = 20f;
    [SerializeField] GameObject Grab;
    [SerializeField] GameObject Body;

    [HideInInspector] Rigidbody2D rgbd;
    [HideInInspector] Vector2 PointGrab;
    [HideInInspector] bool isGrappling = false;
    [HideInInspector] bool isRetracting = false;
    [HideInInspector] bool isCanceling = false;
    [HideInInspector] bool grabLaunch = false;

    Vector2 target;

    void Start()
    {
        player = ReInput.players.GetPlayer(playerId);

        line = GetComponent<LineRenderer>();
        distanceJoint = Body.GetComponent<DistanceJoint2D>();
        rgbd = Body.GetComponent<Rigidbody2D>();

        line.enabled = false;
        distanceJoint.enabled = false;
        distanceJoint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        GrabPosition();
        Inputs();
    }

    private void Inputs()
    {
        if ((Input.GetMouseButtonDown(0)) && (!isGrappling))
        {
            StartGrapple();
        }

        else if ((Input.GetMouseButtonDown(0)) && (isGrappling) && (!isCanceling) && (grabLaunch))
        {
            StartCoroutine(Retract());
        }

        else if ((Input.GetMouseButtonDown(1)) && (isGrappling) && (!isRetracting) && (grabLaunch))
        {
            StartCoroutine(CancelGrab());
        }
    }

    private void GrabPosition()
    {
        transform.position = new Vector2(Body.transform.position.x + 0.5f, Body.transform.position.y + 0.03f);

        PointGrab = new Vector2(transform.position.x + 0.35f, transform.position.y);

        if (distanceGrappin > Mathf.Sqrt((PointGrab.x - target.x) * (PointGrab.x - target.x) + (PointGrab.y - target.y) * (PointGrab.y - target.y)))
        {
            distanceJoint.anchor = new Vector2(transform.position.x, transform.position.y + 0.85f);
            distanceGrappin = distanceJoint.distance;
            
        }
        if (isGrappling)
        {
            line.SetPosition(0, PointGrab);
        }

        if (grabLaunch)
        {
            Grab.transform.position = target;
        }

        else
        {
            Grab.transform.position = PointGrab;
        }
    }

    private void StartGrapple()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, grapplableMask);

        if (hit.collider != null)
        {
            isGrappling = true;
            target = hit.point;
            line.enabled = true;
            line.positionCount = 2;

            StartCoroutine(Grapple());
        }
    }

    IEnumerator Grapple()
    {
        float t = 0;
        float time = 10;

        line.SetPosition(0, PointGrab);
        line.SetPosition(1, PointGrab);

        Vector2 newPos;

        for (; t < time; t += grappleShootSpeed * Time.deltaTime)
        {
            newPos = Vector2.Lerp(PointGrab, target, t / time);
            line.SetPosition(0, PointGrab);
            line.SetPosition(1, newPos);
            Grab.transform.position = newPos;
            yield return null;
        }
        distanceJoint.enabled = true;
        line.SetPosition(1, target);
        Grab.transform.position = target;
        distanceJoint.connectedAnchor = Grab.transform.position;
        distanceJoint.anchor = new Vector2(transform.position.x, transform.position.y + 0.85f);
        distanceGrappin = distanceJoint.distance;
        grabLaunch = true;
    }

    IEnumerator Retract()
    {
        isRetracting = true;
        distanceJoint.enabled = false;
        rgbd.gravityScale = 0;
        float t = 0;
        float time = 2;

        line.SetPosition(0, PointGrab);
        line.SetPosition(1, target);

        Vector2 newPos;

        for (; t < time; t += grappleSpeed * Time.deltaTime)
        {
            newPos = Vector2.Lerp(PointGrab, target, t / time);
            line.SetPosition(0, newPos);
            line.SetPosition(1, target);
            transform.position = new Vector2(newPos.x - 0.35f, newPos.y);
            Body.transform.position = new Vector2(transform.position.x - 0.5f, transform.position.y - 0.03f);
            yield return null;
        }

        line.SetPosition(0, target);
        Grab.transform.position = PointGrab;
        isGrappling = false;
        line.enabled = false;
        grabLaunch = false;
        rgbd.gravityScale = 1;
        isRetracting = false;
    }

    IEnumerator CancelGrab()
    {
        isCanceling = true;
        distanceJoint.enabled = false;
        rgbd.gravityScale = 1;
        float t = 0;
        float time = 10;

        line.SetPosition(0, PointGrab);
        line.SetPosition(1, target);

        Vector2 newPos;

        for (; t < time; t += grappleShootSpeed * Time.deltaTime)
        {
            newPos = Vector2.Lerp(target, PointGrab, t / time);
            line.SetPosition(0, PointGrab);
            line.SetPosition(1, newPos);
            Grab.transform.position = newPos;
            yield return null;
        }

        line.SetPosition(1, PointGrab);
        Grab.transform.position = PointGrab;
        isGrappling = false;
        line.enabled = false;
        grabLaunch = false;
        isCanceling = false;
    }
}
