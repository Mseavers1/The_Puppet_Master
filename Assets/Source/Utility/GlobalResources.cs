using System;
using UnityEngine;

namespace Source.Utility
{
    public static class GlobalResources
    {
        public static double SoulEssences { get; set; }
        
        static GlobalResources()
        {
            SoulEssences = int.MaxValue;
        }
    }
}
