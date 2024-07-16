using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/Walk")]
    public class WalkState : GroundState
    {
        [SerializeField] private IdleState _idleState;
        [SerializeField] private SprintState _sprintState;
        [SerializeField] private WalkToIdleState _walkToIdleState;
        [SerializeField] private LinearMixerTransition _walkingBlendTree;

        public override void EnterState()
        {
            base.EnterState();

            _state = _blackBoard.animancer.Play(_walkingBlendTree);
        }

        public override StateStatus UpdateState()
        {
            StateStatus baseStatus = base.UpdateState();
            if (baseStatus != StateStatus.Running)
            {
                return baseStatus;
            }

            ((LinearMixerState)_state).Parameter = Mathf.Lerp(((LinearMixerState)_state).Parameter, _blackBoard.playerMovement.GetSpeed(), 55 * Time.deltaTime);

            _blackBoard.playerMovement.SetMovementDirection(_blackBoard.moveDirection);

            //if (_blackBoard.moveDirection.magnitude == 0f)
            //{
            //    _fsm.ChangeState(_idleState);
            //    return StateStatus.Success;
            //}

            if(_blackBoard.sprint)
            {
                _fsm.ChangeState(_sprintState);
                return StateStatus.Success;
            }

            if (_blackBoard.moveDirection.magnitude < 0.1f)
            {
                _fsm.ChangeState(_walkToIdleState);
                return StateStatus.Success;
            }

            return StateStatus.Running;
        }
    }
}
