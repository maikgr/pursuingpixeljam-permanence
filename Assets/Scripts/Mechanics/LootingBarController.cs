using UnityEngine;
using UnityEngine.UI;
using Permanence.Scripts.Cores;
using System;

namespace Permanence.Scripts.Mechanics
{
    [RequireComponent(typeof(ResourceCardBehaviour))]
    public class LootingBarController : MonoBehaviour
    {
        [SerializeField]
        private Transform lootingBar;
        private ResourceCardBehaviour resourceCard;

        private void Awake() {
            resourceCard = GetComponent<ResourceCardBehaviour>();
            lootingBar.gameObject.SetActive(false);
        }

        private void Start() {
            resourceCard.AddEventListener(ResourceCardEvent.ON_LOOTING_PROGRESS, SetPercentage);
            resourceCard.AddEventListener(ResourceCardEvent.ON_LOOTING_START, ShowLootingBar);
            resourceCard.AddEventListener(ResourceCardEvent.ON_LOOTING_STOP, HideLootingBar);
        }

        private void OnDestroy() {
            resourceCard.RemoveEventListener(ResourceCardEvent.ON_LOOTING_PROGRESS, SetPercentage);
            resourceCard.RemoveEventListener(ResourceCardEvent.ON_LOOTING_START, ShowLootingBar);
            resourceCard.RemoveEventListener(ResourceCardEvent.ON_LOOTING_STOP, HideLootingBar);
        }

        private void ShowLootingBar(dynamic value) {
            lootingBar.gameObject.SetActive(true);
        }

        private void HideLootingBar(dynamic value) {
            lootingBar.gameObject.SetActive(false);
        }

        private void SetPercentage(dynamic percentage)
        {
            var progress = (float)percentage;
            lootingBar.localPosition = new Vector2(0 - progress * lootingBar.localScale.x, 0);
        }
    }
}
