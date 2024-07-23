using UnityEngine;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/Dive")]
    public class DiveState : FallState
    {
        public override void EnterState()
        {
            base.EnterState();
            IncreaseFallSpeed();
        }

        private void IncreaseFallSpeed()
        {
            _blackBoard.playerMovement.SetVelocity(_blackBoard.playerMovement.GetVelocity() + Vector3.down * _blackBoard.diveBoost);
        }

        public override StateStatus UpdateState()
        {
            return base.UpdateState();
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}