using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using System.Linq;
using System.Collections.Generic;

public enum CharacterEnum
{
    Alien,
    Escaper
}
public class EndingScreenDataManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI AlienNumber_TMP;
    [SerializeField] TextMeshProUGUI NumberOfEscapers_TMP;
    [SerializeField] TextMeshProUGUI Team_TMP;
    [SerializeField] TextMeshProUGUI Objective_TMP;
    [SerializeField] TextMeshProUGUI Succeded_TMP;
    [SerializeField] TextMeshProUGUI Seconds_TMP;
    [SerializeField] TextMeshProUGUI Minutes_TMP;

    [SerializeField] TextMeshProUGUI _backToLobbyTimer_TMP;

    [SerializeField] float maximumTimeInEndingScene;
    float timePassedInEndingScene = 0;
    bool isPassedMaximum = false;
    private void Start()
    {
        string gameData = ConvertToJson(GatherGameData());
        photonView.RPC(nameof(DoStuff), RpcTarget.AllViaServer, gameData);
    }
    private void Update()
    {
        GamePassedTimeTimerSeconds();
    }
    [PunRPC]
    private void DoStuff(string gameDataJsonAsString)
    {
        SetTexts(ReadFromJson(gameDataJsonAsString));
    }
    private GameRoomData GatherGameData()
    {
        GameRoomData loacalGameData = new GameRoomData();
        List<string> escapersList = PhotonNetwork.CurrentRoom.CustomProperties[Constants.Escapers_List].ToString().Split(',').ToList();
        escapersList.Remove("");
        loacalGameData.NumberOfEscapers = escapersList.Count;
        List<string> aliensList = PhotonNetwork.CurrentRoom.CustomProperties[Constants.Alien_List].ToString().Split(',').ToList();
        aliensList.Remove("");
        loacalGameData.NumberOfAliens = aliensList.Count;
        loacalGameData.TimePassedInSeconds = (float)PhotonNetwork.CurrentRoom.CustomProperties[Constants.Game_Timer];
        return loacalGameData;
    }

    public void BackToLobby()
    {
        PhotonNetwork.LoadLevel(0);//local on porpuse.
    }

    private void GamePassedTimeTimerSeconds()//when master switched, to notify the timer data
    {
        if (!isPassedMaximum)
        timePassedInEndingScene += Time.deltaTime;
        _backToLobbyTimer_TMP.text = Mathf.Ceil(timePassedInEndingScene).ToString();
            if (timePassedInEndingScene >= maximumTimeInEndingScene)
            { 
                isPassedMaximum = true;
                if (PhotonNetwork.IsMasterClient)
                {
                    BackToLobby();
                }       
            }
        //leave room?
    }
    public void CaculateTimePassedToFormatText(float timePassedInSeconds)
    {
        float seconds = (int)timePassedInSeconds % 60;
        float minutes = (int)timePassedInSeconds / 60;
        Seconds_TMP.text = seconds.ToString();
        Minutes_TMP.text = minutes.ToString();
    }
    public void SetTexts(GameRoomData gameData)
    {
        GamePlayerData loacalPlayerData =new GamePlayerData();
        if ((bool)NewOnlineGameManager.GetMyLocalPlayer().CustomProperties[Constants.Is_Player_Escaper])
            loacalPlayerData.Team = CharacterEnum.Escaper;
         else
            loacalPlayerData.Team = CharacterEnum.Alien;

        if (loacalPlayerData.Team == CharacterEnum.Escaper)
            loacalPlayerData.Objective = "Get Out Of Maze";
        else
            loacalPlayerData.Objective = "dont let escapers get out of maze";
        Objective_TMP.text = loacalPlayerData.Objective;
        if (loacalPlayerData.Succeded)
            Succeded_TMP.text = "Yes";
        else
            Succeded_TMP.text = "No";
        Team_TMP.text = loacalPlayerData.Team.ToString();
        NumberOfEscapers_TMP.text = gameData.NumberOfEscapers.ToString();
        AlienNumber_TMP.text = gameData.NumberOfAliens.ToString();
        CaculateTimePassedToFormatText(gameData.TimePassedInSeconds);
    }
    public static string ConvertToJson(GameRoomData gameData)
    {
        string JsonString = JsonUtility.ToJson(gameData);
        return JsonString;
    }

    public GameRoomData ReadFromJson(string gameDataStringFromJson)
    {
        GameRoomData gamedata = JsonUtility.FromJson<GameRoomData>(gameDataStringFromJson);
        return gamedata;
    }
    [System.Serializable]
    public class GameRoomData
    {
        public int NumberOfAliens;
        public int NumberOfEscapers;
        public float TimePassedInSeconds;
    }
    public class GamePlayerData
    {
        public CharacterEnum Team;
        public string Objective;
        public bool Succeded;
    }

}
