using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraButtonCatcher : MonoBehaviour
{
    [SerializeField] private BoxCollider cameraCollider;
    [SerializeField] private float alienGateLockDuration;
    [SerializeField] private float alienCooldown;
    private float latestTimer;
    private bool canClick = true;
    [SerializeField] private Camera camera;
    

    // Update is called once per frame
    void Update()
    {
        StartCoolDownTimer();
        RayCastButton();
    }

    private void RayCastButton()
    {
        
        // Draw Ray
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        mousePos = camera.ScreenToWorldPoint(mousePos);
       // Debug.DrawRay(transform.position, mousePos - transform.position, Color.red);
        

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit)) return;
            if (!hit.collider.CompareTag("Button") || !canClick) return;
            hit.collider.gameObject.GetComponent<GateButtonScript>().OnAlienClick.Invoke(alienGateLockDuration);
            Debug.Log("hit button");
            canClick = false;
            latestTimer = Time.time + alienCooldown;
        }
    }
    
    private void StartCoolDownTimer()
    {
        if (Time.time >= latestTimer)
        {
            canClick = true;
        }
    }
}