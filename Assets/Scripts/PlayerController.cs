using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public bool canControl = false;
    [SerializeField] private int speed;

    private void Start()
    {
        if(PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(Constants.MATCH_STARTED))
            canControl = (bool)PhotonNetwork.CurrentRoom.CustomProperties[Constants.MATCH_STARTED];
    }

    // Update is called once per frame
    void Update()
    {
        if (canControl && photonView.IsMine)
        {
            if (Input.GetKey(KeyCode.W))
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
            if (Input.GetKey(KeyCode.S))
                transform.Translate(Vector3.back * Time.deltaTime * speed);
            if (Input.GetKey(KeyCode.A))
                transform.Translate(Vector3.left * Time.deltaTime * speed);
            if (Input.GetKey(KeyCode.D))
                transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if(info.photonView.IsMine)
            OnlineGameManager.Instance.SetPlayerController(this);
        
        Debug.Log("Instad!");
    }
}
