using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Constants;
using Permanence.Scripts.Mechanics;
using Permanence.Scripts.Entities;

namespace Permanence.Scripts.UI {
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
        private MaterialConsumerCard<CardProgressBar> eventConsumerCard;
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
            this.eventConsumerCard = null;

            materialText.text = cardType.ToString();
            RequiredCardType = cardType;
            this.consumerCard = consumerCard;
            this.slotIndex = slotIndex;
        }

        public void SetSlotRequirement(CardType cardType, MaterialConsumerCard<CardProgressBar> eventConsumerCard, int slotIndex)
        {
            if (eventConsumerCard.requiredMaterials[slotIndex].isFulfilled)
            {
                this.materialImage.sprite = eventConsumerCard.requiredMaterials[slotIndex].material.cardSprite;
                this.materialImage.color = Color.white;
            }
            else
            {
                ClearSlot();
            }
            this.consumerCard = null;

            materialText.text = cardType.ToString();
            RequiredCardType = cardType;
            this.eventConsumerCard = eventConsumerCard;
            this.slotIndex = slotIndex;
        }

        public bool TrySetSlot(GameCard gameCard)
        {
            if (!RequiredCardType.Equals(gameCard.cardType))
            {
                return false;
            }
            if (consumerCard != null && consumerCard.requiredMaterials[slotIndex].isFulfilled)
            {
                return false;
            }
            if (eventConsumerCard != null && eventConsumerCard.requiredMaterials[slotIndex].isFulfilled)
            {
                return false;
            }
            SfxController.instance.PlayAudio(GameSfxType.MaterialsPlaced, transform.position);
            materialImage.sprite = gameCard.cardSprite;
            materialImage.color = Color.white;
            if (consumerCard != null)
            {
                consumerCard.AddMaterial(gameCard, slotIndex);
            }
            if (eventConsumerCard != null)
            {
                eventConsumerCard.AddMaterial(gameCard, slotIndex);
            }
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