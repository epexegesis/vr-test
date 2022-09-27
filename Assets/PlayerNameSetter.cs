using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerNameSetter : MonoBehaviour
{
    public TMP_InputField inputField;
    private void Start()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            inputField.text = PlayerPrefs.GetString("username");
            PhotonNetwork.NickName = PlayerPrefs.GetString("username");
        }
        else
        {
            inputField.text = "";
        }
    }

    public void SetPlayerName()
    {
        PhotonNetwork.NickName = inputField.text;
        PlayerPrefs.SetString("username", inputField.text);
    }
}
