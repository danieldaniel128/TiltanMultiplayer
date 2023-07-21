
using Photon.Pun;
using UnityEngine;

public class CameraSwitch : MonoBehaviourPun
{
    [SerializeField] private GameObject[] Cameras;
    private int currentCamersIndex = 0;

    private void Start()
    {
        foreach (GameObject camera in Cameras)
        {
            camera.SetActive(false);
            camera.GetComponent<AudioListener>().gameObject.SetActive(false);
        }

        if (currentCamersIndex == 0)
        {
            Cameras[currentCamersIndex].SetActive(true);
            Cameras[currentCamersIndex].GetComponent<AudioListener>().gameObject.SetActive(true);
        }
    }

    public void ChangeCamera(int cameraIndex)
    {
        foreach (GameObject camera in Cameras)
        {
            camera.SetActive(false);
            camera.GetComponent<AudioListener>().gameObject.SetActive(false);
        }

        Cameras[cameraIndex].SetActive(true);

        Cameras[cameraIndex].GetComponent<AudioListener>().gameObject.SetActive(true);
    }
}
