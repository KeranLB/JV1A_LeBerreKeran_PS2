using UnityEngine;

public class levier : MonoBehaviour
{

    [SerializeField] GameObject door;
    [SerializeField] GameObject BaseGrappin;
    [HideInInspector] Grappin Grappin;
    [HideInInspector] bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        Grappin = BaseGrappin.GetComponent<Grappin>();
        isOpen = false;
    }

    private void Update()
    {
        if (isOpen)
        {
            door.SetActive(true);
        }
        else
        {
            door.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Grab"))
        {
            if (!isOpen)
            {
                isOpen = true;
            }
            else
            {
                isOpen = false;
            }
            Grappin.isLaunched = false;
            Grappin.StartCancel();
        }
    }
}
