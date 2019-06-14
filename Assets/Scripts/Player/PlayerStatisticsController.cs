using MyRPGGame.Events;
using MyRPGGame.Statistic;
using MyRPGGame.Templates;
using UnityEngine;

namespace MyRPGGame.Player
{
    public class PlayerStatisticsController : MonoBehaviour
    {
        [SerializeField] private PlayerStatsTemplate playerStatsTemplate;
        private StatBlock stats;
        private bool pause = true;
        private void Awake()
        {
            stats = gameObject.AddComponent<StatBlock>();
            stats.AddStat(new HealthRegeneration(0));
            stats.AddStat(new StaminaRegeneration(0));
            stats.AddStat(new Stamina(0));
            stats.AddStat(new MaximumStamina(0));
            stats.AddStat(new Speed(0));
            stats.AddStat(new Health(0));
            stats.AddStat(new MaximumHealth(0));
            stats.AddStat(new AttackDamage(0));
            stats.AddStat(new AttackSpeed(0));
            stats.AddStat(new Experimence(0));
            stats.AddStat(new Lvl(0));
            stats.AddStat(new RequiredExperimence(0));
            stats.AddStat(new Gold(0));
            EventManager.Instance.AddListener<OnGameLoaded>(LoadPlayerDataFromSave);
            EventManager.Instance.AddListener<OnGameSaved>(SavePlayerStatsData);
            EventManager.Instance.AddListener<OnPauseStart>(StartPause);
            EventManager.Instance.AddListener<OnPauseEnd>(EndPause);
            EventManager.Instance.AddListener<OnNewGame>(PlayerNewGame);
        }

        private void OnEnable()
        {
            InvokeRepeating(nameof(Regeneration), 0, 0.1f);
        }
        public void SetStartingValues()
        {
            stats.ChangeStatBase(typeof(HealthRegeneration), playerStatsTemplate.startingHealthRegeneration);
            stats.ChangeStatBase(typeof(StaminaRegeneration), playerStatsTemplate.startingStaminaRegeneration);
            stats.ChangeStatBase(typeof(Stamina), playerStatsTemplate.startingMaxStamina);
            stats.ChangeStatBase(typeof(MaximumStamina), playerStatsTemplate.startingMaxStamina);
            stats.ChangeStatBase(typeof(Speed), playerStatsTemplate.startingSpeed);
            stats.ChangeStatBase(typeof(Health), playerStatsTemplate.startingMaxHealth);
            stats.ChangeStatBase(typeof(MaximumHealth), playerStatsTemplate.startingMaxHealth);
            stats.ChangeStatBase(typeof(AttackDamage), playerStatsTemplate.startingAttackDamage);
            stats.ChangeStatBase(typeof(AttackSpeed), playerStatsTemplate.startingAttackSpeed);
            stats.ChangeStatBase(typeof(Experimence), 0);
            stats.ChangeStatBase(typeof(Lvl), 1);
            stats.ChangeStatBase(typeof(RequiredExperimence), CalculateExperimenceRequired(2));
            stats.ChangeStatBase(typeof(Gold), 0);
        }
        public void RefreshWholeGUI()
        {
            EventManager.Instance.TriggerEvent(new OnPlayerStaminaChanged(stats.GetStat(typeof(Stamina))));
            EventManager.Instance.TriggerEvent(new OnPlayerMaxStaminaChanged(stats.GetStat(typeof(MaximumStamina))));
            EventManager.Instance.TriggerEvent(new OnPlayerHealthChanged(stats.GetStat(typeof(Health))));
            EventManager.Instance.TriggerEvent(new OnPlayerMaxHealthChanged(stats.GetStat(typeof(MaximumHealth))));
            EventManager.Instance.TriggerEvent(new OnPlayerExperimenceChanged(stats.GetStat(typeof(Experimence))));
            EventManager.Instance.TriggerEvent(new OnPlayerRequiredExperimenceChanged(stats.GetStat(typeof(RequiredExperimence))));
            EventManager.Instance.TriggerEvent(new OnPlayerLvlChanged(stats.GetStat(typeof(Lvl))));
            EventManager.Instance.TriggerEvent(new OnPlayerGoldChanged(stats.GetStat(typeof(Gold))));
        }

        private void Regeneration()
        {
            if (!pause)
            {
                if (stats.GetStat(typeof(Health)) < stats.GetStat(typeof(MaximumHealth)))
                {
                    ChangeHealth(stats.GetStat(typeof(HealthRegeneration)));
                }
                if (stats.GetStat(typeof(Stamina)) < stats.GetStat(typeof(MaximumStamina)))
                {
                    ChangeStamina(stats.GetStat(typeof(StaminaRegeneration)));
                }
            }
        }

        public void ChangeGold(double changeAmount)
        {
            double newValue = stats.GetStat(typeof(Gold)) + changeAmount;
            stats.ChangeStatBase(typeof(Gold), newValue);
            EventManager.Instance.TriggerEvent(new OnPlayerGoldChanged(newValue));
        }

        public void GainExperimence(double amountOfExperimence)
        {
            double newExperimence = stats.GetStat(typeof(Experimence)) + amountOfExperimence;
            stats.ChangeStatBase(typeof(Experimence), newExperimence);
            if (newExperimence > stats.GetStat(typeof(RequiredExperimence)))
            {
                LvlUp();
            }
            EventManager.Instance.TriggerEvent(new OnPlayerExperimenceChanged(stats.GetStat(typeof(Experimence))));
        }
        public double DealDamage()//Used by enemies to get amount of damage they should receive
        {
            double damage = stats.GetStat(typeof(AttackDamage));
            return  damage+ Random.Range((float)(-0.2 * damage), (float)(0.2 * damage));
        }
        public void ChangeHealth(double healthDifference)
        {
            double newHealth = stats.GetStat(typeof(Health)) + healthDifference;
            double currentMaximumHealth = stats.GetStat(typeof(MaximumHealth));
            if (newHealth > currentMaximumHealth)
            {
                newHealth = currentMaximumHealth;
            }
            else
            {
                if (newHealth <= 0)
                {
                    newHealth = 0;
                    Die();
                }
            }
            stats.ChangeStatBase(typeof(Health), newHealth);
            EventManager.Instance.TriggerEvent(new OnPlayerHealthChanged(newHealth));
        }
        public void ChangeStamina(double staminaDifference)
        {
            double newStamina = stats.GetStat(typeof(Stamina)) + staminaDifference;
            double currenMaximumStamina = stats.GetStat(typeof(MaximumStamina));
            if (newStamina > currenMaximumStamina)
            {
                newStamina = currenMaximumStamina;
            }
            else
            {
                if (newStamina < 0)
                {
                    newStamina = 0;
                    EventManager.Instance.TriggerEvent(new OnPlayerExhausted());
                }
            }
            stats.ChangeStatBase(typeof(Stamina), newStamina);
            EventManager.Instance.TriggerEvent(new OnPlayerStaminaChanged(newStamina));
        }
        public double GetPlayerStat(System.Type identifier)
        {
            return stats.GetStat(identifier);
        }
        private void LvlUp()
        {
            //Calculating lvl and experimence
            double newCurrentLvl = stats.GetStat(typeof(Lvl)) + 1;
            double newCurrentExperimence = stats.GetStat(typeof(Experimence)) - stats.GetStat(typeof(RequiredExperimence));
            double newRequiredExperimence = CalculateExperimenceRequired(newCurrentLvl + 1);

            //Applying changes
            stats.ChangeStatBase(typeof(Lvl), newCurrentLvl);
            stats.ChangeStatBase(typeof(Experimence), newCurrentExperimence);
            stats.ChangeStatBase(typeof(RequiredExperimence), newRequiredExperimence);

            //Raising stats acording to the template
            RaiseStats();

            //Rasing events(updating GUI)
            EventManager.Instance.TriggerEvent(new OnPlayerLvlChanged(newCurrentLvl));
            EventManager.Instance.TriggerEvent(new OnPlayerMaxStaminaChanged(stats.GetStat(typeof(MaximumStamina))));
            EventManager.Instance.TriggerEvent(new OnPlayerMaxHealthChanged(stats.GetStat(typeof(MaximumHealth))));
            EventManager.Instance.TriggerEvent(new OnPlayerRequiredExperimenceChanged(newRequiredExperimence));
        }
        private void RaiseStats()
        {
            stats.ChangeStatBase(typeof(MaximumHealth), stats.GetStat(typeof(MaximumHealth)) + playerStatsTemplate.healthAddedOnPromotion);
            stats.ChangeStatBase(typeof(MaximumStamina), stats.GetStat(typeof(MaximumStamina)) + playerStatsTemplate.staminaAddedOnPromotion);
            stats.ChangeStatBase(typeof(AttackSpeed), stats.GetStat(typeof(AttackSpeed)) + playerStatsTemplate.attackSpeedAddedOnPromotion);
            stats.ChangeStatBase(typeof(HealthRegeneration), stats.GetStat(typeof(HealthRegeneration)) + playerStatsTemplate.healthRegenerationAddedOnPromotion);
            stats.ChangeStatBase(typeof(StaminaRegeneration), stats.GetStat(typeof(StaminaRegeneration)) + playerStatsTemplate.staminaRegenrationAddedOnPromotion);
            stats.ChangeStatBase(typeof(Speed), stats.GetStat(typeof(Speed)) + playerStatsTemplate.speedAddedOnPromotion);
        }
        private double CalculateExperimenceRequired(double targetLvl) //Formula from RuneScape i think
        {
            double result = 0;
            for (int i = 1; i <= targetLvl - 1; i++)
            {
                result += (Mathf.Floor(i + 300 * Mathf.Pow(2, i / 7)));
            }
            result = Mathf.Floor((float)(result / 4));
            return result;
        }
        private void StartPause(OnPauseStart data)
        {
            pause = true;
        }
        private void PlayerNewGame(OnNewGame data)
        {
            SetStartingValues();
            gameObject.SetActive(true);
            RefreshWholeGUI();
        }
        private void EndPause(OnPauseEnd data)
        {
            pause = false;
        }
        public void LoadPlayerDataFromSave(OnGameLoaded data)
        {
            stats.ChangeStatBase(typeof(MaximumHealth), data.saveData.maximumHealth);
            stats.ChangeStatBase(typeof(Health), data.saveData.maximumHealth);
            stats.ChangeStatBase(typeof(HealthRegeneration), data.saveData.healthRegeneration);
            stats.ChangeStatBase(typeof(MaximumStamina), data.saveData.maximumStamina);
            stats.ChangeStatBase(typeof(Stamina), data.saveData.maximumStamina);
            stats.ChangeStatBase(typeof(StaminaRegeneration), data.saveData.staminaRegeneration);
            stats.ChangeStatBase(typeof(Speed), data.saveData.speed);
            stats.ChangeStatBase(typeof(AttackSpeed), data.saveData.attackSpeed);
            stats.ChangeStatBase(typeof(Experimence), data.saveData.experimence);
            stats.ChangeStatBase(typeof(Lvl), data.saveData.lvl);
            stats.ChangeStatBase(typeof(Gold), data.saveData.gold);
            stats.ChangeStatBase(typeof(RequiredExperimence), CalculateExperimenceRequired(data.saveData.lvl));
            gameObject.SetActive(true);
            RefreshWholeGUI();
        }
        public void SavePlayerStatsData(OnGameSaved data)
        {
            data.saveData.maximumHealth = stats.GetStat(typeof(MaximumHealth));
            data.saveData.healthRegeneration = stats.GetStat(typeof(HealthRegeneration));
            data.saveData.maximumStamina = stats.GetStat(typeof(MaximumStamina));
            data.saveData.staminaRegeneration = stats.GetStat(typeof(StaminaRegeneration));
            data.saveData.speed = stats.GetStat(typeof(Speed));
            data.saveData.attackSpeed = stats.GetStat(typeof(AttackSpeed));
            data.saveData.experimence = stats.GetStat(typeof(Experimence));
            data.saveData.lvl = stats.GetStat(typeof(Lvl));
            data.saveData.gold = stats.GetStat(typeof(Gold));
        }

        private void Die()
        {
            EventManager.Instance.TriggerEvent(new OnPlayerKilled());
            Invoke(nameof(DeactivatePlayer), 1f);
        }
        private void DeactivatePlayer()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            CancelInvoke(nameof(Regeneration));
        }

        private void OnDestroy()
        {
            if (EventManager.Instance)
            {
                EventManager.Instance.RemoveListener<OnGameLoaded>(LoadPlayerDataFromSave);
                EventManager.Instance.RemoveListener<OnNewGame>(PlayerNewGame);
                EventManager.Instance.RemoveListener<OnGameSaved>(SavePlayerStatsData);
                EventManager.Instance.RemoveListener<OnPauseStart>(StartPause);
                EventManager.Instance.RemoveListener<OnPauseEnd>(EndPause);
            }
        }
    }
}
