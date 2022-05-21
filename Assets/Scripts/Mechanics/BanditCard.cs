using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Entities;
using Permanence.Scripts.Extensions;
using Permanence.Scripts.Constants;

namespace Permanence.Scripts.Mechanics
{
    public class BanditCard : EventBusBehaviour<CardProgressBar>
    {
        [SerializeField]
        private float health;
        [SerializeField]
        private BlockerCard fireCard;
        [SerializeField]
        private float fireSpawnInterval;
        private float currentHealth;
        private float damageMultiplier;
        private CardProgressBar cardProgressBar;
        private bool isBeingDamaged;
        private Vector2 fireCardOffset;
        private float nextFireSpawn;
        private Action onCardDeath;

        protected override void Awake()
        {
            base.Awake();
            cardProgressBar = new CardProgressBar()
            {
                MinValue = 0,
                MaxValue = health
            };
            currentHealth = health;
            fireCardOffset = new Vector2(0, -0.65f);
            nextFireSpawn = Time.time;
        }

        private void Update()
        {
            if (isBeingDamaged)
            {
                currentHealth -= Time.deltaTime * damageMultiplier;
                cardProgressBar.Value = currentHealth / health;
                DispatchEvent(CardProgressBarEvent.ON_PROGRESSING, cardProgressBar);
                if (currentHealth <= 0)
                {
                    isBeingDamaged = false;
                    RemoveBandit();
                }
            }
            if (currentHealth > 0 && Time.time > nextFireSpawn)
            {
                SpawnFire();
                nextFireSpawn = Time.time + fireSpawnInterval;
            }
        }

        private void SpawnFire()
        {
            var flammables = GameObject.FindObjectsOfType<StructureCard>()
                .Where(card => card.GetComponent<Collider2D>().enabled)
                .ToList();
            var fires = GameObject.FindObjectsOfType<GameCard>()
                .Where(card => card.cardType.Equals(CardType.Fire) && card.GetComponent<Collider2D>().enabled)
                .ToList();

            if (flammables.Count.Equals(0) && fires.Count.Equals(0)) return;

            // Prioritize setting resource on fire
            var targetPos = Vector2.zero;
            if (flammables.Count > 0)
            {
                targetPos = flammables.GetRandom().transform.position;
            }
            else
            {
                targetPos = fires.GetRandom().transform.position;
            }
            Instantiate(fireCard, targetPos + fireCardOffset, Quaternion.identity);
            SfxController.instance.PlayAudio(GameSfxType.Fire, targetPos);
        }

        public void StartReduceHealth(Action onCardDeath, float damageMultiplier = 1f)
        {
            this.onCardDeath = onCardDeath;
            isBeingDamaged = true;
            cardProgressBar.IsShow = true;
            DispatchEvent(CardProgressBarEvent.ON_PROGRESS_START, cardProgressBar);
            this.damageMultiplier = damageMultiplier;
        }

        public void StopReduceHealth()
        {
            isBeingDamaged = false;
            cardProgressBar.IsShow = false;
            DispatchEvent(CardProgressBarEvent.ON_PROGRESS_STOP, cardProgressBar);
            this.damageMultiplier = 1f;
        }

        private void RemoveBandit()
        {
            BanditEventController.instance.BanditRemoved();
            StopReduceHealth();
            onCardDeath.Invoke();
            Destroy(gameObject, 0.5f); // Delay to let other card proceed their work
        }
    }
}
