namespace RR.UnitOfWork;

public static class GlobalVariables
{
    private static bool RuningTests;
    public static void SetRunningTests(bool isRunning)
    {
        RuningTests = isRunning;
    }
    public static bool AreTestsRunning()
    {
        return RuningTests;
    }

    private static int UserId;

    public static void SetUserId(int id)
    { 
        UserId = id;
    }
    public static int GetUserId()
    {
        return UserId;
    }
}
