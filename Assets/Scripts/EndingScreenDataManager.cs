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
    [SerializeField] Button ReturnToLobby;


    private void Start()
    {
        Debug.Log(PhotonNetwork.CurrentRoom);
        Debug.Log(PhotonNetwork.CurrentRoom.CustomProperties[Constants.Escapers_List]);
        Debug.Log(PhotonNetwork.CurrentRoom.CustomProperties[Constants.Alien_List]);
        photonView.RPC(nameof(GatherGameData), RpcTarget.AllViaServer);
    }
    private void GatherGameData()
    {
        GameData loacalGameData = new GameData();
        List<string> escapersList = PhotonNetwork.CurrentRoom.CustomProperties[Constants.Escapers_List].ToString().Split(',').ToList();
        escapersList.Remove("");
        loacalGameData.NumberOfEscapers = escapersList.Count;
        List<string> aliensList = PhotonNetwork.CurrentRoom.CustomProperties[Constants.Alien_List].ToString().Split(',').ToList();
        aliensList.Remove("");
        loacalGameData.NumberOfAliens = aliensList.Count;
        if ((bool)NewOnlineGameManager.GetMyLocalPlayer().CustomProperties[Constants.Is_Player_Escaper])
            loacalGameData.Team = CharacterEnum.Escaper;
         else
            loacalGameData.Team = CharacterEnum.Alien;
        
    }

    public void BackToLobby()
    {
        SceneManager.LoadScene(0);//local on porpuse.
    }

    public void CaculateTimePassedToFormatText(float timePassedInSeconds)
    {
        float seconds = timePassedInSeconds % 60;
        float minutes = timePassedInSeconds / 60;
        Seconds_TMP.text = seconds.ToString();
        Minutes_TMP.text = minutes.ToString();
    }
    public void SetTexts(GameData gameData)
    {
        Objective_TMP.text = gameData.Objective;
        if (gameData.Succeded)
            Succeded_TMP.text = "Yes";
        else
            Succeded_TMP.text = "No";
        Team_TMP.text = gameData.Team.ToString();
        NumberOfEscapers_TMP.text = gameData.NumberOfEscapers.ToString();
        AlienNumber_TMP.text = gameData.NumberOfAliens.ToString();
        CaculateTimePassedToFormatText(gameData.TimePassedInSeconds);
    }
    public static string ConvertToJson(GameData gameData)
    {
        string JsonString = JsonUtility.ToJson(gameData);
        return JsonString;
    }

    public GameData ReadFromJson(string gameDataStringFromJson)
    {
        GameData gamedata = JsonUtility.FromJson<GameData>(gameDataStringFromJson);
        return gamedata;
    }
    [System.Serializable]
    public class GameData
    {
        public int NumberOfAliens;
        public int NumberOfEscapers;
        public CharacterEnum Team;
        public string Objective;
        public bool Succeded;
        public float TimePassedInSeconds;
    }

}
