namespace MyRPGGame.Events
{
    public class OnPlayerRequiredExperimenceChanged : IGameEvent
    {
        public double RequiredExperimence { get; set; }
        public OnPlayerRequiredExperimenceChanged(double _requiredExperimence)
        {
            RequiredExperimence = _requiredExperimence;
        }
    }
}
