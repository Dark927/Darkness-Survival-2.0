using UnityEngine;

namespace Characters.Stats
{
    [System.Serializable]
    public struct CharacterStats
    {
        [Header("Main Settings")]

        public float Health;
        public float Speed;

        [Space, Header("Invincibility Settings")]

        public float InvincibilityTime;
        public Color InvincibilityColor;

        public override string ToString()
        {
            return $"HP : {Health}" +
                $"\nSpeed : {Speed}";
        }
    }
}
