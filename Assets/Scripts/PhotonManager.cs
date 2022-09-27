using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Management;


public class PhotonManager : MonoBehaviourPunCallbacks
{


    public GameObject gameController;

    [Header("Photon Related")]
    [SerializeField]
    GameObject loadingScreen;
    [SerializeField]
    GameObject loginScreen;
    [SerializeField]
    GameObject userSelectScreen;
    [SerializeField]
    GameObject startScreen;

    public enum UserChoice
    {
        VR,
        Traditional
    }

    public enum UserType
    {
        Mod,
        Guest
    }

    public UserChoice choice;
    public UserType type;


    // Start is called before the first frame update
    void Start()
    {
        ConnectToServer();
    }


    void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Trying to Connect to Photon Server...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Photon Server.");
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();

    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined the Photon Lobby");
        loadingScreen.SetActive(false);
        loginScreen.SetActive(true);
    }


    public void JoinOrCreateRoom()
    {

        //CREATES ROOM INFO IF THERE ISN'T A ROOM
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 20,
            IsVisible = true,
            IsOpen = true
        };
        PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default);

    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Room 1 has been created");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log(message);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player joined the Photon room");
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public void JoinRoom()
    {
        Debug.Log("User joined the Photon room");
        PhotonNetwork.JoinRoom("Room 1");
    }

    public void SelectVR()
    {
        StartCoroutine(StartXRCoroutine());
    }

    public void SelectTraditional()
    {
        SetUserChoice(UserChoice.Traditional);

        userSelectScreen.SetActive(false);
        startScreen.SetActive(true);
    }

    XRLoader m_SelectedXRLoader;

    public IEnumerator StartXRCoroutine()
    {
        Debug.Log("Initializing XR...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
        }
        else
        {
            Debug.Log("Starting XR...");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            userSelectScreen.SetActive(false);
            startScreen.SetActive(true);
            SetUserChoice(UserChoice.VR);
        }
    }

    void SetUserChoice(UserChoice userChoice)
    {

        if (userChoice == UserChoice.Traditional)
            gameController.GetComponent<MainSceneController>().userChoice = MainSceneController.UserChoice.Traditional;
        else
            gameController.GetComponent<MainSceneController>().userChoice = MainSceneController.UserChoice.VR;

    }

    public void SetTypeGuest()
    {
        type = UserType.Guest;
        gameController.GetComponent<MainSceneController>().userType = MainSceneController.UserType.Guest;
    }

    public void SetTypeMod()
    {
        type = UserType.Mod;
        gameController.GetComponent<MainSceneController>().userType = MainSceneController.UserType.Mod;
    }

}
