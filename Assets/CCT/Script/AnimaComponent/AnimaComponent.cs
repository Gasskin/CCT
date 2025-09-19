using Animancer;
using UnityEngine;

namespace CCT.Script
{
    public class AnimaComponent: MonoBehaviour
    {
        [SerializeField]
        private AnimancerComponent _animancer;

        private AnimaParams _animaParams = new();
        
        public void PlayAnima(AnimationClip clip)
        {
            _animancer.Play(clip);
        }
    }
}