using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PrefabGrappin : MonoBehaviour
{
    #region condition
    public bool isGrabing = false;
    public bool moveObject = false;
    #endregion

    #region Object/component
    [HideInInspector] Rigidbody2D rgbd;
    [HideInInspector] GameObject objectGrabed;
    [HideInInspector] public Rigidbody2D objectGrabedRgbd;
    [HideInInspector] Vector2 PointImpact;
    [SerializeField] Transform GrabPoint;
    [SerializeField] GameObject BaseGrab;
    [HideInInspector] Grappin Grappin;
    #endregion

    private void Start()
    {
        rgbd = gameObject.GetComponent<Rigidbody2D>();
        Grappin = BaseGrab.GetComponent<Grappin>();
    }

    private void Update()
    {
        if (!Grappin.isCanceling)
        {
            if (isGrabing)
            {
                transform.position = PointImpact;
                
            }
            else if ((!Grappin.isGrabling) && (!Grappin.isRetracting) && (!Grappin.isCanceling))
            {
                transform.position = GrabPoint.position;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Grappable"))
        {
            isGrabing = true;
            rgbd.velocity = new Vector2 (0.0f,0.0f);
            PointImpact = transform.position;
            objectGrabed = collision.gameObject;
            if (objectGrabed.GetComponent<Rigidbody2D>() == null)
            {
                moveObject = false;
            }
            else
            {
                moveObject = true;
                objectGrabedRgbd = objectGrabed.GetComponent<Rigidbody2D>();
            }
            Grappin.isLaunched = false;
            Grappin.SetDistanceJointPosition(PointImpact);
        }
    }

}
