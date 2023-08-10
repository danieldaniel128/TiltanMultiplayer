using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ROnlineTest : MonoBehaviourPunCallbacks
{
    SpawnPoint[] spawnPoints;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        CreateRoom();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CreateRoom()
    {
        PhotonNetwork.JoinOrCreateRoom( "test", new Photon.Realtime.RoomOptions(), TypedLobby.Default);
    }
}