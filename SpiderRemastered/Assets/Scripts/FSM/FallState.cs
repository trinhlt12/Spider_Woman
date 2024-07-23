using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/Fall")]
    public class FallState : StateBase
    {
        [SerializeField] private WalkState _walkState;
        [SerializeField] private SprintState _sprintState;
        //[SerializeField] private SwingState _swingState;
        [SerializeField] private LandToIdleState _landIdleState;
        [SerializeField] private LandToWalkState _landWalkState;
        [SerializeField] private LandToSprintState _landSprintState;
        [SerializeField] private DiveState _diveState;
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
            base.UpdateState();

            _blackBoard.playerMovement.SetMovementDirection(_blackBoard.moveDirection);

            if (_blackBoard.playerMovement.IsGrounded())
            {
                if(_blackBoard.moveDirection.magnitude < 0.3f)
                    _fsm.ChangeState(_landIdleState);
                else if(_blackBoard.sprint  && elapsedTime > .6f)
                    _fsm.ChangeState(_landSprintState);
                else if(elapsedTime > .4f)
                    _fsm.ChangeState(_landWalkState);
                else if(!_blackBoard.sprint)
                    _fsm.ChangeState(_walkState);
                else
                    _fsm.ChangeState(_sprintState);
                return StateStatus.Success;
            }
            
            if(!_blackBoard.playerMovement.IsGrounded() && _blackBoard.playerMovement.GetVelocity().y < -10f)
            {
                _fsm.ChangeState(_diveState);
                return StateStatus.Success;
            }

            return StateStatus.Running;
        }
    }
}
