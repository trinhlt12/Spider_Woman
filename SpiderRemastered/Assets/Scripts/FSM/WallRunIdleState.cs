using EasyCharacterMovement;
using SFRemastered.InputSystem;
using UnityEngine;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/WallRunIdle")]
    public class WallRunIdleState : StateBase
    {
        [SerializeField] private WalkState _walkState;
        [SerializeField] private JumpState _jumpState;
        [SerializeField] private FallState _fallState;

        public override void EnterState()
        {
            base.EnterState();
            _blackBoard.playerMovement.SetMovementMode(MovementMode.None);
            _blackBoard.onWall = true;
            _blackBoard.rigidbody.useGravity = true;
            _blackBoard.rigidbody.isKinematic = false;
            _blackBoard.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            _blackBoard.rigidbody.velocity = Vector3.zero;
            _blackBoard.rigidbody.useGravity = false;
        }

        public override void ExitState()
        {
            base.ExitState();
            Vector3 velocity = _blackBoard.rigidbody.velocity.projectedOnPlane(Vector3.up);
            _blackBoard.playerMovement.SetMovementMode(MovementMode.Walking);
            _blackBoard.rigidbody.useGravity = false;
            _blackBoard.rigidbody.isKinematic = true;
            _blackBoard.rigidbody.constraints = RigidbodyConstraints.None;
            _blackBoard.playerMovement.SetVelocity(velocity);

        }

        public override StateStatus UpdateState()
        {
            base.UpdateState();

            // Check if wall is still detected
            if (!_wallDetection.IsWallDetected())
            {
                _fsm.ChangeState(_fallState);
                return StateStatus.Failure;
            }

            // Check if player is jumping
            if (_blackBoard.jump)
            {
                _fsm.ChangeState(_jumpState);
                return StateStatus.Success;
            }

            // Check this condition
            if (_blackBoard.moveDirection.magnitude > 0f && _wallDetection.IsWallDetected())
            {
                Debug.Log(_blackBoard.moveDirection.magnitude);
                _fsm.ChangeState(_wallRunState);
                return StateStatus.Success;
            }

            //ApplyStickToWallForce();

            return StateStatus.Running;
        }
    }
}