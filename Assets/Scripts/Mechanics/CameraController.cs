using Permanence.Scripts.Cores;
using UnityEngine;

namespace Permanence.Scripts.Mechanics
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Camera mainCamera;
        [SerializeField]
        private float buttonSpeed;
        [SerializeField]
        private float mouseDragSpeed;
        [SerializeField]
        private Vector2 MinCameraPos;
        [SerializeField]
        private Vector2 MaxCameraPos;

        private void LateUpdate() {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x + Input.GetAxis("Horizontal") * Time.deltaTime * buttonSpeed, MinCameraPos.x, MaxCameraPos.x),
                    Mathf.Clamp(transform.position.y + Input.GetAxis("Vertical") * Time.deltaTime * buttonSpeed, MinCameraPos.y, MaxCameraPos.y),
                    transform.position.z
                );
            }
            else if (Input.GetMouseButton(2))
            {
                transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x - Input.GetAxis("Mouse X") * mouseDragSpeed * Time.deltaTime, MinCameraPos.x, MaxCameraPos.x),
                    Mathf.Clamp(transform.position.y - Input.GetAxis("Mouse Y") * mouseDragSpeed * Time.deltaTime, MinCameraPos.y, MaxCameraPos.y),
                    transform.position.z
                );
            }
        }
    }
}