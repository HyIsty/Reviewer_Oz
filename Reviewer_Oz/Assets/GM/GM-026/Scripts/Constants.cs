using UnityEngine;

namespace GM_026
{
    [System.Serializable]
    public struct DoorRequirement
    {
        public KeyType requiredKeyType;
        public bool consumeKeyOnOpen;
    }

    public enum ItemType
    {
        None,
        Key,
        Consumable,
        Gold,
        Quest
    }

    public enum KeyType
    {
        None,
        Red,
        Blue,
        Green
    }

    public enum AltarState
    {
        Inactive,
        Active,
        Purified
    }


    public interface IInteractable
    {
        void Interact(PlayerInteraction player);
        string GetInteractionDescription();
    }

}