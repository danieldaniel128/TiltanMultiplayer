using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class SignUpManager : MonoBehaviourPun
{
    public static SignUpManager Instance;

    [SerializeField] private TMP_InputField userNameInput;
    [SerializeField] private TMP_InputField passWordInput;
    [SerializeField] private GameObject enterGameCanvas;
    [SerializeField] private GameObject loginCanvas;
    [SerializeField] private GameObject signUpCanvas;

    public string PlayerNickname;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void SignUpButtonClick()
    {
        string username = userNameInput.text;
        string password = passWordInput.text;

        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest()
        {
            RequireBothUsernameAndEmail = false,
            Username = username,
            Password = password,
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, onSignUpSucces, onSignUpFaliure);
    }

    public void LoginButtonClick()
    {
        string username = userNameInput.text;
        string password = passWordInput.text;

        LoginWithPlayFabRequest request = new LoginWithPlayFabRequest()
        {
            Username = username,
            Password = password,
        };

        PlayFabClientAPI.LoginWithPlayFab(request, onLoginSucces, onLoginFaliure);


    }

    private void onSignUpSucces(RegisterPlayFabUserResult result)
    {
        string username = userNameInput.text;
        PhotonNetwork.NickName = username;
        loginCanvas.SetActive(true);
        signUpCanvas.SetActive(false);
    }
    private void onSignUpFaliure(PlayFabError error)
    {
        Debug.LogError("sign up Failed" + error.ErrorMessage);
    }
    private void onLoginSucces(LoginResult result)
    {
        string username = userNameInput.text;
        PhotonNetwork.NickName = username;
        enterGameCanvas.SetActive(true);
        loginCanvas.SetActive(false);
        PlayerNickname = username;
        
    }
    private void onLoginFaliure(PlayFabError error)
    {
        Debug.LogError("Login Failed" + error.ErrorMessage);
    }

}
