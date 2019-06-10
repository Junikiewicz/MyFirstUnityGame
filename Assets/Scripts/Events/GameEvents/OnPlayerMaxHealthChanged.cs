namespace MyRPGGame.Events
{
    public class OnPlayerMaxHealthChanged : IGameEvent
    {
        public double MaxiumHealth { get; set; }
        public OnPlayerMaxHealthChanged(double _maximumHealth)
        {
            MaxiumHealth = _maximumHealth;
        }
    }
}
