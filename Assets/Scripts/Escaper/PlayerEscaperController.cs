using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEscaperController : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    [SerializeField] private float speed = 5;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject escaperCamera;
    private float obfuscatedSpeed = 0;
    readonly ObfuscateAlgoritm obfuscateAlgorithm = new ObfuscateAlgoritm();
    public bool canControl = false;

    void Awake()
    {
        obfuscatedSpeed = obfuscateAlgorithm.VisibleToObfuscatedFloat(speed);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(Constants.MATCH_STARTED))
            canControl = (bool)PhotonNetwork.CurrentRoom.CustomProperties[Constants.MATCH_STARTED];
    }
    public void startTest()
    {
        mainCamera.SetActive(false);
        escaperCamera.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (canControl && photonView.IsMine) Inputs();
    }

    private void Inputs()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) Move();
    }

    private void Move()
    {
        float movementSpeed = obfuscateAlgorithm.ObfuscatedToVisibleFloat(obfuscatedSpeed);
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rb.velocity = direction * movementSpeed;

    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (info.photonView.IsMine)
        {
            //  OnlineGameManager.Instance.SetPlayerController(this); for later
            // OnlineGameManager.Instance.AddPlayerController(this);
        }
    }

}
