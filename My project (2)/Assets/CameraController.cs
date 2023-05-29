using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 2f, -5f);
    public Vector2 rotationSpeed = new Vector2(2f, 2f);
    public Vector2 rotationClamp = new Vector2(-80f, 80f);
    public bool lockCursorOnClick = true;

    private float rotationX;
    private float rotationY;

    private void Start()
    {
        if (lockCursorOnClick)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void LateUpdate()
    {
        // Check if the right mouse button is clicked to rotate the camera
        if (Input.GetMouseButton(1))
        {
            rotationX += Input.GetAxis("Mouse X") * rotationSpeed.x;
            rotationY -= Input.GetAxis("Mouse Y") * rotationSpeed.y;

            rotationY = Mathf.Clamp(rotationY, rotationClamp.x, rotationClamp.y);

            Quaternion targetRotation = Quaternion.Euler(rotationY, rotationX, 0f);
            transform.rotation = targetRotation;
        }

        if (lockCursorOnClick && Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Update camera position based on the target's position and offset
        transform.position = target.position + offset;
    }
}
