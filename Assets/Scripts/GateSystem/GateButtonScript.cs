using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GateButtonScript : MonoBehaviour
{
    [SerializeField] UnityEvent OnClick;

    private void OnCollisionStay(Collision coll)
    {
        if (coll.gameObject.CompareTag("Player") && Input.GetButtonDown("Interact"))
        {
            OnClick.Invoke();
        }
    }
}