using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        private CardType slotCardType;

        public void SetSlotRequirement(CardType cardType)
        {
            materialText.text = cardType.ToString();
            slotCardType = cardType;
        }

        public bool TrySetSlot(GameCard gameCard)
        {
            if (!slotCardType.Equals(gameCard.cardType)) return false;
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