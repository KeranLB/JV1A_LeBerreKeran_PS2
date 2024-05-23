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
    public bool isGrounded;
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
        rgbd.velocity = new Vector2(horizontal * moveSpeed, rgbd.velocity.y);

        if (jump)
        {
            rgbd.AddForce(Vector2.up * jumpForce);
            jump = false;
        }
    }

    private void Inputs()
    {
        horizontal = player.GetAxis("Horizontal");
        if ((isGrounded == true) && (player.GetButtonDown("Jump")))
        {
            jump = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
