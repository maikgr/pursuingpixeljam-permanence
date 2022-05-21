using UnityEngine;
using UnityEngine.UI;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Entities;
using System;

namespace Permanence.Scripts.Mechanics
{
    [RequireComponent(typeof(EventBusBehaviour<CardProgressBar>))]
    public class LifetimeBarController : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer progressBar;
        private EventBusBehaviour<CardProgressBar> progressCard;
        [SerializeField]
        private float emptyBarPosition;
        [SerializeField]
        private float fullBarPosition;
        [SerializeField]
        private Color startingColor;
        [SerializeField]
        private Color endColor;
        private AnimationCurve[] progressCurves;

        private void Awake() {
            progressCard = GetComponent<EventBusBehaviour<CardProgressBar>>();
        }

        private void Start() {
            progressCard.AddEventListener(CardProgressBarEvent.ON_PROGRESSING, SetPercentage);
        }

        private void OnDestroy() {
            progressCard.RemoveEventListener(CardProgressBarEvent.ON_PROGRESSING, SetPercentage);
        }

        private void SetPercentage(CardProgressBar cardProgressBar)
        {
            if (progressCurves == null)
            {
                progressCurves = GenerateColorCurves(cardProgressBar);
            }
            progressBar.transform.localPosition = new Vector2(progressCurves[3].Evaluate(cardProgressBar.Value), 0);
            progressBar.color = new Color(
                progressCurves[0].Evaluate(cardProgressBar.Value),
                progressCurves[1].Evaluate(cardProgressBar.Value),
                progressCurves[2].Evaluate(cardProgressBar.Value)
            );
        }

        private AnimationCurve[] GenerateColorCurves(CardProgressBar progressBar)
        {
            var redCurve = AnimationCurve.Linear(progressBar.MinValue, startingColor.r, progressBar.MaxValue, endColor.r);
            var greenCurve = AnimationCurve.Linear(progressBar.MinValue, startingColor.g, progressBar.MaxValue, endColor.g);
            var blueCurve = AnimationCurve.Linear(progressBar.MinValue, startingColor.b, progressBar.MaxValue, endColor.b);
            var positionCurve = AnimationCurve.Linear(progressBar.MinValue, fullBarPosition, progressBar.MaxValue, emptyBarPosition);

            return new AnimationCurve[4]
            {
                redCurve,
                greenCurve,
                blueCurve,
                positionCurve
            };
        }
    }
}
