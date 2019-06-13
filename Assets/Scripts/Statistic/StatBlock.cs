using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyRPGGame.Statistic
{
    public class StatBlock : MonoBehaviour
    {
        private Dictionary<System.Type, Statistic> statistics = new Dictionary<System.Type, Statistic>();
        public void AddStat(Statistic stat)
        {
            statistics.Add(stat.GetType(), stat);
        }
        public void ChangeStatBase(System.Type typeOfStat, double value)
        {
            if (statistics.TryGetValue(typeOfStat, out Statistic sts))
            {
                sts.OrginalValue = value;
                sts.CalculateCurrentValue();
            }
            else
            {
                Debug.LogError("Stat: " + typeOfStat + ",that you are trying to change, couldn't be found!");
            }
        }
        public double GetStat(System.Type typeOfStat)
        {
            if (statistics.TryGetValue(typeOfStat, out Statistic sts))
            {
                return sts.CurrentValue;
            }
            else
            {
                Debug.LogError("Stat: " + typeOfStat + ",that you are trying to get, couldn't be found!");
                return 0;
            }
        }
        public void ApplyStatModifier(System.Type typeOfStat, double value, bool isModifierMultiplicative, float time = 0)
        {
            if (statistics.TryGetValue(typeOfStat, out Statistic sts))
            {
                if (isModifierMultiplicative)
                {
                    sts.MultiplicativeModifers.Add(value);
                }
                else
                {
                    sts.AdditiveModifiers.Add(value);
                }
                sts.CalculateCurrentValue();
                if (time != 0)
                {
                    StartCoroutine(RevertStatModifier(sts, isModifierMultiplicative, value, time));
                }
            }
            else
            {
                Debug.LogError("Stat: " + typeOfStat + ",that you are trying to add modifier to, couldn't be found!");
            }
        }
        private IEnumerator RevertStatModifier(Statistic statistic, bool isModifierMultiplicative, double value, float delayTime)
        {
            yield return new WaitForSeconds(delayTime);

            if (isModifierMultiplicative)
            {
                statistic.MultiplicativeModifers.Remove(value);
            }
            else
            {
                statistic.AdditiveModifiers.Remove(value);
            }
            statistic.CalculateCurrentValue();
        }
       
    }
}
