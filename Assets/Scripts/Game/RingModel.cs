using UnityEngine;

public class RingModel : MonoBehaviour
{
    public float unitSpawnRadius;
    public float unitCurrentRadius;
    public float unitSpeed;
    public float unitDestroyRadius;
    public enum RingColor { blue, red }
    public RingColor color = RingColor.blue;
}
