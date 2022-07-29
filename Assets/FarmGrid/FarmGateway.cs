using UnityEngine;

public class FarmGateway : MonoBehaviour
{
    private int _playersIn;

    private void OnTriggerEnter(Collider other)
    {
        var rigidbody = other.attachedRigidbody;
        if (!rigidbody) return;
        
        var farmer = rigidbody.GetComponent<FarmerMovement>();
        if (farmer)
        {
            _playersIn += 1;

            if (_playersIn == GamePlayersManager.Get().PlayerCount())
            {
                GameSceneManager.Get().ChangeSceneToForrest();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var rigidbody = other.attachedRigidbody;
        if (!rigidbody) return;
        
        var farmer = rigidbody.GetComponent<FarmerMovement>();
        if (farmer)
        {
            _playersIn -= 1;
        }
    }
}