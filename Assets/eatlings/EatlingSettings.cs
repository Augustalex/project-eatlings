using UnityEngine;

[CreateAssetMenu(fileName = "EatlingSettings", menuName = "EatlingSettings", order = 0)]
public class EatlingSettings : ScriptableObject
{
    public float maxWater = 30f;
    public float waterConsumptionPerSecond = 1f;
    public float maxDryTimeBeforeDeath = 30f;
    
    public float timeUntilTeen = 10f;
    public float timeUntilFullyGrown = 30f;
}