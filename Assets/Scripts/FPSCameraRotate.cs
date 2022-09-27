using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class FPSCameraRotate : MonoBehaviour
{

    PhotonView photonView;
    [SerializeField] Transform playerCamera;
    [SerializeField] float sensitivityX;
    [SerializeField] float sensitivityY;
    float xRotation = 0f;

    float mouseX, mouseY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        if (photonView.IsMine)
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -75f, 75f);
                Vector3 targetRotation = transform.eulerAngles;
                targetRotation.x = xRotation;
                playerCamera.eulerAngles = targetRotation;

                transform.Rotate(Vector3.up, mouseX * Time.deltaTime);

                if (Keyboard.current.leftAltKey.wasPressedThisFrame)
                    Cursor.lockState = CursorLockMode.None;

            }
            else
            {
                if (Keyboard.current.leftAltKey.wasPressedThisFrame)
                    Cursor.lockState = CursorLockMode.Locked;

            }

        }
    }

    public void ReceiveInput(Vector2 mouseInput)
    {
        mouseX = mouseInput.x * sensitivityX;
        mouseY = mouseInput.y * sensitivityY;
    }

}
