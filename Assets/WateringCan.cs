using UnityEngine;

public class WateringCan : MonoBehaviour
{
    // Public

    public float maxWater = 100f;

    // Private

    private float _waterLevel;

    // Public

    public void Water(GameObject target)
    {
        var babyGrowth = target.GetComponentInChildren<EatlingBabyGrowth>();
        if (!babyGrowth)
        {
            Debug.Log("ERROR: No baby growth class on targeted eatling");
            return;
        }

        var waterTaken = TakeWater();
        Debug.Log("WATER TAKEN: " + waterTaken);
        babyGrowth.Water(waterTaken);
    }

    public void Refill()
    {
        Debug.Log("REFILL!");
        _waterLevel = maxWater;
    }

    // Private
    private float TakeWater()
    {
        var take = Mathf.Min(_waterLevel, 10f);
        _waterLevel -= take;

        return take;
    }
}