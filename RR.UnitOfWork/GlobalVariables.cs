namespace RR.UnitOfWork;

public static class GlobalVariables
{
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
