
namespace MyRPGGame.SaveSystem
{
    [System.Serializable]
    public class Save
    {
        public double maximumHealth;
        public double healthRegeneration;
        public double maximumStamina;
        public double staminaRegeneration;

        public double speed;
        public double attackSpeed;

        public double experimence;
        public double lvl;

        public int worldLvl;

        public string playerName;
        public string date;

    }
}