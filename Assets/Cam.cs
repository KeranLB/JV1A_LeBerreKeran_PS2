using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [SerializeField]
    public Transform Subject;

    Vector2 startPosition;

    float startZ;
    float startY;

    Vector2 travel => (Vector2)Subject.transform.position - startPosition;

    void Start()
    {
        startPosition = transform.position;
        startZ = transform.localPosition.z;
        startY = transform.localPosition.y;
    }

    void Update()
    {
        Vector2 newPos = startPosition + travel;
        transform.position = new Vector3(newPos.x, startY, startZ);
    }
}
