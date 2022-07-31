using UnityEngine;

public class Pond : MonoBehaviour
{
    public void TryApplyItem(GameObject item)
    {
        if (!item) return;
        
        var waterCan = item.GetComponent<WateringCan>();
        if (waterCan)
        {
            waterCan.Refill();
        }
    }
}
