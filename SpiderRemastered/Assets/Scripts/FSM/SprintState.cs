using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/Sprint")]
    public class SprintState : GroundState
    {
        [SerializeField] private WalkState _walkState;
        [SerializeField] private SprintTurn180State _turn180State;
        [SerializeField] private SprintToIdleState _sprintToIdleState;
        [SerializeField] private LinearMixerTransition _sprintingBlendTree;

        public override void EnterState()
        {
            base.EnterState();

            _blackBoard.playerMovement.Sprint();
            _state = _blackBoard.animancer.Play(_sprintingBlendTree);
        }

        public override void ExitState()
        {
            base.ExitState();
            _blackBoard.playerMovement.rotationRate = 540;
            _blackBoard.playerMovement.StopSprinting();
        }

        public override StateStatus UpdateState()
        {
            StateStatus baseStatus = base.UpdateState();
            if (baseStatus != StateStatus.Running)
            {
                return baseStatus;
            }

            if(_blackBoard.playerMovement.GetSpeed() >= 6)
            {
                _blackBoard.playerMovement.rotationRate = 270;
            }
            else
            {
                _blackBoard.playerMovement.rotationRate = 540;
            }

            ((LinearMixerState)_state).Parameter = Mathf.Lerp(((LinearMixerState)_state).Parameter, _blackBoard.playerMovement.GetSpeed(), 55 * Time.deltaTime);

            _blackBoard.playerMovement.SetMovementDirection(_blackBoard.moveDirection);

            if (_blackBoard.sprint == false)
            {
                _fsm.ChangeState(_walkState);
                return StateStatus.Success;
            }

            if(_blackBoard.moveDirection.magnitude < 0.1f)
            {
                _fsm.ChangeState(_sprintToIdleState);
                return StateStatus.Success;
            }

            if(Vector3.Angle(_fsm.transform.forward, _blackBoard.moveDirection) > 150f && elapsedTime > .2f)
            {
                _fsm.ChangeState(_turn180State);
                return StateStatus.Success;
            }

            return StateStatus.Running;
        }
    }
}
