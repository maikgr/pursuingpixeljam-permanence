using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Permanence.Scripts.Entities;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Constants;
using Permanence.Scripts.Extensions;

namespace Permanence.Scripts.Mechanics
{
    public class ChefMaterialConsumer : MaterialConsumerCard<CardProgressBar>
    {
        [SerializeField]
        private float workTime;
        [SerializeField]
        private List<GameObject> Npcs;
        [SerializeField]
        private List<GameObject> normalRewards;
        [SerializeField]
        private ResourceSpawnArea resourceSpawnArea;
        private float timeTaken;
        private bool isWorking;
        private CardProgressBar cardProgressBar;
        
        protected override void Awake() {
            base.Awake();
            cardProgressBar = new CardProgressBar
            {
                MinValue = 0,
                MaxValue = workTime
            };
            timeTaken = 0;
            isWorking = false;
        }

       private void Update() {
            if (isWorking)
            {
                timeTaken += Time.deltaTime;
                cardProgressBar.Value = timeTaken;
                DispatchEvent(CardProgressBarEvent.ON_PROGRESSING, cardProgressBar);
                if (timeTaken <= 0)
                {
                    isWorking = false;
                    if (Npcs.Count > 0)
                    {
                        SpawnNpc(Npcs);
                    }
                    else
                    {
                        SpawnLoot(normalRewards);
                    }
                }
            }
        }
        
        public override bool SubmitMaterial()
        {
            isWorking = true;
            DispatchEvent(CardProgressBarEvent.ON_PROGRESS_START, cardProgressBar);
            return true;
        }

        private void SpawnNpc(List<GameObject> npcs) {
            var npc = npcs.GetRandom();
            var spawnPoint = resourceSpawnArea.GetRandomSpawnPoint(transform.position);
            Instantiate(npc, spawnPoint, Quaternion.identity);
            SfxController.instance.PlayAudio(GameSfxType.CardSpawn, transform.position);
            Npcs.Remove(npc);
        }

        private void SpawnLoot(List<GameObject> loots) {
            var loot = loots.GetRandom();
            var spawnPoint = resourceSpawnArea.GetRandomSpawnPoint(transform.position);
            Instantiate(loot, spawnPoint, Quaternion.identity);
            SfxController.instance.PlayAudio(GameSfxType.CardSpawn, transform.position);
        }
    }
}