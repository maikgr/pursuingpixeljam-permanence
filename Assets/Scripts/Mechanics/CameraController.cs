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

        private void LateUpdate() {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                transform.position = new Vector3(
                    transform.position.x + Input.GetAxis("Horizontal") * Time.deltaTime * buttonSpeed,
                    transform.position.y + Input.GetAxis("Vertical") * Time.deltaTime * buttonSpeed,
                    transform.position.z
                );
            }
            else if (Input.GetMouseButton(2))
            {
                transform.position = new Vector3(
                    transform.position.x - Input.GetAxis("Mouse X") * mouseDragSpeed * Time.deltaTime,
                    transform.position.y - Input.GetAxis("Mouse Y") * mouseDragSpeed * Time.deltaTime,
                    transform.position.z
                );
            }
        }
    }
}