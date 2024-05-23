using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEmptyBody : MonoBehaviour
{
    [SerializeField] Transform Body;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = Body.transform.position;    
    }
}
