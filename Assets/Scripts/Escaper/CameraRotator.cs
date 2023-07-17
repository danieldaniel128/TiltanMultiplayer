using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField] private Transform escaper;
    float rotationX;
    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }
    // Update is called once per frame
    void Update()
    {
        rotationX += Input.GetAxis("Mouse X");
        escaper.transform.localRotation = Quaternion.Euler(0, rotationX, 0);
    }
}
