using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AlienCanvasManager : MonoBehaviourPunCallbacks
{
 [SerializeField] private Canvas alienCanvas;

 private void Start()
 {
     if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(Constants.MATCH_STARTED))
     {
      alienCanvas.GetComponent<Canvas>().enabled = (bool)PhotonNetwork.CurrentRoom.CustomProperties[Constants.MATCH_STARTED];
      Debug.Log("game start");   
     }

    
 }

 public void ActivateAlienCanvas(float timerDuration)
    {
      alienCanvas.GetComponent<Canvas>().enabled = true;
    }
}
