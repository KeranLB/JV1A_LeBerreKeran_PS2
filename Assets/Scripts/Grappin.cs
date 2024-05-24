using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Grappin : MonoBehaviour
{
    #region rewired
    private Player player;
    private int playerId = 0;
    #endregion

    #region Objet/Component
    [SerializeField] GameObject Body;
    [SerializeField] GameObject Grab;
    [SerializeField] GameObject Aim;
    [HideInInspector] LineRenderer line;
    [HideInInspector] DistanceJoint2D distanceJoint;
    [HideInInspector] Rigidbody2D rgbd;
    [HideInInspector] move move;
    [HideInInspector] PrefabGrappin prefab;
    #endregion

    #region parametreGrab
    [SerializeField] float maxDistance = 10f;
    [SerializeField] float grappleSpeed = 10f;
    [SerializeField] float grappleShootSpeed = 20f;
    [SerializeField] Vector2 GrabPosition;
    #endregion

    #region conditions
    [HideInInspector] bool isGrabling = false;
    [HideInInspector] bool isRetracting = false;
    [HideInInspector] bool isCanceling = false;
    [HideInInspector] bool isLaunched = false;
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

        line.enabled = false;
        distanceJoint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        SetLinePosition();
        if (isGrabling)
        {
            SetDistanceJointPosition();
        }
        if (isRetracting)
        {
            Retract();
        }
    }

    private void SetLinePosition()
    {
        line.SetPosition(0, GrabPosition);
        line.SetPosition(1, Grab.transform.position);
    }

    private void SetDistanceJointPosition()
    {
        distanceJoint.enabled = true;
        distanceJoint.anchor = GrabPosition;
        distanceJoint.connectedAnchor = Grab.transform.position;
    }
    private void Inputs()
    {
        if ((!isGrabling) && (player.GetButtonDown("Fire")) && (!isRetracting) && (!isCanceling))
        {
            isLaunched = true;
            isGrabling = true;
            StartGrable();
        }

        else if ((isGrabling) && (isLaunched) && (player.GetButtonDown("Fire")) && (!isRetracting) && (!isCanceling))
        {
            isRetracting = true;
        }
        else if ((isGrabling) && (isLaunched) && (player.GetButtonUp("Fire")) && (isRetracting) && (!isCanceling))
        {
            isRetracting = false;
        }

        else if ((isGrabling) && (isLaunched) && (player.GetButtonDown("Fire")) && (!isRetracting) && (!isCanceling))
        {
            Cancel();
        }
    }

    private void StartGrable()
    {
        Vector2 direction = Aim.transform.position - transform.position;

        GameObject grappinIns = Instantiate(Grab, GrabPosition, Quaternion.identity);
        grappinIns.GetComponent<Rigidbody2D>().velocity = direction * grappleShootSpeed;
        grappinIns.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        float distance = Mathf.Sqrt((GrabPosition.x - Grab.transform.position.x) * (GrabPosition.x - Grab.transform.position.x) + (GrabPosition.y - Grab.transform.position.y) * (GrabPosition.y - Grab.transform.position.y));

        StartCoroutine(Grable());
    }
    IEnumerator Grable()
    {
        yield return null;
    }

    IEnumerator Cancel()
    {
        isGrabling = false;
        yield return null;
    }

    private void Retract()
    {

    }
}