using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    void Update()
    {
        if (!photonView.IsMine)
            return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(h, 0, v);

        if (moveDir != Vector3.zero)
        {
            // Rotate to face movement direction
            transform.rotation = Quaternion.LookRotation(moveDir);
            // Move forward
            transform.Translate(moveDir * 5f * Time.deltaTime, Space.World);
        }
    }
}