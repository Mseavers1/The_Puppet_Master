using JetBrains.Annotations;

namespace Source.Game
{
    public class GameEvents
    {
        public int Id;
        public string Start;
        public string Lose;

        public SpawningMobs[] Mobs;

    }
    
    public class SpawningMobs
    {
        public int Id;
        public int LevelMin;
        public int LevelMax;
    }
}
