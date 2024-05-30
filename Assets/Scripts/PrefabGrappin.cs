using UnityEngine;

public class PrefabGrappin : MonoBehaviour
{
    #region condition
    public bool isGrabing = false;
    public bool moveObject = false;
    #endregion

    #region Object/component
    [HideInInspector] Rigidbody2D rgbd;
    [HideInInspector] public GameObject objectGrabed;
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
            if ((!Grappin.isGrabling) && (!Grappin.isRetracting) && (!Grappin.isCanceling))
            {
                transform.position = GrabPoint.position;
            }
        }
    }

    private void SetGrab(GameObject collision)
    {
        isGrabing = true;
        rgbd.velocity = new Vector2(0.0f, 0.0f);
        PointImpact = transform.position;
        Grappin.Grab.transform.SetParent(collision.transform);
        Grappin.isLaunched = false;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Grappable") && Grappin.isLaunched)
        {
            SetGrab(collision.gameObject);
            moveObject = false;

            Grappin.GrabRgbd.bodyType = RigidbodyType2D.Static;
            Grappin.distanceJoint.enabled = true;
        }        

        else if (collision.CompareTag("Object") && (Grappin.isLaunched))
        {
            SetGrab(collision.gameObject);
            moveObject = true;
            objectGrabedRgbd = collision.gameObject.GetComponent<Rigidbody2D>();
            Grappin.spring.connectedBody = objectGrabedRgbd;
            Grappin.distanceJoint.connectedBody = objectGrabedRgbd;
            Grappin.distanceJoint.enabled = true;
            Grappin.Grab.SetActive(false);
        }

        else if (collision.CompareTag("BreakWall") && collision.gameObject.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic)
        {
            SetGrab(collision.gameObject);
            moveObject = true;
            objectGrabedRgbd = collision.gameObject.GetComponent<Rigidbody2D>();
            Grappin.spring.connectedBody = objectGrabedRgbd;
            Grappin.distanceJoint.connectedBody = objectGrabedRgbd;
            Grappin.distanceJoint.enabled = true;
            Grappin.Grab.SetActive(false);
        }
    }

}
