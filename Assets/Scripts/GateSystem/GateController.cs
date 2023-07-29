using UnityEngine;
using UnityEngine.Serialization;

public class GateController : MonoBehaviour
{
    [FormerlySerializedAs("IsGateOpen")] public bool isGateOpen = true;
    [SerializeField] private GameObject gateDoor;
    [SerializeField] private Transform openedPosition;
    [SerializeField] private Transform closedPosition;
    private float _latestTimer;
    private bool _canOpen = true;
    // Start is called before the first frame update
    
    private void Awake()
    {
        if (isGateOpen) OpenGate();
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
        if(!_canOpen) return;
        isGateOpen = true;
        gateDoor.transform.position = openedPosition.position;
        Debug.Log("gate open");
    }
    
    public void LockDoor(float timerDuration)
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
