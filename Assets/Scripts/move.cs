using UnityEngine;
using Rewired;

public class move : MonoBehaviour
{
    #region rewired
    private Player player;
    private int playerId = 0;
    #endregion

    public Rigidbody2D rgbd;
    public float horizontal;
    public bool jump;
    public bool isGrounded = false;
    public bool isLeftWalled = false;
    public bool isRightWalled = false;
    public int jumpForce;
    public int moveSpeed;

    private void Start()
    {
        player = ReInput.players.GetPlayer(playerId);
        jump = false;
    }

    private void Update()
    {
        Inputs();
    }

    private void FixedUpdate()
    {
        if ((horizontal > 0.0f) && (rgbd.velocity.x <= 10))
        {
            rgbd.AddForce(Vector2.right.normalized * moveSpeed);
        }
        else if ( (horizontal < 0.0f) && (rgbd.velocity.x >= -10) )
        {
            rgbd.AddForce(Vector2.left * moveSpeed);
        }

        if ((jump) && (isGrounded))
        {
            rgbd.AddForce(Vector2.up * jumpForce);
            jump = false;
            isGrounded = false;
        }
        else if ((jump) && (isLeftWalled) && (!isGrounded))
        {
            rgbd.AddForce(Vector2.up * jumpForce );
            rgbd.AddForce(Vector2.right * jumpForce );
            jump = false;
            isLeftWalled = false;
        }
        else if ((jump) && (isRightWalled))
        {
            rgbd.AddForce(Vector2.up * jumpForce / 2);
            rgbd.AddForce(Vector2.left * jumpForce / 2);
            jump = false;
            isRightWalled = false;
        }
    }

    private void Inputs()
    {
        horizontal = player.GetAxis("Horizontal");

        if ( ( (isGrounded) || (isLeftWalled) || (isRightWalled) ) && (player.GetButtonDown("Jump")))
        {
            jump = true;
        }
    }
}
