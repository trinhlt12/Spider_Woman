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
            _blackBoard.rigidbody.useGravity = false;
            _blackBoard.rigidbody.isKinematic = false;
            _blackBoard.onWall = true;
            _blackBoard.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;


            Debug.Log("Enter wall run state");
        }

        public override void ExitState()
        {
            base.ExitState();
            _blackBoard.onWall = false;
            _blackBoard.playerMovement.SetMovementMode(MovementMode.Walking);
            _blackBoard.rigidbody.useGravity = true;
            _blackBoard.rigidbody.isKinematic = true;
            _blackBoard.rigidbody.constraints = RigidbodyConstraints.None;
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

            ApplyWallRunMovement();
            ApplyStickToWallForce();

            return StateStatus.Running;
        }

        private void ApplyWallRunMovement()
        {
            Vector3 wallNormal = _wallDetection.detectedWallNormal;
            
            // Calculate wall right and up vectors
            Vector3 wallRight = Vector3.Cross(Vector3.up, wallNormal).normalized;
            Vector3 wallUp = Vector3.up;

            // Get the raw input
            Vector3 inputDirection = new Vector3(_blackBoard.moveDirection.x, _blackBoard.moveDirection.y, _blackBoard.moveDirection.z);
            // Project input onto the wall plane
            Vector3 inputOnWall = Vector3.ProjectOnPlane(inputDirection, wallNormal).normalized;

            // Calculate horizontal and vertical components
            float horizontalInput = Vector3.Dot(inputOnWall, wallRight);
            float verticalInput = Vector3.Dot(inputDirection, wallUp);

            // Combine horizontal and vertical movements
            Vector3 wallRunDirection = (wallRight * horizontalInput + wallUp * verticalInput).normalized;

            // Apply velocity
            Vector3 velocity = wallRunDirection * wallRunSpeed;
            _blackBoard.rigidbody.velocity = velocity;

            // Rotate the player to face the wall
            if (wallRunDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(-wallNormal, Vector3.up);
                _blackBoard.transform.rotation = Quaternion.Slerp(_blackBoard.transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
            
        }

        private void ApplyStickToWallForce()
        {
            Vector3 stickToWallVector = -_wallDetection.detectedWallNormal * stickToWallForce;
            Debug.DrawLine(_blackBoard.playerMovement.transform.position, _blackBoard.playerMovement.transform.position + stickToWallVector*10);
            _blackBoard.rigidbody.AddForce(stickToWallVector, ForceMode.Force);
        }
    }
}
