using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public bool IsGateOpen;
    [SerializeField] private GameObject gateDoor;
    [SerializeField] Transform openedPosition;
    private Transform closedPosition;
    // Start is called before the first frame update
    void Start()
    {
        closedPosition = gateDoor.transform;
        if (IsGateOpen) OpenGate();
        else CloseGate();
    }

    public void CloseGate()
    {
        IsGateOpen = false;
        gateDoor.transform.localPosition = closedPosition.localPosition;
    }
    public void OpenGate()
    {
        IsGateOpen = true;
        gateDoor.transform.localPosition = openedPosition.localPosition;
        Debug.Log("gate open");
    }
}
