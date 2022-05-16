namespace Permanence.Scripts.Cores
{
    public interface ICardEvent
    {
        void OnStartInteracting(CardInteractionController other);
        void OnStopInteracting(CardInteractionController other);
    }
}