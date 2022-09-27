using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
#pragma warning disable 649
    PhotonView photonView;
    CharacterController characterController;
    Vector2 horizontalInput;
    [SerializeField] Animator animator;

    [SerializeField] float speed = 12f;
    //[SerializeField] float gravity = -30f;
    //Vector3 verticalVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        photonView = GetComponent<PhotonView>();
        animator = GetComponentInChildren<Animator>();

        if (!photonView.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(characterController);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
            return;

        if (photonView.IsMine)
        {

            Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * speed;
            characterController.Move(horizontalVelocity * Time.deltaTime);

            Vector3 pos = transform.position;
            pos.y = 0;
            transform.position = pos;
        }
    }

    public void ReceiveInput(Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
    }

}
