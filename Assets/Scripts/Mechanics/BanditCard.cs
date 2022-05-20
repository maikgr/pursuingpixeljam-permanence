using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        protected override void Awake()
        {
            base.Awake();
            cardProgressBar = new CardProgressBar();
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
                DispatchEvent(CardProgressBarEvent.ON_LOOTING_PROGRESS, cardProgressBar);
                if (currentHealth <= 0)
                {
                    ClearBlocker();
                }
            }
            if (Time.time > nextFireSpawn)
            {
                SpawnFire();
                nextFireSpawn = Time.time + fireSpawnInterval;
            }
        }

        private void SpawnFire()
        {
            var flammables = GameObject.FindObjectsOfType<ResourceCardBehaviour>()
                .Where(card => card.GetComponent<Collider2D>().enabled)
                .ToList();
            var fires = GameObject.FindObjectsOfType<GameCard>()
                .Where(card => card.cardType.Equals(CardType.Fire))
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
