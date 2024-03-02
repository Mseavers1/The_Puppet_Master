namespace Source.Game
{
    public interface IGameEvent
    {
        public int GetEventType();
        public NormalGameEvents GetNormalGameEvents();
        public LootGameEvents GetLootGameEvents();

    }
}
