using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Constants;
using Permanence.Scripts.Entities;
using Permanence.Scripts.UI;

namespace Permanence.Scripts.Mechanics
{
    public abstract class MaterialConsumerCard : MonoBehaviour
    {
        [SerializeField]
        public List<MaterialConsumerGameCard> requiredMaterials;
        protected DetailsModalController detailsModal;

        protected virtual void Awake() {
            detailsModal = GameObject.FindGameObjectWithTag(GameTags.GameCanvas).GetComponent<DetailsModalController>();
        }

        public virtual void AddMaterial(GameCard gameCard, int index)
        {
            if (gameCard.cardType.Equals(requiredMaterials[index].cardType))
            {
                requiredMaterials[index].material = gameCard;
                requiredMaterials[index].isFulfilled = true;
            }
        }
        
        public abstract bool SubmitMaterial();
    }

    [Serializable]
    public class MaterialConsumerGameCard
    {
        public CardType cardType;
        public GameCard material;
        public bool isFulfilled;
    }
}