using Permanence.Scripts.Constants;
using UnityEngine;

namespace Permanence.Scripts.Cores
{
    public class GameCard : MonoBehaviour
    {
        public CardType cardType;
        public string cardName;
        public string cardDescription;
        public string cardInstruction;
        public Sprite cardSprite;
        public Vector2 MinCardBodyPoint
        { 
            get
            {
                return (Vector2)transform.position + GameCardValue.MIN_CARD_BODY_SIZE_POINT;
            }
        }
        public Vector2 MaxCardBodyPoint
        {
            get
            {
                return (Vector2)transform.position + GameCardValue.MAX_CARD_BODY_SIZE_POINT;
            }
        }
    }
}