using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Extensions;
using Permanence.Scripts.Constants;
using System;

namespace Permanence.Scripts.Mechanics
{
    public class MovableModalController : MonoBehaviour
    {
        [SerializeField]
        private Canvas canvas;

        public void DragHandler(BaseEventData data)
        {
            var pointerData = (PointerEventData)data;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)canvas.transform,
                pointerData.position + (Vector2)transform.position,
                canvas.worldCamera,
                out Vector2 mousePosition
            );

            transform.position = canvas.transform.TransformPoint(mousePosition);
        }
    }
}