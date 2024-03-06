using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the player's transform

    void Update()
    {
        if (target != null)
        {
            // Set the camera's position to match the player's position
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }
    }
}