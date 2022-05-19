using System;
using System.Collections.Generic;

namespace Permanence.Scripts.Entities
{
    public class CardProgressBar
    {
        public float Value { get; set; }
        public bool IsShow { get; set; }
    }
    public static class CardProgressBarEvent {
        public const string ON_LOOTING_START = "onLootingStart";
        public const string ON_LOOTING_PROGRESS = "onLootingProgress";
        public const string ON_LOOTING_STOP = "onLootingStop";
    }
}