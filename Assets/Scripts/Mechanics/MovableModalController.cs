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
    public class MovableModalController : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        [SerializeField]
        private Canvas canvas;
        private Vector3 offset;

        public void OnBeginDrag(PointerEventData pointerData)
        {
            var rectTransform = (RectTransform)transform;
            offset = rectTransform.anchoredPosition - (Vector2)Input.mousePosition;
        }

        public void OnDrag(PointerEventData pointerData)
        {
            var rectTransform = (RectTransform)transform;
            rectTransform.anchoredPosition = Input.mousePosition + offset;
        }
    }
}