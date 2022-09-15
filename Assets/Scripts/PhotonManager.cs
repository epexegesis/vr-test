using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{

    [Header("Photon Related")]
    [SerializeField]
    GameObject loadingScreen;
    [SerializeField]
    GameObject startScreen;

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
        startScreen.SetActive(true);
    }


}
