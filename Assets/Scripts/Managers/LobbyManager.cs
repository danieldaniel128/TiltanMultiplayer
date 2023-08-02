using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
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
    [Header("Manage Rooms Btns")]
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button leaveRoomButton;
    [Header("Manage CharacterSelection Btns")]
    [SerializeField] private Button selectAlienButton;
    [SerializeField] private Button selectEscaperButton;
    [SerializeField] private TMP_InputField roomNameInputField;

    [SerializeField] private Transform contentObject;

    [Header("Lobby Texts")]
    [SerializeField] private TextMeshProUGUI currentRoomPlayersCountTextUI;
    [SerializeField] private TextMeshProUGUI serverDebugTextUI;
    [SerializeField] private TextMeshProUGUI playerListText;

    [Header("Character Selection Texts")]
    [SerializeField] private TextMeshProUGUI playerAlienListText;
    [SerializeField] private TextMeshProUGUI playerEscaperListText;

    private void Start()
    {
        selectAlienButton.interactable = false;
        selectEscaperButton.interactable = false;
        leaveRoomButton.interactable = false;
        roomButtonsList = new List<RoomToJoin>();
        createRoomButton.interactable = true;
        currentRoomPlayersCountTextUI.text = string.Format(Constants.CURRENT_ROOM_PLAYERS_PATTERN,
        0, 0);
        startGameButton.interactable = false;
        PhotonNetwork.AutomaticallySyncScene = true;
        LoginToPhoton();

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

        List<RoomToJoin> roomsButtons = new List<RoomToJoin>();

        foreach (RoomToJoin roomButton in roomButtonsList)
        {
            roomsButtons.Add(roomButton);
        }

        // Loop through the list of rooms in the roomList
        foreach (RoomInfo room in roomList)
        {
            // Check if the room has not been removed from the list
            if (!room.RemovedFromList)
            {
                Debug.Log("not removed sadasd");
                // Check if the room has players in it and is not already in the existingRoom list
                Debug.Log(roomsButtons.Count);
                Debug.Log(roomsButtons.Where(c => c.GetRoomName().Equals(room.Name)).ToList().Count);
                if (room.PlayerCount > 0 && roomsButtons.Where(c => c.GetRoomName().Equals(room.Name)).ToList().Count==0)
                {
                    Debug.Log("room and people full");
                    // Set the room name input field to the name of the room
                    roomNameInputField.text = room.Name;

                    // Instantiate a new RoomToJoin prefab and set its room name
                    RoomToJoin roomToJoin = Instantiate(roomItemPrefab, contentObject);
                    roomToJoin.SetRoomName(room.Name);
                    roomsButtons.Add(roomToJoin);

                    // Get the button component from the RoomToJoin prefab and add a click listener to it
                    Button buttonToPress = roomToJoin.GetComponent<Button>();
                    buttonToPress.onClick.AddListener(JoinRoom);
                }
                

                // If the room has no players and is in the existingRoom list

                // Output the room name and player count in the console for debugging
                Debug.Log("Room: " + room.Name + ", PlayerCount: " + room.PlayerCount);
            }
            else if (room.PlayerCount == 0 && roomsButtons.Where(c => c.GetRoomName().Equals(room.Name)).ToList().Count > 0)
            {
                // Find the RoomToJoin prefab in the roomButtonsList that matches the room name
                RoomToJoin buttonToRemove = roomsButtons.FirstOrDefault(button => button.GetRoomName() == room.Name);
                if (buttonToRemove != null)
                {
                    Debug.Log("room and people empty");
                    Debug.Log("removed btn");
                    // Remove the button from the roomButtonsList and destroy the GameObject
                    roomsButtons.Remove(buttonToRemove);
                    Destroy(buttonToRemove.gameObject);
                }
            }
            roomButtonsList = new List<RoomToJoin>(roomsButtons);
        }
    }


    public void CreateRoom()
    {
        createRoomButton.interactable = false;
        Hashtable hashtable = new Hashtable();
        hashtable.Add(Constants.MIN_LEVEL, 6);
        hashtable.Add(Constants.MAX_LEVEL, 666);
        hashtable.Add(Constants.GAME_MODE, "EscaperRoom");
        hashtable.Add(Constants.Alien_List, "");
        hashtable.Add(Constants.Escapers_List,"");
        hashtable.Add(Constants.Can_Join_Alien_List, true);
        hashtable.Add(Constants.Can_Join_Escapers_List, true);
        RoomOptions roomOptions =
            new RoomOptions
            {
                MaxPlayers = 4,
                EmptyRoomTtl = 0,
                PlayerTtl = 0,
                CustomRoomProperties = hashtable,
                CleanupCacheOnLeave = false,

            };
        PhotonNetwork.CreateRoom(roomNameInputField.text.ToString(),
            roomOptions,
            null);

    }
    public void JoinAlien()
    {
        string AliensPlayers = (string)PhotonNetwork.CurrentRoom.CustomProperties[Constants.Alien_List];
        AliensPlayers += "," + (SignUpManager.Instance.PlayerNickname);
        PhotonNetwork.CurrentRoom.CustomProperties[Constants.Alien_List] = AliensPlayers;
        List<string> aliensPlayers = AliensPlayers.Split(',').ToList();
        aliensPlayers.Remove("");
        playerAlienListText.text = "";
        foreach (string alienPlayer in aliensPlayers)
        {
            playerAlienListText.text += alienPlayer + "\n";
        }
        if (aliensPlayers.Count == 1)//only one in our game
        {
            selectAlienButton.interactable = false;
        }
    }
    public void JoinEscapers()
    {
        string EscapersPlayers = (string)PhotonNetwork.CurrentRoom.CustomProperties[Constants.Escapers_List];
        EscapersPlayers += "," +(SignUpManager.Instance.PlayerNickname);
        PhotonNetwork.CurrentRoom.CustomProperties[Constants.Escapers_List] = EscapersPlayers;
        List<string> escapersPlayers = EscapersPlayers.Split(',').ToList();
        escapersPlayers.Remove("");
        playerEscaperListText.text = "";
        foreach (string escaperPlayer in escapersPlayers)
        {
            playerEscaperListText.text += escaperPlayer + "\n";
        }
        if (escapersPlayers.Count == 3)//only 3 in our game
        {
            selectEscaperButton.interactable = false;
        }
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
    private void RemovePlayerFromATeam()
    {
        //daniel wrote it, promise its not ai.
        //get the custom property of the room. the player escaper list as a long string.
        string EscapersPlayers = (string)PhotonNetwork.CurrentRoom.CustomProperties[Constants.Escapers_List];
        //make a list of strings to use instead of a long string.
        List<string> escapersPlayers = EscapersPlayers.Split(',').ToList();
        //removing the empty string in case there is one in the list.
        escapersPlayers.Remove("");
        //remove the player that is loged in that left the room.
        escapersPlayers.Remove(SignUpManager.Instance.PlayerNickname);
        //if the list after the player got removed is empty, make the property empty too.
        if (escapersPlayers.Count == 0)
            PhotonNetwork.CurrentRoom.CustomProperties[Constants.Escapers_List] = "";
        else
        {
            //if the list after the player got removed is not empty, make a string of all current escaper players in the room. each player string has ',' that seperates it from the others.
            EscapersPlayers = escapersPlayers[0];
            for (int i = 1; i < escapersPlayers.Count; i++)
            {
                EscapersPlayers += "," + escapersPlayers[i];
            }
            //set the custom property of the room of the escapers to the new string of current escaper players
            PhotonNetwork.CurrentRoom.CustomProperties[Constants.Escapers_List] = EscapersPlayers;
        }

        //same logic of escapers for the aliens. even tho there is only oone alien in a game, we did the same logic for future extention since its the same.
        string AliensPlayers = (string)PhotonNetwork.CurrentRoom.CustomProperties[Constants.Alien_List];
        List<string> aliensPPlayers = AliensPlayers.Split(',').ToList();
        aliensPPlayers.Remove("");
        aliensPPlayers.Remove(SignUpManager.Instance.PlayerNickname);
        if (aliensPPlayers.Count == 0)
            PhotonNetwork.CurrentRoom.CustomProperties[Constants.Alien_List] = "";
        else
        {
            AliensPlayers = aliensPPlayers[0];
            for (int i = 1; i < aliensPPlayers.Count; i++)
            {
                AliensPlayers += "," + aliensPPlayers[i];
            }
            PhotonNetwork.CurrentRoom.CustomProperties[Constants.Alien_List] = aliensPPlayers;
        }
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        playerEscaperListText.text = "";
        playerAlienListText.text = "";
        selectAlienButton.interactable = false;
        selectEscaperButton.interactable = false;
        RemovePlayerFromATeam();
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
        selectAlienButton.interactable = true;
        selectEscaperButton.interactable = true;
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

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    RoomToJoin buttonToRemove = roomButtonsList.Find(button => button.GetRoomName() == PhotonNetwork.CurrentRoom.Name);
        //    if (buttonToRemove != null)
        //    {
        //        roomButtonsList.Remove(buttonToRemove);
        //        Destroy(buttonToRemove.gameObject);
        //    }
        //}
        if (PhotonNetwork.CurrentRoom.PlayerCount == 0 && roomButtonsList.Where(c => c.GetRoomName().Equals(PhotonNetwork.CurrentRoom.Name)).ToList().Count > 0)
        {
            // Find the RoomToJoin prefab in the roomButtonsList that matches the room name
            RoomToJoin buttonToRemove = roomButtonsList.FirstOrDefault(button => button.GetRoomName() == PhotonNetwork.CurrentRoom.Name);
            if (buttonToRemove != null)
            {
                Debug.Log("room and people empty");
                Debug.Log("removed btn");
                // Remove the button from the roomButtonsList and destroy the GameObject
                roomButtonsList.Remove(buttonToRemove);
                Destroy(buttonToRemove.gameObject);
            }
            else
            {
                Debug.Log($"buttonToRemove is null: {buttonToRemove}");
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
            PhotonNetwork.CurrentRoom.PlayerTtl = 60000;
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
