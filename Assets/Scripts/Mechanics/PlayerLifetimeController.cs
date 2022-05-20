using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

namespace Permanence.Scripts.Mechanics
{
    [RequireComponent(typeof(WorkerCard))]
    public class PlayerLifetimeController : MonoBehaviour
    {
        [SerializeField]
        private Color startingColor;
        [SerializeField]
        private Color endColor;
        [SerializeField]
        private TMP_Text lifetimeText;
        [SerializeField]
        private float totalLifetime;
        [SerializeField]
        private GameObject OldPlayer;
        private float currentLifetime;
        private bool isLifetimeReducing;
        private AnimationCurve[] colorCurves;
        private WorkerCard worker;

        private void Awake()
        {
            worker = GetComponent<WorkerCard>();
            colorCurves = GenerateColorCurves(startingColor, endColor);
            currentLifetime = totalLifetime;
            AdjustLifetimeText();
        }

        private void Start()
        {
            worker.AddEventListener(WorkerCardEvent.ON_START_WORKING, StartReduceLifetime);
            worker.AddEventListener(WorkerCardEvent.ON_STOP_WORKING, StopReduceLifetime);
        }

        private void OnDestroy() {
            worker.RemoveEventListener(WorkerCardEvent.ON_START_WORKING, StartReduceLifetime);
            worker.RemoveEventListener(WorkerCardEvent.ON_STOP_WORKING, StopReduceLifetime);
        }

        private void Update() {
            if (isLifetimeReducing && currentLifetime > 0)
            {
                AdjustLifetimeText();
                currentLifetime -= Time.deltaTime;
                if (currentLifetime <= 0)
                {
                    StartOldAgeEvent();
                }
            }
        }

        private void AdjustLifetimeText()
        {
            lifetimeText.text = currentLifetime.ToString("F", CultureInfo.InvariantCulture);
            lifetimeText.color = new Color(
                colorCurves[0].Evaluate(totalLifetime - currentLifetime),
                colorCurves[1].Evaluate(totalLifetime - currentLifetime),
                colorCurves[2].Evaluate(totalLifetime - currentLifetime)
            );
        }

        private void StartReduceLifetime()
        {
            isLifetimeReducing = true;
        }

        private void StopReduceLifetime()
        {
            isLifetimeReducing = false;
        }

        private AnimationCurve[] GenerateColorCurves(Color startColor, Color endColor)
        {
            var redCurve = AnimationCurve.Linear(0f, startColor.r, totalLifetime, endColor.r);
            var greenCurve = AnimationCurve.Linear(0f, startColor.g, totalLifetime, endColor.g);
            var blueCurve = AnimationCurve.Linear(0f, startColor.b, totalLifetime, endColor.b);

            return new AnimationCurve[3]
            {
                redCurve,
                greenCurve,
                blueCurve
            };
        }

        private void StartOldAgeEvent()
        {
            var spawnPoint = transform.position + new Vector3(2f, 0f, transform.position.z);
            Instantiate(OldPlayer, spawnPoint, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}