using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Game
{
    public class LootGameEvents
    {
        public int Id;
        public float Chance;
        public string Start;
        public string FileName;
        public Inside[] Inside;
    }

    public class Inside
    {
        public int Id;
        public int Sp;
        public float Chance;
        public string Talk;
    }
}
