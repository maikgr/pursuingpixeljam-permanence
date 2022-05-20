using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Entities;

namespace Permanence.Scripts.Mechanics
{
    public class BlockerCard : EventBusBehaviour<CardProgressBar>
    {
        [SerializeField]
        private float health;
        private Collider2D cardBody;
        private GameCard targetCard;
        private float currentHealth;
        private float damageMultiplier;
        private CardProgressBar cardProgressBar;
        private bool isBeingDamaged;

        protected override void Awake()
        {
            base.Awake();
            cardBody = GetComponent<Collider2D>();
            cardProgressBar = new CardProgressBar();
            currentHealth = health;
        }

        private void Start()
        {
            var results = new RaycastHit2D[10];
            cardBody.Cast(Vector2.zero, results);
            var rayCard = results.FirstOrDefault(res => res.collider != null && res.collider.GetComponent<GameCard>() != null);
            if (rayCard.collider == null)
            {
                Destroy(gameObject);
            }

            targetCard = rayCard.collider.GetComponent<GameCard>();
            // Check if this is a resource card
            var resourceCard = targetCard.GetComponent<ResourceCardBehaviour>();
            if (resourceCard != null)
            {
                // Stop resource generation
                resourceCard.StopUseResource();
            }

            rayCard.collider.enabled = false;
        }

        private void Update()
        {
            if (isBeingDamaged)
            {
                currentHealth -= Time.deltaTime * damageMultiplier;
                cardProgressBar.Value = currentHealth / health;
                DispatchEvent(CardProgressBarEvent.ON_LOOTING_PROGRESS, cardProgressBar);
                if (currentHealth <= 0)
                {
                    ClearBlocker();
                }
            }
        }

        public void StartReduceHealth(float damageMultiplier = 1f)
        {
            isBeingDamaged = true;
            cardProgressBar.IsShow = true;
            DispatchEvent(CardProgressBarEvent.ON_LOOTING_START, cardProgressBar);
            this.damageMultiplier = damageMultiplier;
        }

        public void StopReduceHealth()
        {
            isBeingDamaged = false;
            cardProgressBar.IsShow = false;
            DispatchEvent(CardProgressBarEvent.ON_LOOTING_STOP, cardProgressBar);
            this.damageMultiplier = 1f;
        }

        private void ClearBlocker()
        {
            StopReduceHealth();
            Destroy(gameObject);
        }
    }
}
