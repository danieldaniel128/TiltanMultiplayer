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
        PhotonNetwork.AutomaticallySyncScene = true;

    }

    private void Update()
    {
        serverDebugTextUI.text = PhotonNetwork.NetworkClientState.ToString();
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
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        HashSet<string> existingRoom = new HashSet<string>();

        foreach (RoomToJoin roomButton in roomButtonsList)
        {
            existingRoom.Add(roomButton.name);
        }

        // Loop through the list of rooms in the roomList
        foreach (RoomInfo room in roomList)
        {
            // Check if the room has not been removed from the list
            if (!room.RemovedFromList)
            {
                // Check if the room has players in it and is not already in the existingRoom list
                if (room.PlayerCount > 0 && !existingRoom.Contains(room.Name))
                {
                    // Set the room name input field to the name of the room
                    roomNameInputField.text = room.Name;

                    // Instantiate a new RoomToJoin prefab and set its room name
                    RoomToJoin roomToJoin = Instantiate(roomItemPrefab, contentObject);
                    roomToJoin.SetRoomName(room.Name);
                    roomButtonsList.Add(roomToJoin);

                    // Get the button component from the RoomToJoin prefab and add a click listener to it
                    Button buttonToPress = roomToJoin.GetComponent<Button>();
                    buttonToPress.onClick.AddListener(JoinRoom);
                }
                // If the room has no players and is in the existingRoom list

                // Output the room name and player count in the console for debugging
                Debug.Log("Room: " + room.Name + ", PlayerCount: " + room.PlayerCount);
            }
            else if (room.PlayerCount == 0 && existingRoom.Contains(room.Name))
            {
                // Find the RoomToJoin prefab in the roomButtonsList that matches the room name
                RoomToJoin buttonToRemove = roomButtonsList.Find(button => button.GetRoomName() == room.Name);
                if (buttonToRemove != null)
                {
                    Debug.Log("removed btn");
                    // Remove the button from the roomButtonsList and destroy the GameObject
                    roomButtonsList.Remove(buttonToRemove);
                    Destroy(buttonToRemove.gameObject);
                }
            }
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
                CleanupCacheOnLeave = false,

            };
        PhotonNetwork.CreateRoom(roomNameInputField.text.ToString(),
            roomOptions,
            null);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomNameInputField.text.ToString(), null);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
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
        Debug.Log("We are in a room!");

    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined Room!");
        RefreshCurrentRoomInfoUI();
        leaveRoomButton.interactable = true;
        createRoomButton.interactable = false;
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


        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            startGameButton.interactable = false;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            RoomToJoin buttonToRemove = roomButtonsList.Find(button => button.GetRoomName() == PhotonNetwork.CurrentRoom.Name);
            if (buttonToRemove != null)
            {
                roomButtonsList.Remove(buttonToRemove);
                Destroy(buttonToRemove.gameObject);
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
