using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    private Vector3 offset = new Vector3(0, 0.5f, 0);

    public void SetTarget(Transform t)
    {
        target = t;
        Debug.Log("Camera target set: " + t.name);
    }

    void LateUpdate()
    {
        if (target == null)
        {
            // Find avatar if target lost
            GameObject avatar = GameObject.FindWithTag("Player");
            if (avatar != null)
                target = avatar.transform;
            return;
        }

        transform.position = target.position + offset;
        transform.rotation = target.rotation;
    }
}