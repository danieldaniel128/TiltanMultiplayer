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
    //[SerializeField] private TMP_InputField scoreInputField;

    private List<RoomToJoin> roomButtonsList;

    [SerializeField] private RoomToJoin roomItemPrefab;

    [Header("Lobby Buttons and others")]
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button joinRoomButton;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button leaveRoomButton;
    [SerializeField] private Transform contentObject;
    [SerializeField] private TMP_InputField roomNameInputField;

    [Header("Lobby Texts")]
    [SerializeField] private TextMeshProUGUI currentRoomPlayersCountTextUI;
    [SerializeField] private TextMeshProUGUI serverDebugTextUI;
    [SerializeField] private TextMeshProUGUI playerListText;
    private void Start()
    {
        leaveRoomButton.interactable = false;
        roomButtonsList = new List<RoomToJoin>();
        createRoomButton.interactable = true;
        currentRoomPlayersCountTextUI.text = string.Format(Constants.CURRENT_ROOM_PLAYERS_PATTERN,
        0, 0);
        startGameButton.interactable = false;
        joinRoomButton.interactable = false;
        PhotonNetwork.AutomaticallySyncScene = true;

    }

    private void Update()
    {
        serverDebugTextUI.text = PhotonNetwork.NetworkClientState.ToString();

        if (!PhotonNetwork.IsMasterClient)
        {
            joinRoomButton.interactable = true;
        }
    }

    public void LoginToPhoton()
    {
        Debug.Log("Player nickname is " + PhotonNetwork.NickName);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("<color=#00ff00>We are connected!</color>");
        PhotonNetwork.JoinLobby();
        SetUsersUniqueID();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        HashSet<string> existingRoom = new HashSet<string>();

        foreach (RoomToJoin roomButton in roomButtonsList)
        {
            existingRoom.Add(roomButton.name);
        }

        foreach (RoomInfo room in roomList)
        {
            if (!room.RemovedFromList && room.PlayerCount > 0)
            {

                roomNameInputField.text = room.Name;
                RoomToJoin roomToJoin = Instantiate(roomItemPrefab, contentObject);
                roomToJoin.SetRoomName(room.Name);
                roomButtonsList.Add(roomToJoin);

                Button buttonToPress = roomToJoin.GetComponent<Button>();

                buttonToPress.onClick.AddListener(JoinRoom);
            }

            else
            {
                RoomToJoin buttonToRemove = roomButtonsList.Find(button => button.GetRoomName() == room.Name);
                if (buttonToRemove != null)
                {
                    roomButtonsList.Remove(buttonToRemove);
                    Destroy(buttonToRemove.gameObject);
                }
            }
        }
    }


    public void CreateRoom()
    {
        createRoomButton.interactable = false;
        PhotonNetwork.AutomaticallySyncScene = false;
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
                CleanupCacheOnLeave = false,

            };
        PhotonNetwork.CreateRoom(roomNameInputField.text.ToString(),
            roomOptions,
            null);
    }

    public void JoinRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.JoinRoom(roomNameInputField.text.ToString(), null);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.AutomaticallySyncScene = true;
        RefreshCurrentRoomInfoUI();
        createRoomButton.interactable = true;
        leaveRoomButton.interactable = false;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        PhotonNetwork.AutomaticallySyncScene = false;
        Debug.Log("We are in a room!");

    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("Joined Room!");
        RefreshCurrentRoomInfoUI();
        leaveRoomButton.interactable = true;
        createRoomButton.interactable = false;
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        PhotonNetwork.AutomaticallySyncScene = true;

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


    private void RefreshCurrentRoomInfoUI()
    {
        playerListText.text = string.Empty;
        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            currentRoomPlayersCountTextUI.text = string.Format(Constants.CURRENT_ROOM_PLAYERS_PATTERN,
            PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers);
            foreach (Player photonPlayer in PhotonNetwork.PlayerList)
            {
                playerListText.text += photonPlayer.NickName + Environment.NewLine;
            }
        }
        else
        {
            currentRoomPlayersCountTextUI.text = string.Empty;
        }

    }

    private void SetUsersUniqueID()
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add(Constants.USER_UNIQUE_ID, SystemInfo.deviceUniqueIdentifier);
        PhotonNetwork.SetPlayerCustomProperties(hashtable);
    }
}
