using System;
using System.Collections.Generic;
using UnityEngine;
namespace Permanence.Scripts.Entities
{
    public class CardProgressBar
    {
        public float MinValue { get; set; }
        public float MaxValue { get; set; }
        public float Value { get; set; }
        public bool IsShow { get; set; }
    }

    public static class CardProgressBarEvent {
        public const string ON_PROGRESS_START = "onProgressStart";
        public const string ON_PROGRESSING = "onProgressing";
        public const string ON_PROGRESS_STOP = "onProgressStop";
    }
}