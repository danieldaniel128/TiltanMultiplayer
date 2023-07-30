using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomToJoin : MonoBehaviourPun
{
    [SerializeField] private TextMeshProUGUI ButtonText;

    private string roomName;

    public void SetRoomName(string roomName)
    {
        this.roomName = roomName;
        ButtonText.text = roomName;
    }

    public string GetRoomName()
    {
        return roomName;
    }


}
