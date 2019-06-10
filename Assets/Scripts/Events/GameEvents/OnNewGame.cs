namespace MyRPGGame.Events
{
    class OnNewGame : IGameEvent
    {
        public string CharacterName { get; set;}
        public OnNewGame(string _characterName)
        {
            CharacterName = _characterName;
        }
    }
}
