using System;
using System.Collections.Generic;
using UnityEngine;
using Permanence.Scripts.Constants;
using Permanence.Scripts.Extensions;

namespace Permanence.Scripts.Mechanics
{
    public class RiverController : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> loots;
        [SerializeField]
        private float lootTime;
        [SerializeField]
        private LootingBarController loadingBar;
        [SerializeField]
        private SpawnArea spawnArea;
        private float timeUntilNextLoot;
        private bool isLooting;

        private void Awake() {
            timeUntilNextLoot = lootTime;
            loadingBar.gameObject.SetActive(false);
        }

        private void Update() {
            if (isLooting)
            {
                timeUntilNextLoot -= Time.deltaTime;
                loadingBar.SetPercentage(timeUntilNextLoot / lootTime);
                if (timeUntilNextLoot <= 0)
                {
                    Debug.Log("Spawn loot");
                    loadingBar.ResetBar();
                    SpawnLoot();
                    timeUntilNextLoot = lootTime;
                }
            }
        }

        private void SpawnLoot() {
            var loot = loots.GetRandom();
            var spawnPoint = GetRandomSpawnPoint(spawnArea.MinLocalPoint, spawnArea.MaxLocalPoint);
            var lootObj = Instantiate(loot, spawnPoint, Quaternion.identity);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            Debug.Log("OnTriggerEnter2D");
            if (other.transform.CompareTag(GameTags.GameCard))
            {
                loadingBar.gameObject.SetActive(true);
                isLooting = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            Debug.Log("OnTriggerExit2D");
            if (other.transform.CompareTag(GameTags.GameCard))
            {
                isLooting = false;
                loadingBar.gameObject.SetActive(false);
            }
        }

        private Vector2 GetRandomSpawnPoint(Vector2 minPoint, Vector2 maxPoint)
        {
            var hasObject = true;
            Vector2 randomPos = Vector2.zero;
            while (hasObject)
            {
                randomPos = new Vector2(
                    UnityEngine.Random.Range(minPoint.x, maxPoint.x),
                    UnityEngine.Random.Range(minPoint.y, maxPoint.y)
                );
                var hitInfo = Physics2D.Raycast(randomPos, Vector2.zero);
                hasObject = hitInfo.collider != null;
            }
            return randomPos;
        }

        [Serializable]
        private class SpawnArea
        {
            public Vector2 MinLocalPoint;
            public Vector2 MaxLocalPoint;
        }
    }
}