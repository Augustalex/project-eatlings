using System;
using UnityEngine;

namespace eatlings
{
    [CreateAssetMenu(fileName = "EatlingBounceSettings", menuName = "EatlingBounceSettings", order = 0)]
    public class EatlingBounceSettings : ScriptableObject
    {
        public AnimationCurve impactSqueeze;
        
        public float animationDuration = 1f;
    }
}