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
        private SpriteRenderer progressBar;
        private EventBusBehaviour<CardProgressBar> resourceCard;
        private AnimationCurve positionCurve;
        [SerializeField]
        private float emptyBarPosition;
        [SerializeField]
        private float fullBarPosition;

        private void Awake() {
            resourceCard = GetComponent<EventBusBehaviour<CardProgressBar>>();
            progressBar.gameObject.SetActive(true);
        }

        private void Start() {
            resourceCard.AddEventListener(CardProgressBarEvent.ON_PROGRESSING, SetPercentage);
            resourceCard.AddEventListener(CardProgressBarEvent.ON_PROGRESS_START, ShowLootingBar);
            resourceCard.AddEventListener(CardProgressBarEvent.ON_PROGRESS_STOP, HideLootingBar);
        }

        private void OnDestroy() {
            resourceCard.RemoveEventListener(CardProgressBarEvent.ON_PROGRESSING, SetPercentage);
            resourceCard.RemoveEventListener(CardProgressBarEvent.ON_PROGRESS_START, ShowLootingBar);
            resourceCard.RemoveEventListener(CardProgressBarEvent.ON_PROGRESS_STOP, HideLootingBar);
        }

        private void ShowLootingBar(CardProgressBar cardProgressBar) {
            if (positionCurve == null)
            {
                positionCurve = GeneratePositionCurve(cardProgressBar);
            }
            // progressBar.gameObject.SetActive(true);
        }

        private void HideLootingBar(CardProgressBar cardProgressBar) {
            // progressBar.gameObject.SetActive(false);
        }

        private void SetPercentage(CardProgressBar cardProgressBar)
        {
            if (positionCurve == null)
            {
                positionCurve = GeneratePositionCurve(cardProgressBar);
            }
            progressBar.transform.localPosition = new Vector2(positionCurve.Evaluate(cardProgressBar.Value), progressBar.transform.localPosition.y);
        }

        private AnimationCurve GeneratePositionCurve(CardProgressBar progressBar)
        {
            return AnimationCurve.Linear(progressBar.MinValue, emptyBarPosition, progressBar.MaxValue, fullBarPosition);
        }
    }
}
