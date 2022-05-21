using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Entities;
using Permanence.Scripts.Constants;
using Permanence.Scripts.Extensions;
using Permanence.Scripts.UI;

namespace Permanence.Scripts.Mechanics
{
    [RequireComponent(typeof(GameEventController))]
    public class BanditEventController : MonoBehaviour
    {
        private GameEventController eventController;
        public static BanditEventController instance;
        [SerializeField]
        private string title;
        [SerializeField]
        private string description;
        [SerializeField]
        private GameCard banditCard;
        [SerializeField]
        private GameCard fireCard;
        [SerializeField]
        private ResourceSpawnArea banditSpawnArea;
        private float nextEventCheckTime;
        private float checkInterval;
        private float eventChance;
        private bool isEventRunning;
        private int eventLevel;
        private int spawnedBanditsCount;
        
        private void Awake() {
            var instances = FindObjectsOfType<BanditEventController>();
            if (instances.Length > 1)
            {
                Destroy(gameObject);
            }
            instance = this;

            eventController = GetComponent<GameEventController>();
            nextEventCheckTime = 20;
            checkInterval = 5;
            eventChance = 0.2f;
            eventLevel = 1;
        }

        private void FixedUpdate() {
            // Starts 40s after the game, check every 5s
            if (!isEventRunning && Time.time > nextEventCheckTime)
            {
                if (Random.Range(0f, 1f) <= eventChance)
                {
                    isEventRunning = true;
                    ShowEventNotification();
                    StartCoroutine(SpawnBandits());
                }
                else
                {
                    nextEventCheckTime += checkInterval;
                }
            }
        }

        private IEnumerator SpawnBandits()
        {
            for(var i = 0; i < eventLevel; ++i)
            {
                var spawnPoint = banditSpawnArea.GetRandomSpawnPoint(Vector2.zero);
                var bandit = Instantiate(banditCard, spawnPoint, Quaternion.identity);
                SfxController.instance.PlayAudio(GameSfxType.BanditSpawn, Vector2.zero);
                yield return new WaitForSeconds(3);
            }
            spawnedBanditsCount = eventLevel;
        }


        public void BanditRemoved()
        {
            --spawnedBanditsCount;
            CheckEventEnd();
        }

        private void CheckEventEnd()
        {
            if (spawnedBanditsCount > 0) return;
            eventLevel += 1;
            nextEventCheckTime = Time.time + 90;
        }

        private void ShowEventNotification()
        {
            EventNotificationController.instance.ShowNotification(title, description);
        }
    }
}