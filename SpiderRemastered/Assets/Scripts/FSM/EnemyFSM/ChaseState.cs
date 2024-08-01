using System;
using UnityEngine;

namespace SFRemastered.EnemyFSM
{
    [CreateAssetMenu(menuName = "ScriptableObjects/EnemyStates/Chase")]
    public class ChaseState : StateBase
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