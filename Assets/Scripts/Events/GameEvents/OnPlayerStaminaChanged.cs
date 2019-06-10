namespace MyRPGGame.Events
{
    public class OnPlayerStaminaChanged : IGameEvent
    {
        public double CurrentStamina { get; set; }
        public OnPlayerStaminaChanged(double _currentStamina)
        {
            CurrentStamina = _currentStamina;
        }
    }
}
