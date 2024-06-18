using UnityEngine;

public class Arme : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BreakWall"))
        {
            Rigidbody2D rgbd = collision.gameObject.GetComponent<Rigidbody2D>();
            rgbd.bodyType = RigidbodyType2D.Dynamic;
            collision.tag = "Object";
        }
        if (collision.CompareTag("Ennemi"))
        {
            Destroy(collision.gameObject);
        }
    }
}
