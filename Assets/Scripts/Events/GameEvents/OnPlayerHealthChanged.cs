namespace MyRPGGame.Events
{
    public class OnPlayerHealthChanged : IGameEvent
    {
        public double CurrentHealth { get; set; }
        public OnPlayerHealthChanged(double _currentHealth)
        {
            CurrentHealth = _currentHealth;
        }
    }
}
