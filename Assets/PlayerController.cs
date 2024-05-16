using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /*
    public Camera mainCamera;
    public LineRenderer _linerRenderer;
    public DistanceJoint2D _distanceJoint;
    */

    public GameObject grappin;
    public DistanceJoint2D _distanceJoint;
    public float grappinSpeed;
    public LineRenderer line; 
    public Transform ShootPoint;
    Vector2 Direction;
    GameObject target;
    void Start()
    {
        _distanceJoint.enabled = false;
        line.enabled = false;
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
        if (Input.GetMouseButtonDown(0))
        {
            GameObject grappinIns = Instantiate(grappin, ShootPoint.position, Quaternion.identity);
            grappinIns.GetComponent<Rigidbody2D>().velocity = Direction * grappinSpeed;
            grappinIns.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg);
        }
        if (target != null)
        {
            line.SetPosition(0, ShootPoint.position);
            line.SetPosition(1, target.transform.position);

        }
    }
    public void TargetHit(GameObject hit)
    {
        target = hit;
        line.enabled = true;
        _distanceJoint.enabled = true;
    }
}
