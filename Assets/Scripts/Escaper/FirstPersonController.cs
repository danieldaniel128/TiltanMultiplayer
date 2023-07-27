using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
     [SerializeField] private float mouseSensitivity = 2f;
     [SerializeField] private Transform playerCamera;
     [SerializeField] private Rigidbody rb;
     private float verticalRotation = 0f;
 
     private void Start()
     {
         Cursor.lockState = CursorLockMode.Locked;
         Cursor.visible = false;
     }
 
     private void Update()
     {
         // Handle character movement
         float horizontal = Input.GetAxis("Horizontal");
         float vertical = Input.GetAxis("Vertical");
         Vector3 movement = transform.forward * vertical + transform.right * horizontal;
         movement.y = 0f; // Remove vertical movement from the player
         rb.velocity = movement * moveSpeed;
 
         // Handle camera rotation
         float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
         float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;
         verticalRotation += mouseY;
         verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
         playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
         transform.rotation *= Quaternion.Euler(0f, mouseX, 0f);
     }
 }

