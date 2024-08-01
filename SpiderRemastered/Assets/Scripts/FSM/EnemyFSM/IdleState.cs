using UnityEngine;

namespace SFRemastered.EnemyFSM
{
    [CreateAssetMenu(menuName = "ScriptableObjects/EnemyStates/Idle")]
    public class IdleState : StateBase
    {
        public override void EnterState()
        {
            base.EnterState();
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override StateStatus UpdateState()
        {
            return base.UpdateState();
        }
    }
}