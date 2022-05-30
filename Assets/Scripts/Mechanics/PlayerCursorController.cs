using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Extensions;
using Permanence.Scripts.Constants;
using Permanence.Scripts.UI;
using System;

namespace Permanence.Scripts.Mechanics
{
    public class PlayerCursorController : MonoBehaviour
    {
        private Camera mainCamera;
        private StackableCard selectedCard;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && selectedCard == null)
            {
                var results = Physics2D.RaycastAll(mainCamera.WorldMousePosition(), Vector2.zero);
                var topCard = results.Where(res => res.collider != null && res.collider.GetComponent<StackableCard>() != null)
                    .OrderBy(res => res.transform.position.z)
                    .FirstOrDefault();
                if (topCard.collider != null)
                {
                    selectedCard = topCard.collider.GetComponent<StackableCard>();
                    selectedCard.SelectCard();
                }
            }
            else if (Input.GetMouseButton(0) && selectedCard != null)
            {
                selectedCard.MoveCard();
            }
            else if (Input.GetMouseButtonUp(0) && selectedCard != null)
            {
                selectedCard.DropCard();
                selectedCard = null;
            }
        }
    }
}