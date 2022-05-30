using UnityEngine;
using System.Collections.Generic;
using Permanence.Scripts.Mechanics;
using System.Text;

namespace Permanence.Scripts.Extensions
{
    public static class ListExtension
    {
        public static T GetRandom<T>(this List<T> list)
        {
            var randIndex = UnityEngine.Random.Range(0, list.Count);
            return list[randIndex];
        }

        public static T GetRandom<T>(this T[] array)
        {
            var randIndex = UnityEngine.Random.Range(0, array.Length);
            return array[randIndex];
        }

        public static int LastIndex<T>(this List<T> list)
        {
            return list.Count - 1;
        }

        public static string StringFormat(this List<StackableCard> list)
        {
            var sb = new StringBuilder();
            sb.Append("[");
            list.ForEach(item => sb.Append(item.ToString() + ","));
            sb.Length -= 1;
            sb.Append("]");
            return sb.ToString();
        }
    }
}