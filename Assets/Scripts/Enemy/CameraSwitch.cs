
using Photon.Pun;
using UnityEngine;

public class CameraSwitch : MonoBehaviourPun
{
    [SerializeField] private GameObject[] Cameras;
    private int currentCameraIndex = 0;

    private void Start()
    {
        for (int i = 0; i < Cameras.Length; i++)
        {
            Cameras[i].SetActive(i == 0);
        }
    }

    private void Update()
    {
        ChangeCamera();
    }

    private void ChangeCamera()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Cameras[currentCameraIndex].SetActive(false);

            currentCameraIndex = (currentCameraIndex + 1) % Cameras.Length;

            Cameras[currentCameraIndex].SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Cameras[currentCameraIndex].SetActive(false);

            currentCameraIndex = (currentCameraIndex - 1 + Cameras.Length) % Cameras.Length;

            Cameras[currentCameraIndex].SetActive(true);
        }
    }
}
