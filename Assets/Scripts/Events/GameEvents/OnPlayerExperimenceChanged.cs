namespace MyRPGGame.Events
{
    public class OnPlayerExperimenceChanged : IGameEvent
    {
        public double CurrentExperimence { get; set; }
        public OnPlayerExperimenceChanged(double _currentExperimence)
        {
            CurrentExperimence = _currentExperimence;
        }
    }
}
