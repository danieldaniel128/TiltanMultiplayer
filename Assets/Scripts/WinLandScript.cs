using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLandScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has won");
        }
    }
}
