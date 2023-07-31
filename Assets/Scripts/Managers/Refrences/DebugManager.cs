using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    private string lastRoomName;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            lastRoomName = PhotonNetwork.CurrentRoom.Name;
            PhotonNetwork.Disconnect();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PhotonNetwork.ReconnectAndRejoin();
        }
    }
}
