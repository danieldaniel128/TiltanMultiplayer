using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public bool IsGateOpen = true;
    [SerializeField] private GameObject gateDoor;
    [SerializeField] Transform openedPosition;
    [SerializeField] private Transform closedPosition;
    bool isDoorLocked = false;
    private float latestTimer;
    private bool canOpen = true;
    // Start is called before the first frame update


    private void Awake()
    {
     
        if (IsGateOpen) OpenGate();
        else CloseGate();
    }

    private void Update()
    {
        StartCoolDownTimer();
    }

    public void CloseGate()
    {
        IsGateOpen = false;
        gateDoor.transform.localPosition = closedPosition.localPosition;
        Debug.Log("gate closed");
    }
    public void OpenGate()
    {
        if(!canOpen) return;
        IsGateOpen = true;
        gateDoor.transform.localPosition = openedPosition.localPosition;
        Debug.Log("gate open");
    }
    
    public void LockDoor(float timerDuration)
    {
        CloseGate();
        canOpen = false;
        latestTimer = Time.time + timerDuration;
        Debug.Log("gate locked");
    }
    private void StartCoolDownTimer()
    {
        if (Time.time >= latestTimer)
        {
            canOpen = true;
        }
    }
    
}
