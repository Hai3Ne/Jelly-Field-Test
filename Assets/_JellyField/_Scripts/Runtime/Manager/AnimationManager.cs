using Core;
using Runtime.ScriptTableObject;
using UnityEngine;

namespace Runtime.Manager
{
    public class AnimationManager : ManualSingletonMono<AnimationManager>
    {
        public AnimationSetting defaultSetting;
        public AnimationSetting shakeLightOnce;
    }
}