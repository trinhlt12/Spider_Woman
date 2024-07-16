using NodeCanvas.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/LandToSprint")]
    public class LandToSprintState : GroundState
    {
        [SerializeField] private SprintState _sprintState;
        [SerializeField] private SprintToIdleState _idleState;
        public override void EnterState()
        {
            base.EnterState();

            _state.Events.OnEnd = () =>
            {
                _fsm.ChangeState(_sprintState);
            };

            _blackBoard.playerMovement.Sprint();
        }

        public override StateStatus UpdateState()
        {
            StateStatus baseStatus = base.UpdateState();
            if (baseStatus != StateStatus.Running)
            {
                return baseStatus;
            }

            if (_blackBoard.moveDirection.magnitude > 0.1f)
                _blackBoard.playerMovement.SetMovementDirection(_blackBoard.moveDirection);
            else
                _blackBoard.playerMovement.SetMovementDirection(_fsm.transform.forward);

            return StateStatus.Running;
        }

        public override void ExitState()
        {
            base.ExitState();

            _blackBoard.playerMovement.StopSprinting();
        }

        public void FinishRoll()
        {
            if(_blackBoard.moveDirection.magnitude < 0.1f)
            {
                _fsm.ChangeState(_idleState);
            }
        }
    }
}
