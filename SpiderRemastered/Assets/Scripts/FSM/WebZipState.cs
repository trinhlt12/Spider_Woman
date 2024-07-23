using UnityEngine;
using DG.Tweening;
using EasyCharacterMovement;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/WebZip")]
    public class WebZipState : StateBase
    {
        [SerializeField] private IdleState _idleState;
        private Tweener _zipTweener;
        //private Quaternion _intialRotation;

        public override void EnterState()
        {
            base.EnterState();
            // Turn off Easy Character Movement if necessary
            _blackBoard.playerMovement.SetMovementMode(MovementMode.None);
            _blackBoard.rigidbody.isKinematic = false;
            _blackBoard.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            _blackBoard.rigidbody.velocity = Vector3.zero;
            //_intialRotation = _blackBoard.transform.rotation;
            StartZip();
            _blackBoard.isZipping = true;
        }

        public override StateStatus UpdateState()
        {
            return StateStatus.Running;
        }

        private void StartZip()
        {
            if (_blackBoard.zipPointDetected)
            {
                Vector3 targetPosition = _blackBoard.zipPoint; // Use the zipPoint detected in ZipPointerDetector

                // Calculate the correct position to ensure the bottom of the player's collider touches the surface of the ledge
                float colliderBottomOffset = _blackBoard.playerCollider.height / 2f;
                targetPosition.y += colliderBottomOffset;

                // Calculate duration based on distance and desired speed
                float distance = Vector3.Distance(_blackBoard.transform.position, targetPosition);
                float duration = distance / _blackBoard.zipSpeed;
                
                // Rotate player's face direction
                Vector3 direction = (targetPosition - _blackBoard.transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                targetRotation.x = 0; // Maintain upright rotation
                targetRotation.z = 0; // Maintain upright rotation
                
                _blackBoard.transform.DORotateQuaternion(targetRotation, duration);
                
                _zipTweener = _blackBoard.transform.DOMove(targetPosition, duration)
                    .SetEase(Ease.Linear)
                    .OnComplete(OnZipComplete);
            }
        }

        private void OnZipComplete()
        {
            _blackBoard.isZipping = false;
            _fsm.ChangeState(_idleState);
        }

        public override void ExitState()
        {
            base.ExitState();
            _blackBoard.playerMovement.SetMovementMode(MovementMode.Walking);
            _blackBoard.rigidbody.isKinematic = true;
            _blackBoard.rigidbody.constraints = RigidbodyConstraints.None;
            _blackBoard.isZipping = false;
           // _blackBoard.transform.rotation = _intialRotation;
            // Kill the tweener if it's still running
            if (_zipTweener != null && _zipTweener.IsActive())
            {
                _zipTweener.Kill();
            }
        }
    }
}
