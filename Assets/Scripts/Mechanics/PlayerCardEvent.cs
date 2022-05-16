using UnityEngine;
using Permanence.Scripts.Cores;
using Permanence.Scripts.Constants;

namespace Permanence.Scripts.Mechanics
{
    public class PlayerCardEvent : MonoBehaviour, ICardEvent
    {
        public void OnStartInteracting(CardInteractionController other)
        {
            if (other.cardType == CardType.Resource)
            {
                var resource = other.gameObject.GetComponent<ResourceCardBehaviour>();
                resource.StartUseResource();
            }
        }

        public void OnStopInteracting(CardInteractionController other)
        {
            if (other.cardType == CardType.Resource)
            {
                var resource = other.gameObject.GetComponent<ResourceCardBehaviour>();
                resource.StopUseResource();
            }
        }
    }
}