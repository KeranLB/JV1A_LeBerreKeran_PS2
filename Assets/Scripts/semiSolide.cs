using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class semiSolide : MonoBehaviour
{
    #region condition
    [SerializeField] GameObject parent;
    [HideInInspector] Collider2D collider;
    #endregion 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collider.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collider.enabled = true;
        }
    }
}
