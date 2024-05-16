using UnityEngine;

public class Grappin : MonoBehaviour
{

    PlayerController control;
    public Rigidbody2D rgbd;
    Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        GameObject tmp = GameObject.FindGameObjectWithTag("Player");
        control = tmp.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Grappable"))
        {
            control.TargetHit(collision.gameObject);
        }
        position = transform.position;
    }

    private void Update()
    {
        if ( (position.x != 0f) && (position.y != 0f) && (position.z != 0f))
        {
            transform.position = position;
            rgbd.velocity = new Vector2(0f, 0f);
        }
    }
}
