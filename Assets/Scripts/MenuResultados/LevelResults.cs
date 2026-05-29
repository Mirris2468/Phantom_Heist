public static class LevelResults
{
    // =========================
    // ÚLTIMO NIVEL
    // =========================
    public static float levelTime;

    public static int objectivesCollected;
    public static int totalObjectives;

    public static int extrasCollected;
    public static int totalExtras;

    public static int suspicionLevel;

    public static int moneyEarned;
    public static int totalMoney;

    // =========================
    // RUN TOTAL
    // =========================
    public static int runMoney;
    public static int runLevelsPlayed;
    public static float runTime;

    // =========================
    // RESET RUN
    // =========================
    public static void ResetRun()
    {
        runMoney = 0;
        runLevelsPlayed = 0;
        runTime = 0;
    }
}