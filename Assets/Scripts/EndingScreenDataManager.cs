using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum CharacterEnum
{
    Alien,
    Escaper
}
public class EndingScreenDataManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI AlienNumber_TMP;
    [SerializeField] TextMeshProUGUI NumberOfEscapers_TMP;
    [SerializeField] TextMeshProUGUI Team_TMP;
    [SerializeField] TextMeshProUGUI Objective_TMP;
    [SerializeField] TextMeshProUGUI Succeded_TMP;
    [SerializeField] TextMeshProUGUI Seconds_TMP;
    [SerializeField] TextMeshProUGUI Minutes_TMP;
    [SerializeField]  Button ReturnToLobby;
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
