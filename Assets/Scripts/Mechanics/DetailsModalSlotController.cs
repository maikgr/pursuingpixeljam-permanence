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
        [SerializeField]
        private Color defaultColor;
        public CardType RequiredCardType { get; private set;}
        private Camera mainCamera;
        private MaterialConsumerCard consumerCard;
        private int slotIndex;

        private void Awake() {
            mainCamera = Camera.main;
        }

        public void SetSlotRequirement(CardType cardType, MaterialConsumerCard consumerCard, int slotIndex)
        {
            if (consumerCard.requiredMaterials[slotIndex].isFulfilled)
            {
                this.materialImage.sprite = consumerCard.requiredMaterials[slotIndex].material.cardSprite;
                this.materialImage.color = Color.white;
            }
            else
            {
                ClearSlot();
            }

            materialText.text = cardType.ToString();
            RequiredCardType = cardType;
            this.consumerCard = consumerCard;
            this.slotIndex = slotIndex;
        }

        public bool TrySetSlot(GameCard gameCard)
        {
            if (!RequiredCardType.Equals(gameCard.cardType)
                || consumerCard.requiredMaterials[slotIndex].isFulfilled)
            {
                return false;
            }
            materialImage.sprite = gameCard.cardSprite;
            materialImage.color = Color.white;
            consumerCard.AddMaterial(gameCard, slotIndex);
            return true;
        }

        public void ClearSlot()
        {
            materialImage.sprite = null;
            materialText.text = string.Empty;
            materialImage.color = defaultColor;
        }
    }
}