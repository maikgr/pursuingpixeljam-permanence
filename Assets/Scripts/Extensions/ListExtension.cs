using UnityEngine;
using System.Collections.Generic;

namespace Permanence.Scripts.Extensions
{
    public static class ListExtension
    {
        public static T GetRandom<T>(this List<T> list)
        {
            var randIndex = Random.Range(0, list.Count);
            return list[randIndex];
        }

        public static int LastIndex<T>(this List<T> list)
        {
            return list.Count - 1;
        }
    }
}