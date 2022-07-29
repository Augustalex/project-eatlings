using UnityEngine;

public class Pond : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
    }

    public void TryApplyItem(GameObject item)
    {
        var waterCan = item.GetComponent<WateringCan>();
        if (waterCan)
        {
            waterCan.Refill();
        }
    }
}
