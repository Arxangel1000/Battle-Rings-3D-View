
public static class EventSpawnIsFinished
{
    public delegate void SpawnIsFinish();

    public static event SpawnIsFinish spawnIsFinish;

    public static void OnSpawnIsFinished()
    {
        spawnIsFinish?.Invoke();
    }
}
