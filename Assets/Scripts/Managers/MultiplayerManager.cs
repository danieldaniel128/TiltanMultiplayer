using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    private const string CURRENT_ROOM_PLAYERS_PATTERN = "{0}/{1}";
    private const string NO_STRING = "No!";
    private const string YES_STRING = "Yes!";

    [SerializeField] private TMP_InputField nicknameInputField;
    
    [Header("Room Controls")]
    [SerializeField] private Button createRoomButton;
    [SerializeField] private string roomNameToCreate;
    [SerializeField] private Button leaveRoomButton;
    [SerializeField] private Button startGameButton;
    
    [Header("Debug Texts")]
    [SerializeField] private TextMeshProUGUI serverDebugTextUI;
    [SerializeField] private TextMeshProUGUI isConnectedToRoomDebugTextUI;
    [SerializeField] private TextMeshProUGUI currentRoomNameDebugTextUI;
    [SerializeField] private TextMeshProUGUI currentRoomPlayersCountTextUI;
    [SerializeField] private TextMeshProUGUI playerListText;
    [SerializeField] private TextMeshProUGUI roomsListText;
    
    public void LoginToPhoton()
    {
        PhotonNetwork.NickName = nicknameInputField.text;
        Debug.Log("Player nickname is " + PhotonNetwork.NickName);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("<color=#00ff00>We are connected!</color>");
        createRoomButton.interactable = true;
        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Got room list");
        base.OnRoomListUpdate(roomList);
      //  roomsListText.text = string.Empty;
      foreach (RoomInfo roomInfo in roomList)
        {
            if(!roomInfo.RemovedFromList )
            roomsListText.text += roomInfo.Name + Environment.NewLine;
            else
            {
                Debug.Log("Room " + roomInfo.Name + " No longer exist");
            }
        }
    }

    public void CreateRoom()
    {
        createRoomButton.interactable = false;
        PhotonNetwork.JoinOrCreateRoom(roomNameToCreate, new RoomOptions(){MaxPlayers = 20, EmptyRoomTtl = 0},
            null );
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        leaveRoomButton.interactable = false;
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("We are in a room!");
        
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined Room!");
        isConnectedToRoomDebugTextUI.text = YES_STRING;
        RefreshCurrentRoomInfoUI();
        leaveRoomButton.interactable = true;
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        RefreshCurrentRoomInfoUI();
        isConnectedToRoomDebugTextUI.text = NO_STRING;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        RefreshCurrentRoomInfoUI();

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
            {
                startGameButton.interactable = true;
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        RefreshCurrentRoomInfoUI();
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
            {
                startGameButton.interactable = false;
            }
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.LogError("Create failed..." + Environment.NewLine + message);
        createRoomButton.interactable = true;
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
         
            PhotonNetwork.LoadLevel(1);
        }
    }

    private void Start()
    {
        isConnectedToRoomDebugTextUI.text = NO_STRING;
        currentRoomNameDebugTextUI.text = string.Empty;
        createRoomButton.interactable = false;
        currentRoomPlayersCountTextUI.text = string.Format(CURRENT_ROOM_PLAYERS_PATTERN,
            0,0);
        leaveRoomButton.interactable = false;
        startGameButton.interactable = false;
        PhotonNetwork.AutomaticallySyncScene = true;
        LoginToPhoton();
    }
    

    private void Update()
    {
        serverDebugTextUI.text = PhotonNetwork.NetworkClientState.ToString();
    }

    private void RefreshCurrentRoomInfoUI()
    {
        playerListText.text = string.Empty;
        if (PhotonNetwork.CurrentRoom != null)
        {
            currentRoomNameDebugTextUI.text = PhotonNetwork.CurrentRoom.Name;
            currentRoomPlayersCountTextUI.text = string.Format(CURRENT_ROOM_PLAYERS_PATTERN,
                PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers);
            foreach (Player photonPlayer in PhotonNetwork.PlayerList)
            {
                playerListText.text += photonPlayer.NickName + Environment.NewLine;
            }
        }
        else
        {
            currentRoomNameDebugTextUI.text = string.Empty;
            currentRoomPlayersCountTextUI.text = string.Empty;
        }


    }
    
}
