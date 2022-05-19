using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Constants;
using Permanence.Scripts.Entities;

namespace Permanence.Scripts.Mechanics
{
    public abstract class MaterialConsumerCard : EventBusBehaviour<CardProgressBar>
    {
        [SerializeField]
        public List<MaterialConsumerGameCard> requiredMaterials;
        protected DetailsModalController detailsModal;

        #pragma warning disable 0114
        protected virtual void Awake() {
            base.Awake();
            detailsModal = GameObject.FindGameObjectWithTag(GameTags.GameCanvas).GetComponent<DetailsModalController>();
        }
        #pragma warning restore 0114

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