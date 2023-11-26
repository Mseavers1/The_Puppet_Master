public static class StaticHolder
{
    public static Stats PlayerStats { get; private set; }

    public static void StartOfGame()
    {
        PlayerStats = new Stats(1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
    }
}
