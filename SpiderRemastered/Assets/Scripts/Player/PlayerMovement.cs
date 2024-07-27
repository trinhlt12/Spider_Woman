using EasyCharacterMovement;
using SFRemastered.InputSystem;
using UnityEngine;

namespace SFRemastered
{
    public class PlayerMovement : Character
    {
        public float rootmotionSpeedMult = 0.01f;
        public BlackBoard BlackBoard;
        private bool _isLockTargetState;
        [SerializeField] private float lockOnRotationSpeed = 10f;

        protected override void HandleInput() { }

        protected override Vector3 CalcDesiredVelocity()
        {
            Vector3 movementDirection = GetMovementDirection();
            Vector3 desiredVelocity = useRootMotion && rootMotionController
                ? rootMotionController.ConsumeRootMotionVelocity(deltaTime) * rootmotionSpeedMult
                : movementDirection * GetMaxSpeed();

            return characterMovement.ConstrainVectorToPlane(desiredVelocity);
        }

        public void SetLockOnTarget(bool isLocked)
        {
            _isLockTargetState = isLocked;
            SetRotationMode(isLocked ? RotationMode.Custom : RotationMode.OrientToMovement);
        }

        protected override void CustomRotationMode()
        {
            if (_isLockTargetState && BlackBoard.lockedTarget != null)
            {
                Vector3 directionToTarget = (BlackBoard.lockedTarget.position - transform.position).normalized;
                directionToTarget.y = 0; // Keep the rotation level

                if (directionToTarget != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnRotationSpeed * Time.deltaTime);
                }
            }
        }

        // Override UpdateRotation to use our custom logic
        protected override void UpdateRotation()
        {
            if (GetRotationMode() == RotationMode.Custom)
            {
                CustomRotationMode();
            }
            else
            {
                base.UpdateRotation();
            }
        }
    }
}