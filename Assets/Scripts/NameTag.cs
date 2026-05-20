using UnityEngine;
using TMPro;
using Photon.Pun;

public class NameTag : MonoBehaviourPun
{
    public TMP_Text nameText;
    private Camera cam;

    void Update()
    {
        // Get camera every frame until found
        if (cam == null)
            cam = Camera.main;

        if (!photonView.IsMine)
        {
            // Set name
            if (nameText.text == "")
                nameText.text = photonView.Owner.NickName;

            // Face camera
            if (cam != null)
                nameText.transform.LookAt(cam.transform);
        }
        else
        {
            nameText.gameObject.SetActive(false);
        }
    }
}