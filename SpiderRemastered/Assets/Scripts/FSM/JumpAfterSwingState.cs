/*using Animancer;
using EasyCharacterMovement;
using UnityEngine;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/JumpAfterSwingState")]
    public class JumpAfterSwingState : StateBase
    {
        [SerializeField] private IdleState _idleState;
        [SerializeField] private ClipTransition _jumpAfterSwingAnimation;
        private AnimancerState _animState;

        public override void EnterState()
        {
            base.EnterState();
            
            // Clear out any movements from the previous state
            _blackBoard.rigidbody.velocity = Vector3.zero;
            _blackBoard.playerMovement.SetMovementMode(MovementMode.None);

            // Play the Jump After Swing Animation
            if(_jumpAfterSwingAnimation.Clip != null)
            {
                _animState = _blackBoard.animancer.Play(_jumpAfterSwingAnimation);
            }
        }

        public override StateStatus UpdateState()
        {
            // Check if the jump after swing animation has reached 90% completion
            if (_animState != null && _animState.NormalizedTime > 0.9f)
            {
                _fsm.ChangeState(_idleState);
                return StateStatus.Success;
            }

            return StateStatus.Running;
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}*/