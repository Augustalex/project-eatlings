using System;
using UnityEngine;

namespace eatlings
{
    [CreateAssetMenu(fileName = "EatlingBounceSettings", menuName = "EatlingBounceSettings", order = 0)]
    public class EatlingBounceSettings : ScriptableObject
    {
        // Original state:
        // public float[] squishSequence = new[] {.2f, 1.5f, .5f, 1f, .8f, 1f};
        // public float[] squishSequenceLengths = new[] {.2f, .15f, .1f, .05f, .02f, .01f};
        
        public AnimationCurve jumpSqueeze;
        public AnimationCurve impactSqueeze;
        
        public float timeScale = 1f;
        
        public Step[] animation = new[]
        {
            new Step {
                    Squeeze = .2f,
                    Duration = .2f
            },
            new Step {
                Squeeze = 1.5f,
                Duration = .15f
            },
            new Step {
                Squeeze = .5f,
                Duration = .1f
            },
            new Step {
                Squeeze = 1f,
                Duration = .05f
            },
            new Step {
                Squeeze = .8f,
                Duration = .02f
            },
            new Step {
                Squeeze = 1f,
                Duration = .01f
            }
        };

        [Serializable]
        public struct Step
        {
            public float Squeeze;
            public float Duration;
        }
    }
}