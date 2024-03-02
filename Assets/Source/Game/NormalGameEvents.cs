using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Game
{
    public class NormalGameEvents
    {
        public int Id;
        public float Chance;
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
