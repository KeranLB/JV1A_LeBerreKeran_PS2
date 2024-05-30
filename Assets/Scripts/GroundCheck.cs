using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [HideInInspector] move move;

    private void Start()
    {
        move = Player.GetComponent<move>();
    }
    private void Update()
    {
        transform.localPosition = new Vector2(0f, 0f);
    }
    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("entre en collsion");
        if (collision.CompareTag("LeftWall"))
        {
            move.isLeftWalled = true;
        }

        else if (collision.CompareTag("RightWall"))
        {
            move.isRightWalled = true;
        }

        else
        {
            Debug.Log("est greounded");
            move.isGrounded = true;
        }
    }
    */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("entre en collsion");
        if (collision.CompareTag("LeftWall"))
        {
            move.isLeftWalled = true;
        }

        else if (collision.CompareTag("RightWall"))
        {
            move.isRightWalled = true;
        }

        else // if (collision.CompareTag("Gorund") && collision.CompareTag("Object") && collision.CompareTag("BreakWall"))
        {
            Debug.Log("est greounded");
            move.isGrounded = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("LeftWall"))
        {
            move.isLeftWalled = false;
        }
        else if (collision.CompareTag("RightWall"))
        {
            move.isRightWalled = false;
        }
        else
        {
            move.isGrounded = false;
        }
    }
    
}
