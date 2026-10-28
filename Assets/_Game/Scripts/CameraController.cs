using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float distance = 8f;
    public float height = 5f;
    public float mouseSensitivity = 0.2f;

    private float rotationY = 0f;

    void LateUpdate()
    {
        float mouseX = Mouse.current.delta.ReadValue().x * mouseSensitivity;

        rotationY += mouseX;

        Quaternion rotation = Quaternion.Euler(0, rotationY, 0);

        Vector3 offset = rotation * new Vector3(0, height, -distance);

        transform.position = player.position + offset;
        transform.LookAt(player.position + Vector3.up * 2);
    }
}