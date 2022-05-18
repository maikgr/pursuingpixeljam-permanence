using Permanence.Scripts.Constants;

namespace Permanence.Scripts.Cores
{
    public class GameCard<T> : EventBusBehaviour<T>
    {
        public CardType cardType;
        public string cardName;
        public string cardDescription;
        public string cardInstruction;
    }

    public class GameCard : GameCard<GameCard>
    {
    }
}