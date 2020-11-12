public static class EventUnitsTeamIsGone
{
    public delegate void TeamIsGone();

    public static event TeamIsGone teamIsGone;

    public static void OnUnitsTeamIsGone()
    {
        teamIsGone?.Invoke();
    }
}