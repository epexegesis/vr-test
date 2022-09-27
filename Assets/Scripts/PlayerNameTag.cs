using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerNameTag : MonoBehaviourPun
{

    [SerializeField] TextMeshProUGUI nameText;

    // Start is called before the first frame update
    void Start()
    {

        if (photonView.IsMine) { return; }
        SetName();

    }

    private void SetName()
    {
        nameText.text = photonView.Owner.NickName;
    }


}
