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
    [SerializeField] TextMeshProUGUI AlienNumber;
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
