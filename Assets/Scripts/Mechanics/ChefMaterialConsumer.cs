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
        private List<MaterialConsumerGameCard> defaultMats;
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
            defaultMats = CopyRequirement(requiredMaterials);
        }

       private void LateUpdate() {
            cardProgressBar.Value = timeTaken;
            DispatchEvent(CardProgressBarEvent.ON_PROGRESSING, cardProgressBar);
            if (isWorking)
            {
                timeTaken += Time.deltaTime;
                if (timeTaken >= workTime)
                {
                    isWorking = false;
                    timeTaken = 0;
                    if (Npcs.Count > 0)
                    {
                        SpawnNpc(Npcs);
                    }
                    else
                    {
                        SpawnLoot(normalRewards);
                    }
                    requiredMaterials = CopyRequirement(defaultMats);
                }
            }
        }
        
        public override bool SubmitMaterial()
        {
            if (requiredMaterials.Any(mat => !mat.isFulfilled)) return false;
            requiredMaterials.Clear();
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

        private List<MaterialConsumerGameCard> CopyRequirement(List<MaterialConsumerGameCard> source)
        {
            var newList = new List<MaterialConsumerGameCard>();
            source.ForEach(s => {
                newList.Add(new MaterialConsumerGameCard
                {
                    isFulfilled = !!s.isFulfilled,
                    cardType = s.cardType,
                    material = null
                });
            });

            return newList;
        }
    }
}