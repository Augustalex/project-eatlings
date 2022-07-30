using UnityEngine;

namespace Farmer.Scripts
{
    [CreateAssetMenu(fileName = "FarmerSettings", menuName = "FarmerSettings", order = 0)]
    public class FarmerSettings : ScriptableObject
    {
        public float baseMovementSpeed = 20f;
        public float maxWalkSpeed = 10f;
        public float maxRunSpeed = 20f;
        public float walkSpeedMultiplier = 0.8f;
        public float runSpeedMultiplier = 0.8f;
    }
}