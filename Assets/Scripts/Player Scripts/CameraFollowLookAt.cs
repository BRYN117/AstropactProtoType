using UnityEngine;

public class CameraFollowLookAt : MonoBehaviour
{
    public Transform target;       // Your player
    public Vector3 offset;         // Camera position relative to player

    void LateUpdate()
    {
        if (target == null) return;

        // Follow the player
        transform.position = target.position + offset;

        // Always look directly at the player
        transform.LookAt(target);
    }
}
