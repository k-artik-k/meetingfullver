using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    private Vector3 offset = new Vector3(0, 2f, -4f);

    public void SetTarget(Transform t)
    {
        target = t;
    }

    void LateUpdate()
    {
        if (target == null)
        {
            // Refind avatar if lost
            GameObject av = GameObject.FindWithTag("Player");
            if (av != null) target = av.transform;
            return;
        }
        transform.position = target.position + target.TransformDirection(offset);
        transform.LookAt(target.position + Vector3.up);
    }
}