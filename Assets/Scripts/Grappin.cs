using System.Collections;
using UnityEngine;
using Rewired;

public class Grappin : MonoBehaviour
{
    #region rewired
    private Player player;
    private int playerId = 0;
    #endregion

    #region Objet/Component
    [SerializeField] public GameObject Body;
    [SerializeField] public GameObject Grab;
    //[SerializeField] GameObject Aim;
    [HideInInspector] LineRenderer line;
    [HideInInspector] public DistanceJoint2D distanceJoint;
    [HideInInspector] public SpringJoint2D spring;
    [HideInInspector] Rigidbody2D rgbd;
    [HideInInspector] public Rigidbody2D GrabRgbd;
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

        rgbd = Body.GetComponent<Rigidbody2D>();
        move = Body.GetComponent<move>();
        prefab = Grab.GetComponent<PrefabGrappin>();
        GrabRgbd = Grab.GetComponent<Rigidbody2D>();

        line = gameObject.GetComponent<LineRenderer>();
        line.enabled = false;
        line.positionCount = 2;

        distanceJoint = Body.GetComponent<DistanceJoint2D>();
        distanceJoint.enabled = false;
        distanceJoint.anchor = new Vector2(0.0f,0.0f);
        distanceJoint.connectedBody = GrabRgbd;
        distanceJoint.connectedAnchor = new Vector2(0.0f, 0.0f);

        spring = Body.GetComponent<SpringJoint2D>();
        spring.enabled = false;
        spring.connectedAnchor = new Vector2(0.0f, 0.0f);
        spring.connectedBody = GrabRgbd;
        spring.distance = 0.0f;
        spring.anchor = new Vector2(0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
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

    private void Inputs()
    {
        // retract le grappin petit a petit tant que la touche est pressé
        if ((isGrabling) && (!isLaunched) && (player.GetButtonDown("Fire")) && (!isRetracting) && (!isCanceling))
        {
            spring.enabled = true;
            isRetracting = true;
            
            //distanceJoint.enabled = false;
        }
        else if ((isGrabling) && (!isLaunched) && (player.GetButtonUp("Fire")) && (isRetracting) && (!isCanceling))
        {
            isRetracting = false;
            spring.enabled = false;
            //rgbd.bodyType = RigidbodyType2D.Dynamic;
        }

        // Rappel le grapin
        else if ((isGrabling) && (!isLaunched) && (player.GetButtonDown("Cancel")) && (!isRetracting) && (!isCanceling))
        {
            StartCoroutine(Cancel());
        }
    }
    
    public void DirectionGrappin(Vector2 direction) 
    {
        /*
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - (Vector2)transform.position;
        */
        gameObject.transform.right = direction;
    }
    
    public void StartGrable(Vector2 direction)
    {
        isLaunched = true;
        isGrabling = true;
        line.enabled = true;

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

    public void StartCancel()
    {
        StartCoroutine(Cancel());
    }
    IEnumerator Cancel()
    {
        if (prefab.moveObject)
        {
            Grab.transform.position = spring.anchor;
            Grab.SetActive(true);
            spring.connectedBody = GrabRgbd;
            distanceJoint.connectedBody = GrabRgbd;
        }
        else
        {
            GrabRgbd.bodyType = RigidbodyType2D.Dynamic;
            Grab.transform.SetParent(null);
        }

        isCanceling = true;
        prefab.isGrabing = false;
        distanceJoint.enabled = false;
        spring.enabled = false;
        rgbd.gravityScale = 1;


        float t = 0;
        float time = 3;

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
            if (move.isGrounded)
            {
                //rgbd.bodyType = RigidbodyType2D.Static;
            }          
        }
        distanceJoint.anchor = spring.anchor;
    }
}