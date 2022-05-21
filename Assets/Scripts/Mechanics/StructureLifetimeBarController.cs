using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using Permanence.Scripts.Constants;
using Permanence.Scripts.Entities;

namespace Permanence.Scripts.Mechanics
{
    [RequireComponent(typeof(StructureCard))]
    public class StructureLifetimeBarController : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer progressBar;
        private StructureCard progressCard;
        private float emptyBarPosition;
        private float fullBarPosition;
        private Color startColor;
        private Color endColor;
        private AnimationCurve[] progressCurves;

        private void Awake() {
            progressCard = GetComponent<StructureCard>();
            emptyBarPosition = StructureLifetimeBarValues.EMPTY_BAR_POSITION;
            fullBarPosition = StructureLifetimeBarValues.FULL_BAR_POSITION;
            ColorUtility.TryParseHtmlString(StructureLifetimeBarValues.START_COLOR_HEX, out startColor);
            ColorUtility.TryParseHtmlString(StructureLifetimeBarValues.END_COLOR_HEX, out endColor);
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
            progressBar.transform.localPosition = new Vector2(progressBar.transform.localPosition.x, progressCurves[3].Evaluate(cardProgressBar.Value));
            progressBar.color = new Color(
                progressCurves[0].Evaluate(cardProgressBar.Value),
                progressCurves[1].Evaluate(cardProgressBar.Value),
                progressCurves[2].Evaluate(cardProgressBar.Value)
            );
        }

        private AnimationCurve[] GenerateColorCurves(CardProgressBar progressBar)
        {
            var redCurve = AnimationCurve.Linear(progressBar.MinValue, startColor.r, progressBar.MaxValue, endColor.r);
            var greenCurve = AnimationCurve.Linear(progressBar.MinValue, startColor.g, progressBar.MaxValue, endColor.g);
            var blueCurve = AnimationCurve.Linear(progressBar.MinValue, startColor.b, progressBar.MaxValue, endColor.b);
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