namespace MyRPGGame.Events
{
    public class OnPlayerMaxStaminaChanged : IGameEvent
    {
        public double MaximumStamina { get; set; }
        public OnPlayerMaxStaminaChanged(double _maximumStamia)
        {
            MaximumStamina = _maximumStamia;
        }
    }
}
