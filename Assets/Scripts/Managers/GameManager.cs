using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public ObfuscateAlgoritm AntiCheat;
    public NewOnlineGameManager OnlineGameManager;

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

