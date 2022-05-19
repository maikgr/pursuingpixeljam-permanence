using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Constants;

namespace Permanence.Scripts.Mechanics {
    public class DetailsModalSlotController : MonoBehaviour
    {
        [SerializeField]
        private Image materialImage;
        [SerializeField]
        private TMP_Text materialText;
        public CardType RequiredCardType { get; private set;}
        private Camera mainCamera;

        private void Awake() {
            mainCamera = Camera.main;
        }

        public void SetSlotRequirement(CardType cardType)
        {
            materialText.text = cardType.ToString();
            RequiredCardType = cardType;
        }

        public bool TrySetSlot(GameCard gameCard)
        {
            if (!RequiredCardType.Equals(gameCard.cardType)) return false;
            materialImage.sprite = gameCard.cardSprite;
            return true;
        }

        public void ClearSlot()
        {
            materialImage.sprite = null;
            materialText.text = string.Empty;
        }
    }
}