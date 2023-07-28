using Photon.Pun;
using UnityEngine;

public class playerAnimatorController : MonoBehaviourPun, IPunInstantiateMagicCallback 
{
    public bool canControl;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private float speed;
    private const string PLAYER_ANIMATOR_RPC = nameof(PlayerAnimatorRPC);

    private void Start()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(Constants.MATCH_STARTED))
            canControl = (bool)PhotonNetwork.CurrentRoom.CustomProperties[Constants.MATCH_STARTED];
    }

    private void Update()
    {
        PlayerAnimator();
    }
    private void PlayerAnimator()
    {
        photonView.RPC(PLAYER_ANIMATOR_RPC, RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void PlayerAnimatorRPC()
    {
        if (canControl && photonView.IsMine)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
                playerAnimator.SetTrigger("IsWalking");
            }

            else if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.back * Time.deltaTime * speed);
                playerAnimator.SetTrigger("IsIdle");
            }
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (info.photonView.IsMine)
            OnlineGameManager.Instance.SetPlayerController(this);
        OnlineGameManager.Instance.AddPlayerController(this);
    }
}
