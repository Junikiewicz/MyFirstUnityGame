namespace MyRPGGame.Events
{
    public class OnPlayerLvlChanged : IGameEvent
    {
        public double CurrentLvl { get; set; }

        public OnPlayerLvlChanged(double _currentLvl)
        {
            CurrentLvl = _currentLvl;
        }
    }
}
