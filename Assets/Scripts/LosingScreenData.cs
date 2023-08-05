using UnityEngine;

public class LosingScreenData
{
    public string ConvertToJson(GameData gameData)
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
        public int alienNumber;
        public int escapersNumber;
        public string team;
        public string objective;
        public bool succeded;
        public float gameTimeMinutes;
        public float gameTimeSeconds;
    }

}
