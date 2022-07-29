using UnityEngine;

public class ForrestSceneInitializer : MonoBehaviour
{
    public Transform spawnZone;
    
    void Awake()
    {
        foreach (var farmerMovement in FindObjectsOfType<FarmerMovement>())
        {
            var offset = Random.insideUnitCircle * 2f;
            var offset3d = new Vector3(offset.x, 0f, offset.y);
            var spawnPoint = spawnZone.position + offset3d;
            farmerMovement.TeleportTo(spawnPoint);
        }
    }
}
