using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using Permanence.Scripts.Constants;
using Permanence.Scripts.Entities;
using Permanence.Scripts.Cores;

namespace Permanence.Scripts.Mechanics
{
    [RequireComponent(typeof(EventBusBehaviour<CardHealthBar>))]
    public class StructureLifetimeBarController : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer healthBar;
        private EventBusBehaviour<CardHealthBar> healthSourceCard;
        private float emptyBarPosition;
        private float fullBarPosition;
        private Color startColor;
        private Color endColor;
        private AnimationCurve[] progressCurves;

        private void Awake() {
            healthSourceCard = GetComponent<EventBusBehaviour<CardHealthBar>>();
            emptyBarPosition = StructureLifetimeBarValues.EMPTY_BAR_POSITION;
            fullBarPosition = StructureLifetimeBarValues.FULL_BAR_POSITION;
            ColorUtility.TryParseHtmlString(StructureLifetimeBarValues.START_COLOR_HEX, out startColor);
            ColorUtility.TryParseHtmlString(StructureLifetimeBarValues.END_COLOR_HEX, out endColor);
        }

        private void Start() {
            healthSourceCard.AddEventListener(CardHealthBarEvent.ON_UPDATE, SetPercentage);
        }

        private void OnDestroy() {
            healthSourceCard.RemoveEventListener(CardHealthBarEvent.ON_UPDATE, SetPercentage);
        }

        private void SetPercentage(CardHealthBar value)
        {
            if (progressCurves == null)
            {
                progressCurves = GenerateColorCurves(value);
            }
            healthBar.transform.localPosition = new Vector2(healthBar.transform.localPosition.x, progressCurves[3].Evaluate(value.Value));
            healthBar.color = new Color(
                progressCurves[0].Evaluate(value.Value),
                progressCurves[1].Evaluate(value.Value),
                progressCurves[2].Evaluate(value.Value),
                0.6f
            );
        }

        private AnimationCurve[] GenerateColorCurves(CardHealthBar cardValue)
        {
            var redCurve = AnimationCurve.Linear(cardValue.MinValue, startColor.r, cardValue.MaxValue, endColor.r);
            var greenCurve = AnimationCurve.Linear(cardValue.MinValue, startColor.g, cardValue.MaxValue, endColor.g);
            var blueCurve = AnimationCurve.Linear(cardValue.MinValue, startColor.b, cardValue.MaxValue, endColor.b);
            var positionCurve = AnimationCurve.Linear(cardValue.MinValue, fullBarPosition, cardValue.MaxValue, emptyBarPosition);

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