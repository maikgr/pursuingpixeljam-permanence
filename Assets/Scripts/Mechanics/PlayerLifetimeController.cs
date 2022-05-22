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
    [RequireComponent(typeof(PlayerWorkerCard))]
    [RequireComponent(typeof(PlayerNameController))]
    public class PlayerLifetimeController : EventBusBehaviour<CardProgressBar>
    {
        [SerializeField]
        private TMP_Text lifetimeText;
        [SerializeField]
        private float totalLifetime;
        [SerializeField]
        private OldPlayerNameController OldPlayer;
        [SerializeField]
        private PlayerLifetimeController NewPlayer;
        private float currentLifetime;
        private bool isLifetimeReducing;
        private AnimationCurve[] colorCurves;
        private PlayerWorkerCard worker;
        private CardProgressBar cardProgressBar;
        private PlayerNameController playerNameController;
        private PlayerPrefController playerPrefController;

        protected override void Awake()
        {
            base.Awake();
            worker = GetComponent<PlayerWorkerCard>();
            playerPrefController = FindObjectOfType<PlayerPrefController>();
            currentLifetime = totalLifetime;
            cardProgressBar = new CardProgressBar()
            {
                MinValue = 0,
                MaxValue = totalLifetime
            };
            playerNameController = GetComponent<PlayerNameController>();
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
            if (currentLifetime > 0)
            {
                AdjustLifetimeText();
                cardProgressBar.Value = totalLifetime - currentLifetime;
                DispatchEvent(CardProgressBarEvent.ON_PROGRESSING, cardProgressBar);
                if (isLifetimeReducing)
                {
                    currentLifetime -= Time.deltaTime;
                    if (currentLifetime <= 0)
                    {
                        StartOldAgeEvent();
                    }
                }
            }
        }

        private void AdjustLifetimeText()
        {
            lifetimeText.text = currentLifetime.ToString("F", CultureInfo.InvariantCulture);
        }

        private void StartReduceLifetime()
        {
            isLifetimeReducing = true;
            cardProgressBar.IsShow = true;
            DispatchEvent(CardProgressBarEvent.ON_PROGRESSING, cardProgressBar);
        }

        private void StopReduceLifetime()
        {
            isLifetimeReducing = false;
            cardProgressBar.IsShow = false;
            DispatchEvent(CardProgressBarEvent.ON_PROGRESS_STOP, cardProgressBar);
        }


        private void StartOldAgeEvent()
        {
            playerPrefController.IncreaseGeneration();
            var oldSpawnPoint = transform.position + new Vector3(2f, 0f, transform.position.z);
            var newSpawnPoint = transform.position + new Vector3(-2f, 0f, transform.position.z);
            var oldPlayer = Instantiate(OldPlayer, oldSpawnPoint, Quaternion.identity);
            oldPlayer.SetName(playerNameController.CurrentName);
            var newPlayer = Instantiate(NewPlayer, newSpawnPoint, Quaternion.identity);
            newPlayer.totalLifetime = totalLifetime + 5;
            SfxController.instance.PlayAudio(Constants.GameSfxType.CardSpawn, transform.position);
            Destroy(gameObject);
        }
    }
}