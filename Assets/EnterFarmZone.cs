using UnityEngine;

public class EnterFarmZone : MonoBehaviour
{
    private int _playersIn;

    private void OnTriggerEnter(Collider other)
    {
        var rb = other.attachedRigidbody;
        if (!rb) return;

        var farmer = rb.GetComponent<FarmerMovement>();
        if (farmer)
        {
            _playersIn += 1;

            if (_playersIn == GamePlayersManager.Get().PlayerCount())
            {
                GameSceneManager.Get().ChangeSceneToFarm();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var rb = other.attachedRigidbody;
        if (!rb) return;

        var farmer = rb.GetComponent<FarmerMovement>();
        if (farmer)
        {
            _playersIn -= 1;
        }
    }
}