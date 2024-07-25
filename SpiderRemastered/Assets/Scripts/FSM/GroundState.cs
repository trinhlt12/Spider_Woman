using SFRemastered.InputSystem;
using UnityEngine;

namespace SFRemastered
{
    public abstract class GroundState : StateBase
    {
        [SerializeField] protected JumpState _jumpState;
        [SerializeField] protected FallState _fallState;

        public bool canJump = true;

        public override StateStatus UpdateState()
        {
            StateStatus swingStatus = base.UpdateState();
            if (swingStatus != StateStatus.Running)
            {
                return swingStatus;
            }

            if (HandleJump())
            {
                _fsm.ChangeState(_jumpState);
                return StateStatus.Success;
            }

            if (!_blackBoard.playerMovement.IsGrounded() && _blackBoard.playerMovement.GetVelocity().y < -5)
            {
                _fsm.ChangeState(_fallState);
                return StateStatus.Success;
            }
            
            

            return StateStatus.Running;
        }

        protected virtual bool HandleJump()
        {
            if (canJump && _blackBoard.playerMovement.CanJump())
            {
                return _blackBoard.jump;
            }

            return false;
        }
    }
}