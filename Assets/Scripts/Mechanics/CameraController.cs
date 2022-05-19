using Permanence.Scripts.Cores;
using UnityEngine;

namespace Permanence.Scripts.Mechanics
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Camera mainCamera;
        [SerializeField]
        private float speed;

        private void Update() {
            transform.position = new Vector3(
                transform.position.x + Input.GetAxis("Horizontal") * Time.deltaTime * speed,
                transform.position.y + Input.GetAxis("Vertical") * Time.deltaTime * speed,
                transform.position.z
            );
        }
    }
}