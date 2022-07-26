using UnityEngine;

namespace Farmer.Scripts
{
    [CreateAssetMenu(fileName = "FarmerSettings", menuName = "FarmerSettings", order = 0)]
    public class FarmerSettings : ScriptableObject
    {
        public float baseMovementSpeed = 20f;
        public float maxSpeed = 10f;
    }
}