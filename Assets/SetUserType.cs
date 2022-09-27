using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUserType : MonoBehaviour
{

    [SerializeField] PhotonManager photonManager;

    public void SetMod()
    {
        photonManager.SetTypeMod();
    }

    public void SetGuest()
    {
        photonManager.SetTypeGuest();
    }

}
