using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharchterChoosing : MonoBehaviourPun
{
    private PlayerController localPlayerController;

    [SerializeField] private OnlineGameManager onlineGameManager;

    [SerializeField] private SpawnPoint[] spawnPoint;

    [Header("UI")]

    [SerializeField] private Button[] CharchterChoiceButtons;
    [SerializeField] private Button[] MasteClientConfirmChioceButtons;
    [SerializeField] private Button[] PlayerConfirmChoiceButtons;
    [SerializeField] private Canvas CharchterChoiceUI;

    [Header("RPCS")]

    private const string SPAWN_PLAYER_CLIENT_RPC = nameof(SpawnPlayerRPC);
    private const string CHOOSE_CHARCHTER_RPC = nameof(ChooseCharchterRPC);
    private const string BLOCK_CHARCHTER_RPC = nameof(BlockCharchterChoiceRPC);
    private const string CONFIRM_CHARCHTER_RPC = nameof(ConfirmCharchterRPC);
    private const string ASK_FOR_RANDOM_SPAWN_POINT_RPC = nameof(AskForRandomSpawnPointRPC);
    private const string PLAYER_BUTTON_DISAPPER_RPC = nameof(PlayerButtonDisapperRPC);

    [Header("Charchters")]
    private const string WHITE_NETWORK_PLAYER_PREFAB_NAME = "WhiteNetworkPlayerObject";
    private const string RED_NETWORK_PLAYER_PREFAB_NAME = "RedNetworkPlayerObject";
    private const string GREEN_PLAYER_PREFAB_NAME = "GreenNetworkPlayerObject";
    private const string BLACK_PLAYER_PREFAB_NAME = "BlackNetworkPlayerObject";
    private const string YELLOW_PLAYER_PREFAB_NAME = "YellowNetworkPlayerObject";


    private void Start()
    {
        spawnPoint = onlineGameManager.spawnPoints;

        foreach (Button button in MasteClientConfirmChioceButtons)
        {
            button.gameObject.SetActive(false);
        }

        foreach (Button button in PlayerConfirmChoiceButtons)
        {
            button.gameObject.SetActive(false);
        }


    }

    public void ChooseChacrchter(int characterIndex)
    {
        photonView.RPC(CHOOSE_CHARCHTER_RPC, RpcTarget.AllViaServer, characterIndex);
    }


    public void ConfirmChacrchter(int characterIndex)
    {
        photonView.RPC(CONFIRM_CHARCHTER_RPC, RpcTarget.AllViaServer, characterIndex);
    }

    public void ConfirmLocation(int characterIndex)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            CharchterChoiceUI.gameObject.SetActive(false);

            if (!CharchterChoiceUI.gameObject.activeSelf)
            {
                PhotonMessageInfo info = new PhotonMessageInfo();
                photonView.RPC(ASK_FOR_RANDOM_SPAWN_POINT_RPC, RpcTarget.MasterClient, info.Sender, characterIndex);
                photonView.RPC(BLOCK_CHARCHTER_RPC, RpcTarget.AllViaServer, characterIndex);
            }
        }
    }
    private SpawnPoint GetSpawnPointByID(int targetID)
    {
        foreach (SpawnPoint spawnPoint in spawnPoint)
        {
            if (spawnPoint.ID == targetID)
                return spawnPoint;
        }

        return null;
    }


    #region RPC

    [PunRPC]
    void AskForRandomSpawnPointRPC(PhotonMessageInfo messageInfo, int charchterIndex)
    {
        List<SpawnPoint> availableSpawnPoints = new List<SpawnPoint>();
        foreach (SpawnPoint spawnPoint in spawnPoint)
        {
            if (!spawnPoint.taken)
                availableSpawnPoints.Add(spawnPoint);
        }

        SpawnPoint chosenSpawnPoint =
            availableSpawnPoints[Random.Range(0, availableSpawnPoints.Count)];
        chosenSpawnPoint.taken = true;

        bool[] takenSpawnPoints = new bool[spawnPoint.Length];
        for (int i = 0; i < spawnPoint.Length; i++)
        {
            takenSpawnPoints[i] = spawnPoint[i].taken;
        }
        photonView.RPC(SPAWN_PLAYER_CLIENT_RPC,
            messageInfo.Sender, chosenSpawnPoint.ID,
            takenSpawnPoints, charchterIndex);
    }

    [PunRPC]
    void SpawnPlayerRPC(int spawnPointID, bool[] takenSpawnPoints, int charchterIndex)
    {
        string CharchterName = string.Empty;

        switch (charchterIndex)
        {
            case 0:
                CharchterName = WHITE_NETWORK_PLAYER_PREFAB_NAME;
                break;
            case 1:
                CharchterName = RED_NETWORK_PLAYER_PREFAB_NAME;
                break;
            case 2:
                CharchterName = YELLOW_PLAYER_PREFAB_NAME;
                break;
            case 3:
                CharchterName = GREEN_PLAYER_PREFAB_NAME;
                break;
            case 4:
                CharchterName = BLACK_PLAYER_PREFAB_NAME;
                break;
        }
        SpawnPoint spawnPoint = GetSpawnPointByID(spawnPointID);
        localPlayerController =
            PhotonNetwork.Instantiate(CharchterName,
                    spawnPoint.transform.position,
                    spawnPoint.transform.rotation)
                .GetComponent<PlayerController>();

        for (int i = 0; i < takenSpawnPoints.Length; i++)
        {
            this.spawnPoint[i].taken = takenSpawnPoints[i];
        }




    }

    [PunRPC]
    public void ChooseCharchterRPC(int characterIndex)
    {
        foreach (Button button in MasteClientConfirmChioceButtons)
        {
            button.gameObject.SetActive(false);
        }

        foreach (Button button in PlayerConfirmChoiceButtons)
        {
            button.gameObject.SetActive(false);
        }

        Button pressedButton = PlayerConfirmChoiceButtons[characterIndex];
        pressedButton.gameObject.SetActive(true);

    }


    [PunRPC]
    public void ConfirmCharchterRPC(int characterIndex)
    {

        foreach (Button masterButton in MasteClientConfirmChioceButtons)
        {
            masterButton.gameObject.SetActive(false);
        }

        Button pressedButton = MasteClientConfirmChioceButtons[characterIndex];
        pressedButton.gameObject.SetActive(true);

        if (!CharchterChoiceUI.gameObject.activeSelf && PhotonNetwork.IsMasterClient)
        {
            CharchterChoiceUI.gameObject.SetActive(true);
        }


        photonView.RPC(PLAYER_BUTTON_DISAPPER_RPC, RpcTarget.AllViaServer, characterIndex);


    }

    [PunRPC]
    public void BlockCharchterChoiceRPC(int characterIndex)
    {
        CharchterChoiceButtons[characterIndex].gameObject.SetActive(false);
        MasteClientConfirmChioceButtons[characterIndex].gameObject.SetActive(false);
        PlayerConfirmChoiceButtons[characterIndex].gameObject.SetActive(false);
    }

    [PunRPC]
    public void PlayerButtonDisapperRPC(int characterIndex)
    {
        PlayerConfirmChoiceButtons[characterIndex].gameObject.SetActive(false);
    }



    #endregion

}


