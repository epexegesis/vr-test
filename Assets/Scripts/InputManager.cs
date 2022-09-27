using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class InputManager : MonoBehaviour
{

    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] FPSCameraRotate cameraRotate;

    PhotonView photonView;

    PlayerControls controls;
    PlayerControls.MovementActions movement;

    Vector2 horizontalInput;
    Vector2 mouseInput;

    private void Awake()
    {
        controls = new PlayerControls();
        movement = controls.Movement;

        movement.Movement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();

        movement.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        movement.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();


    }

    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {

        if (!photonView.IsMine)
            return;

        if (photonView.IsMine)
        {
            playerMovement.ReceiveInput(horizontalInput);
            cameraRotate.ReceiveInput(mouseInput);
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDestroy()
    {
        controls.Disable();
    }


}
