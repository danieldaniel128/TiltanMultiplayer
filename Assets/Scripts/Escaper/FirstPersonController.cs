using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviourPunCallbacks
{
     public bool canControl = false;
     [SerializeField] private float moveSpeed = 5f;
     [SerializeField] private float mouseSensitivity = 2f;
     [SerializeField] private Transform playerCamera;
     [SerializeField] private Rigidbody rb;
     private float verticalRotation = 0f;
 
     private void Start()
     {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(Constants.MATCH_STARTED))
            canControl = (bool)PhotonNetwork.CurrentRoom.CustomProperties[Constants.MATCH_STARTED];
     }
    
     private void Update()
     {
        if (canControl && photonView.IsMine)
        {
            HandleMoveInput();
            HandleCameraRotation();
        }
     }
    private void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation += mouseY;
        transform.rotation *= Quaternion.Euler(0f, mouseX, 0f);
    }
    private void HandleMoveInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 vectorDirection = new Vector3(horizontal,0,vertical);
        Vector3 movement = transform.TransformDirection(vectorDirection);
        transform.position += movement * moveSpeed * Time.deltaTime * (-1);
    }
 }

