using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    public Transform leftHand;
    public Transform rightHand;
    private float handSwing = 0f;
    private bool inputEnabled = true;

    void OnApplicationFocus(bool hasFocus)
    {
        inputEnabled = hasFocus;
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        if (!inputEnabled) return;

        // Also check if any input field is focused
        if (UnityEngine.EventSystems.EventSystem.current != null &&
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != null)
            return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(h, 0, v);

        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDir);
            transform.Translate(moveDir * 5f * Time.deltaTime, Space.World);

            handSwing += Time.deltaTime * 5f;
            float swing = Mathf.Sin(handSwing) * 30f;
            leftHand.localRotation = Quaternion.Euler(swing, 0, 0);
            rightHand.localRotation = Quaternion.Euler(-swing, 0, 0);
        }
        else
        {
            handSwing = 0f;
            leftHand.localRotation = Quaternion.identity;
            rightHand.localRotation = Quaternion.identity;
        }
    }
}