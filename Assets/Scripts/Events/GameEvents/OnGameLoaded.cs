using MyRPGGame.SaveSystem;

namespace MyRPGGame.Events
{
    public class OnGameLoaded: IGameEvent
    {
        public Save saveData;
        public OnGameLoaded(Save _saveData)
        {
            saveData = _saveData;
        }
    }
}
