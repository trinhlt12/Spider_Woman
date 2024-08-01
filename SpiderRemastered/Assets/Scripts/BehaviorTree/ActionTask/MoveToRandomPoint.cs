using NodeCanvas.Framework;
using UnityEngine.AI;

namespace SFRemastered
{
    public class MoveToRandomPoint : ActionTask<NavMeshAgent>
    {
        public BBParameter<float> patrolRadius = 10f;
        private NavMeshAgent _agent;

        protected override void OnExecute()
        {
            base.OnExecute();
        }
    }
}