using UnityEngine;

namespace Runtime.ScriptTableObject
{
    [CreateAssetMenu(fileName = "New Setting", menuName = "Jelly/AnimationSetting", order = 1)]
    public class AnimationSetting : ScriptableObject
    {
        public float upDownFactor = 0.1f;
        public float upDownSpeed = 6f; 

        public float leftFactor = 1f;
        public float leftSpeed = 5f;
        public float leftOffset = 1f;

        public float strectFactor = -0.1f;
        public float strectSpeed = 2f;
    }
}
