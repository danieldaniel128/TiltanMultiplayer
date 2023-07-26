
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitch : MonoBehaviourPun
{
    [SerializeField] private GameObject[] Cameras;

    [SerializeField] private CameraState[] cameraState;

    [SerializeField] private GameObject objectToTurnOff;

    private int currentCameraIndex = 0;

    private const string EFFECT_WORLD_RPC = nameof(EffectWorldRPC);


    private void Start()
    {
        foreach (GameObject camera in Cameras)
        {
            camera.SetActive(false);
            camera.GetComponent<AudioListener>().gameObject.SetActive(false);
        }

        if (currentCameraIndex == 0)
        {
            Cameras[currentCameraIndex].SetActive(true);
            Cameras[currentCameraIndex].GetComponent<AudioListener>().gameObject.SetActive(true);
        }

        UpdateCameraButtons();
    }

    private void Update()
    {
        rightArrow();
        leftArrow();
    }

    public void effectWorld()
    {
        photonView.RPC(EFFECT_WORLD_RPC, RpcTarget.AllViaServer);
    }
    public void ChangeCameraWithButtons(int cameraIndex)
    {
        currentCameraIndex = cameraIndex;

        foreach (GameObject camera in Cameras)
        {
            camera.SetActive(false);
            camera.GetComponent<AudioListener>().gameObject.SetActive(false);
        }

        Cameras[cameraIndex].SetActive(true);

        Cameras[cameraIndex].GetComponent<AudioListener>().gameObject.SetActive(true);

        UpdateCameraButtons();
    }

    public void rightArrow()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Cameras[currentCameraIndex].SetActive(false);

            Cameras[currentCameraIndex].GetComponent<AudioListener>().gameObject.SetActive(false);

            currentCameraIndex = (currentCameraIndex + 1) % Cameras.Length;

            Cameras[currentCameraIndex].SetActive(true);

            Cameras[currentCameraIndex].GetComponent<AudioListener>().gameObject.SetActive(true);

            UpdateCameraButtons();
        }

    }

    public void leftArrow()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

            Cameras[currentCameraIndex].SetActive(false);

            Cameras[currentCameraIndex].GetComponent<AudioListener>().gameObject.SetActive(false);

            currentCameraIndex = (currentCameraIndex - 1 + Cameras.Length) % Cameras.Length;

            Cameras[currentCameraIndex].SetActive(true);

            Cameras[currentCameraIndex].GetComponent<AudioListener>().gameObject.SetActive(true);

            UpdateCameraButtons();
        }
    }

    [PunRPC]
    private void EffectWorldRPC()
    {
        objectToTurnOff.SetActive(false);
    }

    private void UpdateCameraButtons()
    {
        foreach (CameraState cameraState in cameraState)
        {
            List<Button> buttons = cameraState.buttons;

            foreach (Button button in buttons)
            {
                button.gameObject.SetActive(cameraState.CameraIndex == currentCameraIndex);
            }
        }
    }

}
