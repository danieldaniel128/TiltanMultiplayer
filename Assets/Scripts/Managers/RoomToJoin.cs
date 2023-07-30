using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomToJoin : MonoBehaviourPun
{
    private string Name;

    [SerializeField] private TextMeshProUGUI ButtonText;

    public void SetRoomName(string roomName)
    {
        Name = roomName;
        ButtonText.text = roomName;
    }

    public string GetRoomName()
    {
        return Name;
    }


}
