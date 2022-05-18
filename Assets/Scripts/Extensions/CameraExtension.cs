using UnityEngine;

namespace Permanence.Scripts.Extensions
{
    public static class CameraExtension
    {
        public static Vector3 WorldMousePosition(this Camera camera) {
            return camera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}