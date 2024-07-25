using System.Collections;
using SFRemastered.InputSystem;
using UnityEngine;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/Idle")]
    public class IdleState : GroundState
    {
        [SerializeField] private WalkState _walkState;
        [SerializeField] protected ComboAttackState _comboAttackState;


        public override void EnterState()
        {
            base.EnterState();
            _blackBoard.playerMovement.SetMovementDirection(Vector3.zero);
        }
        public override StateStatus UpdateState()
        {
            StateStatus baseStatus = base.UpdateState();
            if(baseStatus != StateStatus.Running)
            {
                return baseStatus;
            }

            if(_blackBoard.moveDirection.magnitude > 0f && !_blackBoard.isInWallState)
            {
                _fsm.ChangeState(_walkState);
                return StateStatus.Success;
            }
            
            if (_blackBoard.playerMovement.IsGrounded() && InputManager.instance.attack.Down)
            {
                _fsm.ChangeState(_comboAttackState);
                return StateStatus.Success;
            }

            return StateStatus.Running;
        }
    }
}