using UnityEngine;
using Rewired;

public class move : MonoBehaviour
{
    public Rigidbody2D rgbd;
    public float horizontal;
    public bool jump;
    public bool isGrabing;
    public bool isJumping;
    public int jumpForce;
    #region rewired
    private Player player;
    private int playerId = 0;
    #endregion
    #region Move
    private Vector2 movement;
    public int moveSpeed;
    #endregion
    private void Start()
    {
        jump = false;
        isJumping = true;
        isGrabing = false;
    }
    private void Update()
    {
        /*
        movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
        // déplace le personnage
        transform.position = transform.position + movement * Time.deltaTime * moveSpeed;
        */
        /*
        movement = new Vector2(Input.GetAxis("Horizontal"),0.0f);
        rgbd.AddForce(movement);
        */
        horizontal = Input.GetAxis("Horizontal");
        if ( (isJumping == false) && (Input.GetButtonDown("Jump")))
        {
            jump = true;
        }
        /*
        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            rgbd.velocity = new Vector2(1.0f*moveSpeed, rgbd.velocity.y);
        }
        else if (Input.GetAxis("Horizontal") < -0.1f)
        {
            rgbd.velocity = new Vector2(-1.0f * moveSpeed, rgbd.velocity.y);
        }
        */
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
}
