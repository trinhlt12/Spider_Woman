using EasyCharacterMovement;
using SFRemastered.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFRemastered
{
    public class PlayerMovement : Character
    {
        public float rootmotionSpeedMult = 0.01f;
        public BlackBoard BlackBoard;
        private bool isCameraLocked = false;
        private bool _isLockTargetState;

        protected override void HandleInput(){}

        protected override Vector3 CalcDesiredVelocity()
        {
            Vector3 movementDirection = GetMovementDirection();
            Vector3 desiredVelocity = useRootMotion && rootMotionController
                ? rootMotionController.ConsumeRootMotionVelocity(deltaTime) * rootmotionSpeedMult
                : movementDirection * GetMaxSpeed();

            return characterMovement.ConstrainVectorToPlane(desiredVelocity);
        }

        public void SetCameraLockState(bool isLocked)
        {
            isCameraLocked = isLocked;
            SetRotationMode(isLocked ? RotationMode.None : RotationMode.OrientToMovement);
        }

        protected override void CustomRotationMode()
        {
            base.CustomRotationMode();
            
        }
    }
}