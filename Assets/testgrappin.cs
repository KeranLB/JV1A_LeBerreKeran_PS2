using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testgrappin : MonoBehaviour
{
    public Rigidbody2D rgbd;
    public float horizontal;
    public bool jump;
    public bool isGrabing;
    public bool isJumping;
    public int jumpForce;
    public int moveSpeed;


    LineRenderer line;

    [SerializeField] LayerMask grapplableMask;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] float grappleSpeed = 10f;
    [SerializeField] float grappleShootSpeed = 20f;

    bool isGrappling = false;
    [HideInInspector] public bool retracting = false;

    Vector2 target;

    void Start()
    {
        jump = false;
        isJumping = true;
        isGrabing = false;


        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        if ((isJumping == false) && (Input.GetButtonDown("Jump")))
        {
            jump = true;
        }


        if ((Input.GetMouseButtonDown(0)) && (!isGrappling))
        {
            StartGrapple();
        }

        else if ((Input.GetMouseButtonDown(0)) && (isGrappling))
        {
            retracting = true;
        }
        
        if (isGrappling)
        {
            line.SetPosition(0, transform.position);
        }

        if (retracting)
        {
            rgbd.gravityScale = 0;
            Vector2 grapplePos = Vector2.Lerp(transform.position, target, grappleSpeed * Time.deltaTime);

            transform.position = grapplePos;

            line.SetPosition(0, transform.position);

            if (Vector2.Distance(transform.position, target) < 0.5f)
            {
                retracting = false;
                isGrappling = false;
                line.enabled = false;
                rgbd.gravityScale = 1;
            }
        }
    }

    private void FixedUpdate()
    {
        rgbd.velocity = new Vector2(horizontal * moveSpeed, rgbd.velocity.y);

        if (jump)
        {
            rgbd.AddForce(Vector2.up * jumpForce);
            jump = false;
            isJumping = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isJumping = false;
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

        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);

        Vector2 newPos;

        for (; t< time; t += grappleShootSpeed * Time.deltaTime)
        {
            newPos = Vector2.Lerp(transform.position, target, t / time);
            line.SetPosition(0, transform.position);
            line.SetPosition(1, newPos);
            yield return null;
        }

        line.SetPosition(1, target);

    }
}
