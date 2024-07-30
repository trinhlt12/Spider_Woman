using Animancer;
using System.Collections;
using System.Collections.Generic;
using SFRemastered.InputSystem;
using UnityEngine;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/Jump")]
    public class JumpState : StateBase
    {
        [SerializeField] private float jumpImpulseModifier = 1f;
        [SerializeField] private WalkState _walkState;
        [SerializeField] private FallState _fallState;
        [SerializeField] private ZipAttackState _zipAttackState;
        
        [SerializeField] private ClipTransition _fallLoopAnimation;

        public override void EnterState()
        {
            base.EnterState();
            _blackBoard.playerMovement.jumpImpulse *= jumpImpulseModifier;
            _blackBoard.playerMovement.Jump();
            _state.Events.OnEnd = () =>
            {
                _state = _blackBoard.animancer.Play(_fallLoopAnimation);
            };
        }

        public override void ExitState()
        {
            base.ExitState();
            _blackBoard.playerMovement.StopJumping();
            _blackBoard.playerMovement.jumpImpulse /= jumpImpulseModifier;
        }
            

        public override StateStatus UpdateState()
        {
            base.UpdateState();

            if(elapsedTime > .1f)
                _blackBoard.playerMovement.StopJumping();

            _blackBoard.playerMovement.SetMovementDirection(_blackBoard.moveDirection);

            if(_blackBoard.playerMovement.IsGrounded() && elapsedTime > .2f)
            {
                _fsm.ChangeState(_walkState);
                return StateStatus.Success;
            }

            if(_blackBoard.playerMovement.GetVelocity().y < -5 && elapsedTime > .2f)
            {
                _fsm.ChangeState(_fallState);
                return StateStatus.Success;
            }

            if (InputManager.instance.attack.Down)
            {
                _fsm.ChangeState(_zipAttackState);
            }

            return StateStatus.Running;
        }
    }
}


