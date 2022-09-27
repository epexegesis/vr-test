using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModLogin : MonoBehaviour
{

    [SerializeField] GameObject modLogin;
    [SerializeField] GameObject guestLogin;

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.mKey.wasPressedThisFrame && !modLogin.activeSelf)
        {
            guestLogin.SetActive(false);
            modLogin.SetActive(true);
        }
    }
}
