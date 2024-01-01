namespace Source.Soul_Shop
{
    public static class SoulGmSettings
    {
        public const int MAXValuePerStat = 150;
        private const int MAXStatPointsBuyable = 750;
        private static int _currentStatPointsBuyable;

        static SoulGmSettings()
        {
            _currentStatPointsBuyable = 0;
        }

        public static bool IsBuyableMaxedOut()
        {
            return _currentStatPointsBuyable >= MAXStatPointsBuyable;
        }

        public static void BuyStat()
        {
            _currentStatPointsBuyable++;
        }
        
        public static void SellStat()
        {
            _currentStatPointsBuyable--;
        }

        public static int GetCurrentStatPoints()
        {
            return _currentStatPointsBuyable;
        }
    }
}
