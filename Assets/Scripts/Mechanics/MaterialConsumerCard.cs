using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Constants;

namespace Permanence.Scripts.Mechanics
{
    public class MaterialConsumerCard : EventBusBehaviour<MaterialConsumerCard>
    {
        [SerializeField]
        public List<MaterialConsumerGameCard> requiredMaterials;
        private DetailsModalController detailsModal;

        protected override void Awake() {
            base.Awake();
            detailsModal = GameObject.FindGameObjectWithTag(GameTags.GameCanvas).GetComponent<DetailsModalController>();
        }

        public void AddMaterial(GameCard gameCard, int index)
        {
            if (gameCard.gameObject.Equals(gameObject))
            {
                requiredMaterials[index].material = gameCard;
                requiredMaterials[index].isFulfilled = true;
            }
        }
        
        public bool SubmitMaterial()
        {
            if (requiredMaterials.Any(mat => !mat.isFulfilled)) return false;
            GameObject.Destroy(this, 0.5f);
            return true;
        }
    }

    [Serializable]
    public class MaterialConsumerGameCard
    {
        public CardType cardType;
        public GameCard material;
        public bool isFulfilled;
    }
}