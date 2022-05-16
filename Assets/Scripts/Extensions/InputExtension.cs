using UnityEngine;

namespace Permanence.Scripts.Extensions
{
    public static class InputExtension
    {
        public static Vector2 WorldPosition(this Input input, Vector2 objPos)
        {
            var mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = Camera.main.WorldToScreenPoint(objPos).z;
            return Camera.main.ScreenToWorldPoint(mouseScreenPos);
        }

        public static Vector2 World2DPosition(this Camera camera, Vector2 mousePos)
        {
            var pos = camera.ScreenToWorldPoint(mousePos);
            return new Vector2(pos.x, pos.y);
        }
    }
}