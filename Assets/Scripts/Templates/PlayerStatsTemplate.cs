using UnityEngine;

namespace MyRPGGame.Templates
{
    [CreateAssetMenu]
    public class PlayerStatsTemplate : ScriptableObject
    {
        public double startingMaxHealth = 500;
        public double startingMaxStamina = 500;
        public double startingAttackSpeed = 0.5;
        public double startingHealthRegeneration = 1;
        public double startingStaminaRegeneration = 5;
        public double startingSpeed = 5;
        public double startingAttackDamage = 200;

        public double healthAddedOnPromotion = 100;
        public double staminaAddedOnPromotion = 100;
        public double attackSpeedAddedOnPromotion = 0.1;
        public double healthRegenerationAddedOnPromotion = 1;
        public double staminaRegenrationAddedOnPromotion = 1;
        public double speedAddedOnPromotion = 0.5;
        public double attackDamageAddedOnPromotion = 100;
    }
}