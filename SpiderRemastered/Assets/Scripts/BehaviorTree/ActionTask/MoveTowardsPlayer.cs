using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;
using Animancer;
using SFRemastered.BehaviorTree.BaseActionTask;

namespace SFRemastered
{
    public class MoveTowardsPlayer : BaseActionTask<NavMeshAgent>
    {
        public BBParameter<GameObject> player;
        public BBParameter<float> speed;
        public BBParameter<AnimationClip[]> blendTree;

        protected override void OnExecute()
        {
            base.OnExecute();
            agent.speed = speed.value;
            agent.SetDestination(player.value.transform.position);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (player.value != null)
            {
                agent.SetDestination(player.value.transform.position);
            }
        }
    }
}