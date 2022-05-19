using UnityEngine;
using UnityEngine.UI;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Entities;
using System;

namespace Permanence.Scripts.Mechanics
{
    [RequireComponent(typeof(EventBusBehaviour<CardProgressBar>))]
    public class LootingBarController : MonoBehaviour
    {
        [SerializeField]
        private Transform lootingBar;
        private EventBusBehaviour<CardProgressBar> resourceCard;

        private void Awake() {
            resourceCard = GetComponent<EventBusBehaviour<CardProgressBar>>();
            lootingBar.gameObject.SetActive(false);
        }

        private void Start() {
            resourceCard.AddEventListener(CardProgressBarEvent.ON_LOOTING_PROGRESS, SetPercentage);
            resourceCard.AddEventListener(CardProgressBarEvent.ON_LOOTING_START, ShowLootingBar);
            resourceCard.AddEventListener(CardProgressBarEvent.ON_LOOTING_STOP, HideLootingBar);
        }

        private void OnDestroy() {
            resourceCard.RemoveEventListener(CardProgressBarEvent.ON_LOOTING_PROGRESS, SetPercentage);
            resourceCard.RemoveEventListener(CardProgressBarEvent.ON_LOOTING_START, ShowLootingBar);
            resourceCard.RemoveEventListener(CardProgressBarEvent.ON_LOOTING_STOP, HideLootingBar);
        }

        private void ShowLootingBar(CardProgressBar cardProgressBar) {
            lootingBar.gameObject.SetActive(true);
        }

        private void HideLootingBar(CardProgressBar cardProgressBar) {
            lootingBar.gameObject.SetActive(false);
        }

        private void SetPercentage(CardProgressBar cardProgressBar)
        {
            lootingBar.localPosition = new Vector2(0 - cardProgressBar.Value * lootingBar.localScale.x, 0);
        }
    }
}
