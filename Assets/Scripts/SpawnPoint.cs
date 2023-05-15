using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnPoint : MonoBehaviour, IPunObservable
{
    public int ID;
    public bool taken = false;
 
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    [ContextMenu("Turn off taken")]
    void ChangeTaken()
    {
        taken = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(taken);
        }
        else if(stream.IsReading)
        {
            taken = (bool)stream.ReceiveNext();
        }
    }
}
