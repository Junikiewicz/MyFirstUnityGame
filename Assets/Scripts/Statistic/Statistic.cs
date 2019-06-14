using System.Collections.Generic;

namespace MyRPGGame.Statistics
{
    public class Statistic
    {
        public Statistic(Stat identifier,double startingValue)
        {
            Identifier = identifier;
            OrginalValue = CurrentValue = startingValue;
            AdditiveModifiers = new List<double>();
            MultiplicativeModifers = new List<double>();
        }
        public Stat Identifier { get; set; }
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


