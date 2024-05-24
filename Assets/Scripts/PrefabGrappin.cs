using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabGrappin : MonoBehaviour
{
    #region condition
    [HideInInspector] bool isGrabing = false;
    [HideInInspector] Rigidbody2D rgbd;
    #endregion

    private void Start()
    {
        rgbd = gameObject.GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("grappable"))
        {
            isGrabing = true;
        }
    }
}
