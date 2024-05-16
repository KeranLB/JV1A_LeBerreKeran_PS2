using UnityEngine;
using Rewired;

public class move : MonoBehaviour
{
    public Rigidbody2D rgbd;
    #region rewired
    private Player player;
    private int playerId = 0;
    #endregion
    /*
    #region Move
    public int moveSpeed;
    private Vector3 movement;
    #endregion
    */
    #region Move
    private Vector2 movement;
    public int moveSpeed;
    #endregion
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
        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            rgbd.velocity = new Vector2(1.0f*moveSpeed, rgbd.velocity.y);
        }
        else if (Input.GetAxis("Horizontal") < -0.1f)
        {
            rgbd.velocity = new Vector2(-1.0f * moveSpeed, rgbd.velocity.y);
        }
        else if (Input.GetAxis("Horizontal") == 0.0f)
        {
            rgbd.velocity = new Vector2(.0f, rgbd.velocity.y);
        }
    }
}
