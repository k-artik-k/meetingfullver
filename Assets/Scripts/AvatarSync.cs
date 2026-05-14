using Photon.Pun;
using UnityEngine;

public class AvatarSync : MonoBehaviourPun
{
    void Update()
    {
        if (photonView.IsMine)
        {
            transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2f;
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}