using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    public Camera firstPersonCamera;

    private void Update()
    {

        if (firstPersonCamera == null)
            firstPersonCamera = FindObjectOfType<Camera>();

        if (firstPersonCamera == null)
            return;

        transform.LookAt(firstPersonCamera.transform);

    }

}
