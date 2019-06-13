namespace MyRPGGame.Events
{
    public class OnPlayerGoldChanged : IGameEvent
    {
        public double CurrentGold { get; set; }
        public OnPlayerGoldChanged(double _currentGold)
        {
            CurrentGold = _currentGold;
        }
    }
}
