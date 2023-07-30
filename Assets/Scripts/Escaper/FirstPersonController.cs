using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviourPunCallbacks
{
    /// </summary>
     public bool canControl = false;
     [SerializeField] private float moveSpeed = 5f;
     [SerializeField] private float mouseSensitivity = 2f;
     public GameObject PlayerCamera;
     [SerializeField] private Rigidbody rb;
     private float verticalRotation = 0f;

     // Adjustable parameters for mouse smoothing
     public float mouseSmoothingFactor = 0.1f;
     public float maxVerticalAngle = 80f;
     
     // Private variables to store the accumulated mouse input
     private float smoothMouseX = 0f;
     private float smoothMouseY = 0f;

    private void Start()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(Constants.MATCH_STARTED))
            canControl = (bool)PhotonNetwork.CurrentRoom.CustomProperties[Constants.MATCH_STARTED];
        if(photonView.IsMine)
            PlayerCamera.SetActive(true);
    }
    
     private void Update()
     {
        if (canControl && photonView.IsMine)
        {
            ActiveCamera();
            HandleMoveInput();
            HandleCameraRotation();
        }
        else if (!photonView.IsMine)
            DisableCamera();
     }
    void DisableCamera()
    {
        PlayerCamera.SetActive(false);
    }
    private void ActiveCamera()
    {
        PlayerCamera.SetActive(true);
    }
    private void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Apply mouse smoothing
        smoothMouseX = Mathf.Lerp(smoothMouseX, mouseX, mouseSmoothingFactor);
        smoothMouseY = Mathf.Lerp(smoothMouseY, mouseY, mouseSmoothingFactor);

        // Accumulate the smoothed mouse input for vertical rotation
        verticalRotation += smoothMouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);

        // Rotate the player's transform around the vertical axis (Y-axis) and apply the smoothed mouse input
        transform.rotation *= Quaternion.Euler(0f, smoothMouseX, 0f);
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

