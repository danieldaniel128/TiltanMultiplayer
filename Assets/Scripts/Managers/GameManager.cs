using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EndingScreenDataManager;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public ObfuscateAlgoritm AntiCheat;
    public NewOnlineGameManager OnlineGameManager;

    public GameRoomData MyGameData;

    public delegate void WinCond();
    public WinCond winCond;

    public delegate void LoseCond();
    public LoseCond loseCond;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        winCond = InvokeWinCond;
        loseCond = InvokeLoseCond;
        AntiCheat = new ObfuscateAlgoritm();
        MyGameData = new GameRoomData();
    }
    private void InvokeWinCond()
    {
        OnlineGameManager.photonView.RPC("WinGame", Photon.Pun.RpcTarget.All);
        Debug.Log("Win invoked");
    }
    private void InvokeLoseCond()
    {
        Debug.Log("Lose invoked");
    }
}

