using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/SprintTurn180")]
    public class SprintTurn180State : GroundState
    {
        [SerializeField] private SprintState _sprintState;
        [SerializeField] private SprintToIdleState _sprintToIdleState;
        public override void EnterState()
        {
            base.EnterState();

            _blackBoard.playerMovement.TeleportRotation(Quaternion.LookRotation(-_blackBoard.moveDirection));

            _blackBoard.playerMovement.useRootMotion = true;

            _blackBoard.playerMovement.rootmotionSpeedMult = 2f;

            _blackBoard.playerMovement.SetRotationMode(EasyCharacterMovement.RotationMode.OrientWithRootMotion);

            _state.Events.OnEnd = () =>
            {
                if (_blackBoard.moveDirection.magnitude > 0.1f)
                {
                    _fsm.ChangeState(_sprintState);
                }
                else
                {
                    _fsm.ChangeState(_sprintToIdleState);
                }
            };
        }

        public override StateStatus UpdateState()
        {
            StateStatus baseStatus = base.UpdateState();
            if (baseStatus != StateStatus.Running)
            {
                return baseStatus;
            }
            _blackBoard.playerMovement.SetMovementDirection(_blackBoard.moveDirection);
            _blackBoard.playerMovement.Sprint();
            return StateStatus.Running;

        }

        public override void ExitState()
        {
            base.ExitState();

            _blackBoard.playerMovement.useRootMotion = false;

            _blackBoard.playerMovement.rootmotionSpeedMult = 1f;

            _blackBoard.playerMovement.SetRotationMode(EasyCharacterMovement.RotationMode.OrientToMovement);
        }
    }
}
