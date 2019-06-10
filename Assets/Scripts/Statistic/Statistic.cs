using System.Collections.Generic;

namespace MyRPGGame.Statistic
{
    public abstract class Statistic
    {
        public Statistic(double startingValue)
        {
            OrginalValue = CurrentValue = startingValue;
            AdditiveModifiers = new List<double>();
            MultiplicativeModifers = new List<double>();
        }
        public double OrginalValue { get; set; }
        public double CurrentValue { get; set; }
        public List<double> AdditiveModifiers { get; set; }
        public List<double> MultiplicativeModifers { get; set; }
        public void CalculateCurrentValue()
        {
            double finalValue = OrginalValue;
            foreach (double additive in AdditiveModifiers)
                finalValue += additive;
            foreach (double multiple in MultiplicativeModifers)
                finalValue *= multiple;
            CurrentValue = finalValue;
        }
    }
}


