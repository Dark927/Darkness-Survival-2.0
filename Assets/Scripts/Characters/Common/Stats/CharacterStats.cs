using UnityEngine;

namespace Characters.Stats
{
    [System.Serializable]
    public struct CharacterStats
    {
        [Space, Header("Main Settings")]

        public float Health;
        public float Speed;
        public float Damage;


        [Space, Header("Invincibility Settings")]

        public float InvincibilityTime;
        public Color InvincibilityColor;

        public override string ToString()
        {
            return $"HP : {Health}" +
                $"\nSpeed : {Speed}" +
                $"\nDamage : {Damage}";
        }
    }
}
