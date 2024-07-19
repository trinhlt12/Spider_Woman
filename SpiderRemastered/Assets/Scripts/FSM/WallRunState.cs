using EasyCharacterMovement;
using SFRemastered.InputSystem;
using UnityEngine;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/WallRun")]
    public class WallRunState : StateBase
    {
        [SerializeField] private float wallRunSpeed = 5f;
        [SerializeField] private WalkState _walkState;
        [SerializeField] private JumpState _jumpState;
        [SerializeField] private FallState _fallState;

        public override void EnterState()
        {
            base.EnterState();
            Vector3 velocity = _blackBoard.playerMovement.GetVelocity();
            _blackBoard.playerMovement.SetMovementMode(MovementMode.None);
            _blackBoard.rigidbody.isKinematic = false;
            _blackBoard.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            
            _blackBoard.isWallRunning = true;
            _blackBoard.wallRunDirection = Vector3.Cross(_wallDetection.detectedWallNormal, Vector3.up);
            if (_blackBoard.moveDirection.x < 0)
            {
                _blackBoard.wallRunDirection = -_blackBoard.wallRunDirection;
            }
            _blackBoard.playerMovement.SetWallRunning(true, _blackBoard.wallRunDirection, wallRunSpeed);
            Debug.Log("Enter wall run state");
        }

        public override void ExitState()
        {
            base.ExitState();
            _blackBoard.isWallRunning = false;
            _blackBoard.playerMovement.SetWallRunning(false, Vector3.zero, 0);
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

            if (!_wallDetection.IsWallDetected() || !_blackBoard.playerMovement.CanWallRun())
            {
                _fsm.ChangeState(_fallState);
                return StateStatus.Failure;
            }

            if (_blackBoard.jump)
            {
                _fsm.ChangeState(_jumpState);
                return StateStatus.Success;
            }

            _blackBoard.playerMovement.MoveAlongWall();
            
            return StateStatus.Running;
        }
    }
}