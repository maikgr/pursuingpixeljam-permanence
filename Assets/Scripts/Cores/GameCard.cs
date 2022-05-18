using Permanence.Scripts.Constants;
using UnityEngine;

namespace Permanence.Scripts.Cores
{
    public class GameCard<T> : EventBusBehaviour<T>
    {
        public CardType cardType;
        public string cardName;
        public string cardDescription;
        public string cardInstruction;
        public Sprite cardSprite;
    }

    public class GameCard : GameCard<GameCard>
    {
    }
}