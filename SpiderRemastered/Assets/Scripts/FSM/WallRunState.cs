using EasyCharacterMovement;
using UnityEngine;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/WallRun")]
    public class WallRunState : StateBase
    {
        [SerializeField] private float wallRunSpeed = 10f;
        [SerializeField] private float stickToWallForce = 20f; // Increase the stick to wall force
         [SerializeField] private JumpState _jumpState;
        [SerializeField] private FallState _fallState;

        public override void EnterState()
        {
            base.EnterState();

            _blackBoard.playerMovement.SetMovementMode(MovementMode.None);
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

            if (!_wallDetection.IsWallDetected())
            {
                _fsm.ChangeState(_fallState);
                return StateStatus.Failure;
            }

            if (_blackBoard.jump)
            {
                _fsm.ChangeState(_jumpState);
                return StateStatus.Success;
            }

            /*if (_blackBoard.moveDirection.magnitude < 0.1f)
            {
                //Debug.Log(_blackBoard.moveDirection.magnitude);
                _fsm.ChangeState(_wallRunIdleState);
                return StateStatus.Success;
            }*/

            ApplyWallRunMovement();
            ApplyStickToWallForce();

            return StateStatus.Running;
        }

        private void ApplyWallRunMovement()
        {
            _blackBoard.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            Vector3 wallRunDirection = Vector3.Cross(_wallDetection.detectedWallNormal, Vector3.up).normalized;
            if (_blackBoard.moveDirection.x < 0)
            {
                wallRunDirection = -wallRunDirection;
            }

            Vector3 velocity = wallRunDirection * wallRunSpeed;
            velocity.y = _blackBoard.rigidbody.velocity.y;
            _blackBoard.rigidbody.velocity = velocity;
        }

        private void ApplyStickToWallForce()
        {
            Vector3 stickToWallVector = -_wallDetection.detectedWallNormal * stickToWallForce;
            _blackBoard.rigidbody.AddForce(stickToWallVector, ForceMode.Force);
        }
    }
}
