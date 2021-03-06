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
    public class BanditCard : EventBusBehaviour<CardHealthBar>
    {
        [SerializeField]
        private float health;
        [SerializeField]
        private BlockerCard fireCard;
        [SerializeField]
        private float fireSpawnInterval;
        private float currentHealth;
        private float damageMultiplier;
        private CardHealthBar cardHealthBar;
        private bool isBeingDamaged;
        private Vector2 fireCardOffset;
        private float nextFireSpawn;
        private Action onCardDeath;
        private int fireSpawnAmount;

        protected override void Awake()
        {
            base.Awake();
            cardHealthBar = new CardHealthBar()
            {
                MinValue = 0,
                MaxValue = health
            };
            currentHealth = health;
            fireCardOffset = new Vector2(0, -0.65f);
            nextFireSpawn = Time.timeSinceLevelLoad;
        }

        private void Update()
        {
            if (isBeingDamaged)
            {
                currentHealth -= Time.deltaTime * damageMultiplier;
                cardHealthBar.Value = health - currentHealth;
                DispatchEvent(CardHealthBarEvent.ON_UPDATE, cardHealthBar);
                if (currentHealth <= 0)
                {
                    isBeingDamaged = false;
                    RemoveBandit();
                }
            }
            if (currentHealth > 0 && Time.timeSinceLevelLoad > nextFireSpawn)
            {
                SpawnFire();
                nextFireSpawn = Time.timeSinceLevelLoad + fireSpawnInterval;
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
            var targetPos = Vector3.zero;
            if (flammables.Count > 0)
            {
                targetPos = flammables.GetRandom().transform.position;
            }
            else
            {
                targetPos = fires.GetRandom().transform.position;
            }
            targetPos = new Vector3(targetPos.x + fireCardOffset.x, targetPos.y + fireCardOffset.y, targetPos.z - 1);
            Instantiate(fireCard, targetPos, Quaternion.identity);
            SfxController.instance.PlayAudio(GameSfxType.Fire, targetPos);
        }

        public void StartReduceHealth(Action onCardDeath, float damageMultiplier = 1f)
        {
            this.onCardDeath = onCardDeath;
            isBeingDamaged = true;
            this.damageMultiplier = damageMultiplier;
        }

        public void StopReduceHealth()
        {
            isBeingDamaged = false;
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
