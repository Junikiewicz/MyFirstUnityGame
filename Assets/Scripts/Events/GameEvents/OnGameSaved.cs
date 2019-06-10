using MyRPGGame.SaveSystem;

namespace MyRPGGame.Events
{
    public class OnGameSaved:IGameEvent
    {
        public Save saveData;
        public OnGameSaved(Save _saveData)
        {
            saveData = _saveData;
        }
    }
}
