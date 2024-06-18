using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] Transform leftCheckGrounded;
    [SerializeField] Transform rightCheckGrounded;

    [SerializeField] move move;

    [HideInInspector] public bool isGrounded;
    private void Update()
    {
        move.isGrounded = Physics2D.OverlapArea(leftCheckGrounded.position, rightCheckGrounded.position);
    }    
}
