using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

namespace SFRemastered
{
    public class MoveTowardsPlayerAction : ActionTask<NavMeshAgent>
    {
        public BBParameter<GameObject> player;
        public BBParameter<float> speed;

        protected override void OnExecute()
        {
            base.OnExecute();
            agent.speed = speed.value;
            agent.SetDestination(player.value.transform.position);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            
            if(Vector3.Distance(agent.transform.position, player.value.transform.position) <= 1.5f)
            {
                EndAction(true);
            }
        }
    }
}