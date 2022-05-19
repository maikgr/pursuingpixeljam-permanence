using System;
using System.Collections.Generic;
using UnityEngine;

namespace Permanence.Scripts.Entities
{
    [Serializable]
    public class ResourceSpawnArea
    {
        public Vector2 MinLocalPoint;
        public Vector2 MaxLocalPoint;
    }

    public static class ResourceSpawnAreaExtension
    {
        private static Vector2 GetRandomSpawnPoint(this ResourceSpawnArea spawnArea)
        {
            var hasObject = true;
            Vector2 randomPos = Vector2.zero;
            while (hasObject)
            {
                randomPos = new Vector2(
                    UnityEngine.Random.Range(spawnArea.MinLocalPoint.x, spawnArea.MaxLocalPoint.x),
                    UnityEngine.Random.Range(spawnArea.MinLocalPoint.y, spawnArea.MaxLocalPoint.y)
                );
                var hitInfo = Physics2D.Raycast(randomPos, Vector2.zero);
                hasObject = hitInfo.collider != null;
            }
            return randomPos;
        }
    }
}