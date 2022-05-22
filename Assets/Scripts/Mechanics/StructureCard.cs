using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Entities;

namespace Permanence.Scripts.Mechanics
{
    public class StructureCard : EventBusBehaviour<CardHealthBar>
    {
        [SerializeField]
        private float health;
        [SerializeField]
        private GameObject brokenOverlay;
        private float currentHealth;
        private float speedMultiplier;
        private bool isReducing;
        private CardHealthBar cardHealthBar;
        public float CurrentHealth => currentHealth;
        public float MaxHealth => health;

        protected override void Awake() {
            base.Awake();
            currentHealth = health;
            cardHealthBar = new CardHealthBar()
            {
                MinValue = 0,
                MaxValue = health
            };
        }

        private void Update() {
            if (currentHealth > 0)
            {
                cardHealthBar.Value = health - currentHealth;
                DispatchEvent(CardHealthBarEvent.ON_UPDATE, cardHealthBar);
                if (isReducing)
                {
                    currentHealth -= Time.deltaTime * speedMultiplier;
                    if (currentHealth <= 0)
                    {
                        BreakStructure();
                    }
                }
            }
        }

        public void StartReduceHealth(float speedMultiplier = 1f)
        {
            isReducing = true;
            this.speedMultiplier = speedMultiplier;
        }

        public void StopReduceHealth()
        {
            isReducing = false;
            this.speedMultiplier = 1;
        }

        public void RestoreHealth(float speedMultiplier = 1f)
        {
            brokenOverlay.SetActive(false);
            this.currentHealth = health;
        }

        private void BreakStructure()
        {
            brokenOverlay.SetActive(true);
        }
    }
}