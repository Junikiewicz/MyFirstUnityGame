using MyRPGGame.Events;
using MyRPGGame.Statistics;
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
            ;
            stats = gameObject.AddComponent<StatBlock>();
            stats.AddStat(new Statistic(Stat.HealthRegeneration, 0));
            stats.AddStat(new Statistic(Stat.StaminaRegeneration, 0));
            stats.AddStat(new Statistic(Stat.Stamina, 0));
            stats.AddStat(new Statistic(Stat.MaximumStamina, 0));
            stats.AddStat(new Statistic(Stat.Speed, 0));
            stats.AddStat(new Statistic(Stat.Health, 0));
            stats.AddStat(new Statistic(Stat.MaximumHealth, 0));
            stats.AddStat(new Statistic(Stat.AttackDamage, 0));
            stats.AddStat(new Statistic(Stat.AttackSpeed, 0));
            stats.AddStat(new Statistic(Stat.Experimence, 0));
            stats.AddStat(new Statistic(Stat.Lvl, 0));
            stats.AddStat(new Statistic(Stat.RequiredExperimence, 0));
            stats.AddStat(new Statistic(Stat.Gold, 0));
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
            stats.ChangeStatBase(Stat.HealthRegeneration, playerStatsTemplate.startingHealthRegeneration);
            stats.ChangeStatBase(Stat.StaminaRegeneration, playerStatsTemplate.startingStaminaRegeneration);
            stats.ChangeStatBase(Stat.Stamina, playerStatsTemplate.startingMaxStamina);
            stats.ChangeStatBase(Stat.MaximumStamina, playerStatsTemplate.startingMaxStamina);
            stats.ChangeStatBase(Stat.Speed, playerStatsTemplate.startingSpeed);
            stats.ChangeStatBase(Stat.Health, playerStatsTemplate.startingMaxHealth);
            stats.ChangeStatBase(Stat.MaximumHealth, playerStatsTemplate.startingMaxHealth);
            stats.ChangeStatBase(Stat.AttackDamage, playerStatsTemplate.startingAttackDamage);
            stats.ChangeStatBase(Stat.AttackSpeed, playerStatsTemplate.startingAttackSpeed);
            stats.ChangeStatBase(Stat.Experimence, 0);
            stats.ChangeStatBase(Stat.Lvl, 1);
            stats.ChangeStatBase(Stat.RequiredExperimence, CalculateExperimenceRequired(2));
            stats.ChangeStatBase(Stat.Gold, 0);
        }
        public void RefreshWholeGUI()
        {
            EventManager.Instance.TriggerEvent(new OnPlayerStaminaChanged(stats.GetStat(Stat.Stamina)));
            EventManager.Instance.TriggerEvent(new OnPlayerMaxStaminaChanged(stats.GetStat(Stat.MaximumStamina)));
            EventManager.Instance.TriggerEvent(new OnPlayerHealthChanged(stats.GetStat(Stat.Health)));
            EventManager.Instance.TriggerEvent(new OnPlayerMaxHealthChanged(stats.GetStat(Stat.MaximumHealth)));
            EventManager.Instance.TriggerEvent(new OnPlayerExperimenceChanged(stats.GetStat(Stat.Experimence)));
            EventManager.Instance.TriggerEvent(new OnPlayerRequiredExperimenceChanged(stats.GetStat(Stat.RequiredExperimence)));
            EventManager.Instance.TriggerEvent(new OnPlayerLvlChanged(stats.GetStat(Stat.Lvl)));
            EventManager.Instance.TriggerEvent(new OnPlayerGoldChanged(stats.GetStat(Stat.Gold)));
        }

        private void Regeneration()
        {
            if (!pause)
            {
                if (stats.GetStat(Stat.Health) < stats.GetStat(Stat.MaximumHealth))
                {
                    ChangeHealth(stats.GetStat(Stat.HealthRegeneration));
                }
                if (stats.GetStat(Stat.Stamina) < stats.GetStat(Stat.MaximumStamina))
                {
                    ChangeStamina(stats.GetStat(Stat.StaminaRegeneration));
                }
            }
        }

        public void ChangeGold(double changeAmount)
        {
            double newValue = stats.GetStat(Stat.Gold) +changeAmount;
            stats.ChangeStatBase(Stat.Gold, newValue);
            EventManager.Instance.TriggerEvent(new OnPlayerGoldChanged(newValue));
        }

        public void GainExperimence(double amountOfExperimence)
        {
            double newExperimence = stats.GetStat(Stat.Experimence) +amountOfExperimence;
            stats.ChangeStatBase(Stat.Experimence, newExperimence);
            if (newExperimence > stats.GetStat(Stat.RequiredExperimence))
            {
                LvlUp();
            }
            EventManager.Instance.TriggerEvent(new OnPlayerExperimenceChanged(stats.GetStat(Stat.Experimence)));
        }
        public void ChangeHealth(double healthDifference)
        {
            double newHealth = stats.GetStat(Stat.Health) +healthDifference;
            double currentMaximumHealth = stats.GetStat(Stat.MaximumHealth);
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
            stats.ChangeStatBase(Stat.Health, newHealth);
            EventManager.Instance.TriggerEvent(new OnPlayerHealthChanged(newHealth));
        }
        public void ChangeStamina(double staminaDifference)
        {
            double newStamina = stats.GetStat(Stat.Stamina) +staminaDifference;
            double currenMaximumStamina = stats.GetStat(Stat.MaximumStamina);
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
            stats.ChangeStatBase(Stat.Stamina, newStamina);
            EventManager.Instance.TriggerEvent(new OnPlayerStaminaChanged(newStamina));
        }
        public double GetPlayerStat(Stat identifier)
        {
            return stats.GetStat(identifier);
        }
        private void LvlUp()
        {
            //Calculating lvl and experimence
            double newCurrentLvl = stats.GetStat(Stat.Lvl) +1;
            double newCurrentExperimence = stats.GetStat(Stat.Experimence) -stats.GetStat(Stat.RequiredExperimence);
            double newRequiredExperimence = CalculateExperimenceRequired(newCurrentLvl + 1);

            //Applying changes
            stats.ChangeStatBase(Stat.Lvl, newCurrentLvl);
            stats.ChangeStatBase(Stat.Experimence, newCurrentExperimence);
            stats.ChangeStatBase(Stat.RequiredExperimence, newRequiredExperimence);

            //Raising stats acording to the template
            RaiseStats();

            //Rasing events(updating GUI)
            EventManager.Instance.TriggerEvent(new OnPlayerLvlChanged(newCurrentLvl));
            EventManager.Instance.TriggerEvent(new OnPlayerMaxStaminaChanged(stats.GetStat(Stat.MaximumStamina)));
            EventManager.Instance.TriggerEvent(new OnPlayerMaxHealthChanged(stats.GetStat(Stat.MaximumHealth)));
            EventManager.Instance.TriggerEvent(new OnPlayerRequiredExperimenceChanged(newRequiredExperimence));
        }
        private void RaiseStats()
        {
            stats.ChangeStatBase(Stat.MaximumHealth, stats.GetStat(Stat.MaximumHealth) +playerStatsTemplate.healthAddedOnPromotion);
            stats.ChangeStatBase(Stat.MaximumStamina, stats.GetStat(Stat.MaximumStamina) +playerStatsTemplate.staminaAddedOnPromotion);
            stats.ChangeStatBase(Stat.AttackSpeed, stats.GetStat(Stat.AttackSpeed) +playerStatsTemplate.attackSpeedAddedOnPromotion);
            stats.ChangeStatBase(Stat.HealthRegeneration, stats.GetStat(Stat.HealthRegeneration) +playerStatsTemplate.healthRegenerationAddedOnPromotion);
            stats.ChangeStatBase(Stat.StaminaRegeneration, stats.GetStat(Stat.StaminaRegeneration) +playerStatsTemplate.staminaRegenrationAddedOnPromotion);
            stats.ChangeStatBase(Stat.Speed, stats.GetStat(Stat.Speed) +playerStatsTemplate.speedAddedOnPromotion);
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
            RefreshWholeGUI();
        }
        private void EndPause(OnPauseEnd data)
        {
            pause = false;
        }
        public void LoadPlayerDataFromSave(OnGameLoaded data)
        {
            stats.ChangeStatBase(Stat.MaximumHealth, data.saveData.maximumHealth);
            stats.ChangeStatBase(Stat.Health, data.saveData.maximumHealth);
            stats.ChangeStatBase(Stat.HealthRegeneration, data.saveData.healthRegeneration);
            stats.ChangeStatBase(Stat.MaximumStamina, data.saveData.maximumStamina);
            stats.ChangeStatBase(Stat.Stamina, data.saveData.maximumStamina);
            stats.ChangeStatBase(Stat.StaminaRegeneration, data.saveData.staminaRegeneration);
            stats.ChangeStatBase(Stat.Speed, data.saveData.speed);
            stats.ChangeStatBase(Stat.AttackSpeed, data.saveData.attackSpeed);
            stats.ChangeStatBase(Stat.Experimence, data.saveData.experimence);
            stats.ChangeStatBase(Stat.Lvl, data.saveData.lvl);
            stats.ChangeStatBase(Stat.Gold, data.saveData.gold);
            stats.ChangeStatBase(Stat.RequiredExperimence, CalculateExperimenceRequired(data.saveData.lvl));
            RefreshWholeGUI();
        }
        public void SavePlayerStatsData(OnGameSaved data)
        {
            data.saveData.maximumHealth = stats.GetStat(Stat.MaximumHealth);
            data.saveData.healthRegeneration = stats.GetStat(Stat.HealthRegeneration);
            data.saveData.maximumStamina = stats.GetStat(Stat.MaximumStamina);
            data.saveData.staminaRegeneration = stats.GetStat(Stat.StaminaRegeneration);
            data.saveData.speed = stats.GetStat(Stat.Speed);
            data.saveData.attackSpeed = stats.GetStat(Stat.AttackSpeed);
            data.saveData.experimence = stats.GetStat(Stat.Experimence);
            data.saveData.lvl = stats.GetStat(Stat.Lvl);
            data.saveData.gold = stats.GetStat(Stat.Gold);
        }

        private void Die()
        {
            EventManager.Instance.TriggerEvent(new OnPlayerKilled());
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
