using EasyCharacterMovement;
using SFRemastered.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFRemastered
{
    public class PlayerMovement : Character
    {
        public float rootmotionSpeedMult = 1;
        public BlackBoard BlackBoard;
        protected override void HandleInput(){}

        protected override Vector3 CalcDesiredVelocity()
        {
            // Current movement direction

            Vector3 movementDirection = GetMovementDirection();

            // The desired velocity from animation (if using root motion) or from input movement vector

            Vector3 desiredVelocity = useRootMotion && rootMotionController
                ? rootMotionController.ConsumeRootMotionVelocity(deltaTime) * rootmotionSpeedMult
                : movementDirection * GetMaxSpeed();

            // Return desired velocity (constrained to constraint plane if any)

            return characterMovement.ConstrainVectorToPlane(desiredVelocity);
        }

        public void SetWallRunning(bool isWallRunning, Vector3 wallRunDirection, float wallRunSpeed)
        {
            BlackBoard.isWallRunning = isWallRunning;
            BlackBoard.wallRunDirection = wallRunDirection;
            BlackBoard.wallRunSpeed = wallRunSpeed;
        }

        public bool CanWallRun()
        {
            return BlackBoard.isWallRunning;
        }
        public void MoveAlongWall()
        {
            if (BlackBoard.isWallRunning)
            {
                //Debug.Log("Is wall running");
                Vector3 wallRunVelocity = (BlackBoard.wallRunDirection + BlackBoard.moveDirection) * BlackBoard.wallRunSpeed / 2;
               // Debug.Log("Wall run velocity: " + wallRunVelocity);
                characterMovement.velocity = wallRunVelocity;
               // Debug.Log("characterMovement.velocity = " + characterMovement.velocity);
            }
        }
    }
}