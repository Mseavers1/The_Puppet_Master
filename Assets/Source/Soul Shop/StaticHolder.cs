public static class StaticHolder
{
    public static Stats PlayerStats { get; private set; }

    public static void StartOfGame()
    {
        PlayerStats = new Stats(10, 10, 10, 12, 10, 10, 10, 10, 10, 10);
    }
}
