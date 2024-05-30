using UnityEngine;

public class parallaxe : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Transform subject;

    [HideInInspector] Vector2 startPosition;

    [HideInInspector] float startZ;
    [HideInInspector] float startY;

    Vector2 travel => (Vector2)cam.transform.position - startPosition;

    float distanceFromSubject => transform.position.z - subject.position.z;

    float clippingPlane => (cam.transform.position.z + (distanceFromSubject > 0 ? cam.farClipPlane : cam.nearClipPlane));

    float parallaxeFactor => Mathf.Abs(distanceFromSubject) / clippingPlane;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startZ = transform.position.z;
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = startPosition + travel * parallaxeFactor;
        transform.position = new Vector3(newPos.x, startY, startZ);
    }
}
