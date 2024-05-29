using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabed : MonoBehaviour
{
    [HideInInspector] Transform grabedPosition;
    [SerializeField] GameObject grab;
    [HideInInspector] PrefabGrappin grabScript;

    // Start is called before the first frame update
    void Start()
    {
        grabScript = grab.GetComponent<PrefabGrappin>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grabScript.isGrabing)
        {
            transform.position = grabedPosition.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Grab"))
        {
            grabedPosition.position = transform.position;
        }
    }
}
