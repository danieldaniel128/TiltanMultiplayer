
using Photon.Pun;
using UnityEngine;

public class CameraSwitch : MonoBehaviourPun
{
    [SerializeField] private GameObject[] Cameras;
    private GameObject mainCamera;
    private void Start()
    {
        mainCamera = Cameras[0];
        mainCamera.SetActive(true);

        for (int i = 0; i < Cameras.Length; i++)
        {
            if (Cameras[i] != Cameras[0])
            {
                Cameras[i].SetActive(false);
            }
        }
    }
    private void Update()
    {
        ChangeCamera();
    }
    private void ChangeCamera()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            for (int i = 0; i < Cameras.Length; i++)
            {
                GameObject currentCamera = Cameras[i--];

                currentCamera.SetActive(false);

                GameObject nextCamera = Cameras[i];

                nextCamera.SetActive(true);
            }
        }

        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            for (int i = 0; i < Cameras.Length; i++)
            {
                GameObject currentCamera = Cameras[i--];

                currentCamera.SetActive(false);

                GameObject nextCamera = Cameras[i];

                nextCamera.SetActive(true);
            }
        }

    }
}
