using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    public Transform leftHand;
    public Transform rightHand;
    public float moveSpeed = 8f;
    private float handSwing = 0f;

    void Update()
    {
        if (!photonView.IsMine) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3(h, 0, v);

        if (moveDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);

            RaycastHit hit;
            if (!Physics.Raycast(transform.position, moveDir, out hit, 0.6f))
            {
                transform.position += moveDir * moveSpeed * Time.deltaTime;
            }

            handSwing += Time.deltaTime * 5f;
            float swing = Mathf.Sin(handSwing) * 30f;
            leftHand.localRotation = Quaternion.Euler(swing, 0, 0);
            rightHand.localRotation = Quaternion.Euler(-swing, 0, 0);
        }
        else
        {
            handSwing = 0f;
            leftHand.localRotation = Quaternion.Lerp(leftHand.localRotation, Quaternion.identity, Time.deltaTime * 10f);
            rightHand.localRotation = Quaternion.Lerp(rightHand.localRotation, Quaternion.identity, Time.deltaTime * 10f);
        }
    }
}