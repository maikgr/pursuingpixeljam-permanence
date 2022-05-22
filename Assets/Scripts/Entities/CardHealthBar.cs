using System;
using System.Collections.Generic;
using UnityEngine;

namespace Permanence.Scripts.Entities
{
    public class CardHealthBar
    {
        public float MinValue { get; set; }
        public float MaxValue { get; set; }
        public float Value { get; set; }
    }

    public static class CardHealthBarEvent {
        public const string ON_UPDATE = "onUpdate";
    }
}