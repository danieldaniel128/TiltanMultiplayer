using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomToJoin : MonoBehaviourPun
{
    //private string roomName;
    //public string RoomName { get => roomName; set => roomName = value; }

    [SerializeField] private TextMeshProUGUI ButtonText;

    public void SetRoomName(string roomName)
    {
        ButtonText.text = roomName;
    }
}
