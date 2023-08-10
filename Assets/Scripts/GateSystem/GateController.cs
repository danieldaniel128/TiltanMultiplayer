using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class GateController : MonoBehaviourPunCallbacks
{
    [FormerlySerializedAs("IsGateOpen")] public bool isGateOpen = true;
    [SerializeField] private Transform gateDoor;
    [SerializeField] private Transform openedPosition;
    [SerializeField] private Transform closedPosition;
    private float _latestTimer;
    private bool _canOpen = true;


    private const string Open_Gate = nameof(OpenGateRPC);
    private const string Lock_Gate = nameof(LockDoorRPC);

    private void Awake()
    {
        if (isGateOpen) OpenGateRPC();
        else CloseGate();
    }

    private void Update()
    {
        StartCoolDownTimer();
    }

    private void CloseGate()
    {
        isGateOpen = false;
        gateDoor.transform.position = closedPosition.position;
        Debug.Log("gate closed");
    }
    public void OpenGate()
    {
        photonView.RPC(Open_Gate, RpcTarget.All);
    }
    public void LockDoor(float timerDuration)
    {
        photonView.RPC(Lock_Gate, RpcTarget.All, timerDuration );
    }
    [PunRPC]
    private void OpenGateRPC()
    {
        if(!_canOpen) return;
        isGateOpen = true;
        gateDoor.transform.position = openedPosition.position;
        Debug.Log("gate open");
    }
    [PunRPC]
    public void LockDoorRPC(float timerDuration)
    {
        CloseGate();
        _canOpen = false;
        _latestTimer = Time.time + timerDuration;
        Debug.Log("gate locked");
    }
    private void StartCoolDownTimer()
    {
        if (Time.time >= _latestTimer)
        {
            _canOpen = true;
        }
    }
    
}
