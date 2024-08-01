using System.ComponentModel;
using Animancer;
using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using UnityEngine;

namespace SFRemastered.BehaviorTree.BaseActionTask
{
    public class BaseActionTask<T> : ActionTask<T> where T : class
    {
        public BBParameter<AnimancerComponent> animancer;
        public BBParameter<AnimationClip> animationClip;

        protected override void OnExecute()
        {
            base.OnExecute();
            PlayAnimation();
        }

        protected override void OnStop()
        {
            base.OnStop();
            StopAnimation();
        }

        private void StopAnimation()
        {
            if(animancer.value != null)
            {
                animancer.value.Stop();
            }
        }

        private void PlayAnimation()
        {
            if(animancer.value != null && animationClip.value != null)
            {
                animancer.value.Play(animationClip.value);
            }
        }
    }
}