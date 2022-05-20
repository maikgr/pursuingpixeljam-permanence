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
        public static Vector2 GetRandomSpawnPoint(this ResourceSpawnArea spawnArea, Vector2 sourcePos)
        {
            var hasObject = true;
            Vector2 randomPos = Vector2.zero;
            var maxTries = 50; // In case there's no suitable spawn point found
            while (hasObject || maxTries > 0)
            {
                randomPos = new Vector2(
                    UnityEngine.Random.Range(sourcePos.x + spawnArea.MinLocalPoint.x, sourcePos.x + spawnArea.MaxLocalPoint.x),
                    UnityEngine.Random.Range(sourcePos.y + spawnArea.MinLocalPoint.y, sourcePos.y + spawnArea.MaxLocalPoint.y)
                );
                var hitInfo = Physics2D.Raycast(randomPos, Vector2.zero);
                hasObject = hitInfo.collider != null;
                --maxTries;
            }
            return randomPos;
        }
    }
}