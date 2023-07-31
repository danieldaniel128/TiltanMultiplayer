using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallController : MonoBehaviour
{
    [SerializeField] private GameObject particleToInstantiate;
    [SerializeField] private Rigidbody ballRigidbody;
    [SerializeField] private int forceToAdd;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ballRigidbody.AddForce(transform.forward * forceToAdd,
            ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.transform.CompareTag(Constants.DESTROYABLE_TAG))
        {
            Instantiate(particleToInstantiate,
                collision.contacts[0].point, Quaternion.identity);
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(collision.transform.gameObject);
            }
        }
    }
}
