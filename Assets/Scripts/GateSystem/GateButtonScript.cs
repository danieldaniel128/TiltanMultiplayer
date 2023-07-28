using System;
using System.Collections;
using System.Collections.Generic;using System.Security.Claims;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GateButtonScript : MonoBehaviour
{
    public UnityEvent<float> OnAlienClick;
    

    [SerializeField] private UnityEvent OnEscaperClick;

    private void OnCollisionStay(Collision coll)
    {
        if (coll.gameObject.CompareTag("Player") && Input.GetButtonDown("Interact"))
        {
            OnEscaperClick.Invoke();
        }
    }
    

}


