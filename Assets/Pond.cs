using UnityEngine;

public class Pond : MonoBehaviour
{
    public void TryApplyItem(GameObject item)
    {
        var waterCan = item.GetComponent<WateringCan>();
        if (waterCan)
        {
            waterCan.Refill();
        }
    }
}
