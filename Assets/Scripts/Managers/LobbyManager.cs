using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    //[SerializeField] private TextMeshProUGUI serverDebugTextUI;
    //[SerializeField] private TextMeshProUGUI isConnectedToRoomDebugTextUI;
    //[SerializeField] private TextMeshProUGUI currentRoomNameDebugTextUI;
    //[SerializeField] private TextMeshProUGUI currentRoomPlayersCountTextUI;
    //[SerializeField] private TextMeshProUGUI roomsListText;
    //[SerializeField] private Button leaveRoomButton;
    //[SerializeField] private TMP_InputField scoreInputField;

    private List<RoomToJoin> roomButtonsList;

    [SerializeField] private RoomToJoin roomItemPrefab;

    [Header("Lobby Buttons and others")]
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button joinRoomButton;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Transform contentObject;
    [SerializeField] private TMP_InputField roomNameInputField;

    [Header("Lobby Texts")]
    [SerializeField] private TextMeshProUGUI currentRoomPlayersCountTextUI;
    [SerializeField] private TextMeshProUGUI serverDebugTextUI;
    [SerializeField] private TextMeshProUGUI playerListText;

    public void LoginToPhoton()
    {
        Debug.Log("Player nickname is " + PhotonNetwork.NickName);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("<color=#00ff00>We are connected!</color>");
        createRoomButton.interactable = true;
        PhotonNetwork.JoinLobby();
        SetUsersUniqueID();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        foreach (RoomToJoin roomButton in roomButtonsList)
        {
            Destroy(roomButton.gameObject);
        }

        roomButtonsList.Clear();

        foreach (RoomInfo room in roomList)
        {
            roomNameInputField.text = room.Name;
            RoomToJoin roomToJoin = Instantiate(roomItemPrefab, contentObject);
            roomToJoin.SetRoomName(room.Name);
            roomButtonsList.Add(roomToJoin);
            joinRoomButton.interactable = true;
        }
    }







    public void CreateRoom()
    {
        createRoomButton.interactable = false;
        Hashtable hashtable = new Hashtable();
        hashtable.Add(Constants.MIN_LEVEL, 6);
        hashtable.Add(Constants.MAX_LEVEL, 666);
        hashtable.Add(Constants.GAME_MODE, "FreeForAll");
        RoomOptions roomOptions =
            new RoomOptions
            {
                MaxPlayers = 4,
                EmptyRoomTtl = 0,
                PlayerTtl = 35000,
                CustomRoomProperties = hashtable,
                CleanupCacheOnLeave = false
            };
        PhotonNetwork.CreateRoom(roomNameInputField.text.ToString(),
            roomOptions,
            null);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomNameInputField.text.ToString(), null);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        //leaveRoomButton.interactable = false;
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
        //isConnectedToRoomDebugTextUI.text = Constants.YES_STRING;
        RefreshCurrentRoomInfoUI();
        //leaveRoomButton.interactable = true;
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


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.LogError("Create failed..." + Environment.NewLine + message);
        createRoomButton.interactable = true;
    }

    public void SetUserScore(string scoreString)
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogError(
                "We tried to set the user score while not connected!");
            return;
        }
        int score = int.Parse(scoreString);
        Hashtable hashtable
            = new Hashtable();
        hashtable.Add(Constants.PLAYER_STRENGTH_SCORE_PROPERTY_KEY, score);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
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
        //isConnectedToRoomDebugTextUI.text = Constants.NO_STRING;
        roomButtonsList = new List<RoomToJoin>();
        createRoomButton.interactable = false;
        currentRoomPlayersCountTextUI.text = string.Format(Constants.CURRENT_ROOM_PLAYERS_PATTERN,
        0, 0);
        //leaveRoomButton.interactable = false;
        startGameButton.interactable = false;
        joinRoomButton.interactable = false;
        PhotonNetwork.AutomaticallySyncScene = true;
        LoginToPhoton();
    }

    private void Update()
    {
        serverDebugTextUI.text = PhotonNetwork.NetworkClientState.ToString();
    }

    private void RefreshCurrentRoomInfoUI()
    {
        //playerListText.text = string.Empty;
        if (PhotonNetwork.CurrentRoom != null)
        {
            //currentRoomNameDebugTextUI.text = PhotonNetwork.CurrentRoom.Name;
            currentRoomPlayersCountTextUI.text = string.Format(Constants.CURRENT_ROOM_PLAYERS_PATTERN,
            PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers);
            foreach (Player photonPlayer in PhotonNetwork.PlayerList)
            {
                //playerListText.text += photonPlayer.NickName + Environment.NewLine;
            }
        }
        else
        {
            //currentRoomNameDebugTextUI.text = string.Empty;
            //currentRoomPlayersCountTextUI.text = string.Empty;
        }


    }

    private void SetUsersUniqueID()
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add(Constants.USER_UNIQUE_ID, SystemInfo.deviceUniqueIdentifier);
        PhotonNetwork.SetPlayerCustomProperties(hashtable);
    }
}

