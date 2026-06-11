using UnityEngine;

namespace LUP.ES
{
    public interface IInteractable
    {
        bool TryStartInteraction(float deltaTime);
        bool CanInteract();
        void Interact();
        void ResetInteraction();

        Transform transform { get; }
        bool InterruptsOnMove { get; } 
    }
}
