namespace MyRPGGame.Events
{
    public class OnNumberOfEnemiesChanged : IGameEvent
    {
        public int numberOfEnemies;
        public OnNumberOfEnemiesChanged(int _numberOfEnemies)
        {
            numberOfEnemies = _numberOfEnemies;
        }
    }
}
