using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class Grappin : MonoBehaviour
{
    #region rewired
    private Player player;
    private int playerId = 0;
    #endregion

    #region Objet/Component
    [SerializeField] public GameObject Body;
    [SerializeField] GameObject Grab;
    //[SerializeField] GameObject Aim;
    [HideInInspector] LineRenderer line;
    [HideInInspector] DistanceJoint2D distanceJoint;
    [HideInInspector] Rigidbody2D rgbd;
    [HideInInspector] Rigidbody2D GrabRgbd;
    [HideInInspector] move move;
    [HideInInspector] PrefabGrappin prefab;
    #endregion

    #region parametreGrab
    [SerializeField] float maxDistance = 10f;
    //[SerializeField] float grappleSpeed = 10f;
    [SerializeField] float grappleShootSpeed = 20f;
    [SerializeField] Transform GrabPosition;
    [HideInInspector] Vector2 direction;
    [HideInInspector] Vector2 mousePos;
    #endregion

    #region conditions
    public bool isGrabling = false;
    public bool isRetracting = false;
    public bool isCanceling = false;
    public bool isLaunched = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerId);

        line = gameObject.GetComponent<LineRenderer>();
        distanceJoint = Body.GetComponent<DistanceJoint2D>();
        rgbd = Body.GetComponent<Rigidbody2D>();
        move = Body.GetComponent<move>();
        prefab = Grab.GetComponent<PrefabGrappin>();
        GrabRgbd = Grab.GetComponent<Rigidbody2D>();

        line.enabled = false;
        line.positionCount = 2;
        distanceJoint.enabled = false;
        distanceJoint.anchor = new Vector2(0.5f,0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        DirectionGrappin();
        if (isGrabling)
        {
            SetLinePosition();
        }
        if (isLaunched)
        {
            MaxDistance();
        }
        if (isRetracting)
        {
            Retract();
        }
    }

    private void SetLinePosition()
    {
        line.SetPosition(0, GrabPosition.position);
        line.SetPosition(1, Grab.transform.position);
    }

    public void SetDistanceJointPosition(Vector2 PositionB)
    {
        distanceJoint.enabled = true;
        distanceJoint.connectedAnchor = PositionB;
    }

private void Inputs()
    {
        // lance le grappin quand il est ranger
        if ((!isGrabling) && (player.GetButtonDown("Fire")) && (!isRetracting) && (!isCanceling))
        {
            StartGrable();
        }

        // retract le grappin petit a petit tant que la touche est pressé
        else if ((isGrabling) && (!isLaunched) && (player.GetButtonDown("Fire")) && (!isRetracting) && (!isCanceling))
        {
            isRetracting = true;
        }
        else if ((isGrabling) && (!isLaunched) && (player.GetButtonUp("Fire")) && (isRetracting) && (!isCanceling))
        {
            isRetracting = false;
        }

        // Rappel le grapin
        else if ((isGrabling) && (!isLaunched) && (player.GetButtonDown("Cancel")) && (!isRetracting) && (!isCanceling))
        {
            StartCoroutine(Cancel());
        }
    }

    private void DirectionGrappin() 
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - (Vector2)transform.position;
        gameObject.transform.right = direction;
    }
    private void StartGrable()
    {
        isLaunched = true;
        isGrabling = true;
        line.enabled = true;

        //Vector2 direction = Aim.transform.position - transform.position;

        Grab.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        GrabRgbd.velocity = direction * grappleShootSpeed;
        


    }

    private void MaxDistance()
    {
        float distance = Mathf.Sqrt((GrabPosition.position.x - Grab.transform.position.x) * (GrabPosition.position.x - Grab.transform.position.x) + (GrabPosition.position.y - Grab.transform.position.y) * (GrabPosition.position.y - Grab.transform.position.y));

        if (distance > maxDistance) 
        {
            isLaunched = false;
            StartCoroutine(Cancel());
        }
    }

    IEnumerator Cancel()
    {
        isCanceling = true;
        prefab.isGrabing = false;
        distanceJoint.enabled = false;
        rgbd.gravityScale = 1;
        float t = 0;
        float time = 10;

        Vector2 newPos;

        for (; t < time; t += grappleShootSpeed * Time.deltaTime)
        {
            newPos = Vector2.Lerp(Grab.transform.position, GrabPosition.position, t / time);
            Grab.transform.position = newPos;
            SetLinePosition();
            yield return null;
        }

        line.enabled = false;
        isCanceling = false;
        isGrabling = false;
    }

    private void Retract()
    {
        if (prefab.moveObject) 
        {
            Vector2 direction = distanceJoint.anchor - distanceJoint.connectedAnchor;
            prefab.objectGrabedRgbd.velocity = direction;
        }
        else 
        {
            Vector2 direction = distanceJoint.connectedAnchor - distanceJoint.anchor;
            rgbd.velocity = direction;
        }
    }
}