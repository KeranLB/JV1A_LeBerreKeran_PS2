/*
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
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float offsetx, offsety, camspeed;
    private float travelling = 10;

    void Update()
    {
        //suit le joueur
        transform.position = new Vector3(player.position.x + travelling, player.position.y + offsety, transform.position.z);
        //Fait bouger la caméra dans la direction que le joueur regarde
        travelling = Mathf.Lerp(travelling, (offsetx * player.localScale.x), Time.deltaTime * camspeed);
    }
}