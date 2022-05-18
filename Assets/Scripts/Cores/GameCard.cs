using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Permanence.Scripts.Constants;

namespace Permanence.Scripts.Cores
{
    public class GameCard<T> : EventBusBehaviour<T>
    {
        public CardType cardType;
    }

    public class GameCard : EventBusBehaviour<GameCard>
    {
        public CardType cardType;
    }
}