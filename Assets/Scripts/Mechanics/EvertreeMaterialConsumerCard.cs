using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Permanence.Scripts.Entities;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Constants;

namespace Permanence.Scripts.Mechanics
{
    public class EvertreeMaterialConsumerCard : MaterialConsumerCard
    {
        [SerializeField]
        private ResourceSpawnArea resourceSpawnArea;
        [SerializeField]
        private float workTime;
        [SerializeField]
        private List<GameObject> level0Rewards;
        [SerializeField]
        private List<GameObject> level1Rewards;
        [SerializeField]
        private List<GameObject> level2Rewards;
        [SerializeField]
        private List<GameObject> excessRewards;
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private Sprite level1Sprite;
        [SerializeField]
        private Sprite level2Sprite;
        private List<GameObject> rewards;
        private float timeLeft;
        private bool isWorking;
        private CardProgressBar cardProgressBar;
        private int evertreeLevel;
        public int EvertreeLevel => evertreeLevel;

        protected override void Awake() {
            cardProgressBar = new CardProgressBar();
            evertreeLevel = 0;
        }
        private void Start() {
            requiredMaterials = GetRequiredMaterials(evertreeLevel);
            rewards = GetRewards(evertreeLevel);
        }

        protected virtual void Update() {
            if (isWorking)
            {
                timeLeft -= Time.deltaTime;
                cardProgressBar.Value = timeLeft/workTime;
                if (timeLeft <= 0)
                {
                    isWorking = false;
                    SpawnLoot(rewards);
                }
            }
        }

        public override bool SubmitMaterial()
        {
            if (requiredMaterials.Any(mat => !mat.isFulfilled)) return false;
            requiredMaterials.Clear();
            isWorking = true;
            cardProgressBar.IsShow = true;
            timeLeft = workTime;
            SfxController.instance.PlayAudio(GetAudioType(evertreeLevel), transform.position);
            return true;
        }

        protected void SpawnLoot(List<GameObject> loots) {
            cardProgressBar.IsShow = false;
            foreach(var loot in loots)
            {
                var spawnPoint = resourceSpawnArea.GetRandomSpawnPoint(transform.position);
                Instantiate(loot, spawnPoint, Quaternion.identity);
                SfxController.instance.PlayAudio(GameSfxType.CardSpawn, transform.position);
            }

            if (evertreeLevel < 3)
            {
                evertreeLevel += 1;
                spriteRenderer.sprite = GetEvertreeSprite(evertreeLevel);
                rewards = GetRewards(evertreeLevel);
            }
            requiredMaterials = GetRequiredMaterials(evertreeLevel);
        }

        private GameSfxType GetAudioType(int evertreeLevel)
        {
            switch(evertreeLevel)
            {
                case 0:
                    return GameSfxType.EvertreeUpgrade0;
                case 1:
                    return GameSfxType.EvertreeUpgrade1;
                default:
                case 2:
                    return GameSfxType.EvertreeUpgrade2;
            }
        }

        private Sprite GetEvertreeSprite(int evertreeLevel)
        {
            switch(evertreeLevel)
            {
                case 1:
                    return level1Sprite;
                default:
                case 2:
                    return level2Sprite;
            }
        }
        private List<MaterialConsumerGameCard> GetRequiredMaterials(int evertreeLevel)
        {
            switch(evertreeLevel)
            {
                case 0:
                    return new List<MaterialConsumerGameCard>
                    {
                        new MaterialConsumerGameCard
                        {
                            cardType = CardType.Water
                        },
                        new MaterialConsumerGameCard
                        {
                            cardType = CardType.Water
                        },
                        new MaterialConsumerGameCard
                        {
                            cardType = CardType.Water
                        },
                    };
                case 1:
                    return new List<MaterialConsumerGameCard>
                    {
                        new MaterialConsumerGameCard
                        {
                            cardType = CardType.Everfruit
                        },
                        new MaterialConsumerGameCard
                        {
                            cardType = CardType.Wood
                        },
                        new MaterialConsumerGameCard
                        {
                            cardType = CardType.Water
                        },
                    };
                case 2:
                    return new List<MaterialConsumerGameCard>
                    {
                        new MaterialConsumerGameCard
                        {
                            cardType = CardType.Everfruit
                        },
                        new MaterialConsumerGameCard
                        {
                            cardType = CardType.GoldenFish
                        },
                        new MaterialConsumerGameCard
                        {
                            cardType = CardType.IronOre
                        },
                    };
                default:
                    return new List<MaterialConsumerGameCard>
                    {
                        new MaterialConsumerGameCard
                        {
                            cardType = CardType.Everfruit
                        },
                        new MaterialConsumerGameCard
                        {
                            cardType = CardType.Everfruit
                        },
                        new MaterialConsumerGameCard
                        {
                            cardType = CardType.Everfruit
                        },
                    };
            }
        }

        private List<GameObject> GetRewards(int evertreeLevel)
        {
            switch(evertreeLevel)
            {
                case 0:
                    return level0Rewards;
                case 1:
                    return level1Rewards;
                case 2:
                    return level2Rewards;
                default:
                    return excessRewards;
            }
        }
    }
}