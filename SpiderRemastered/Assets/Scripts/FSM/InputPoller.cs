using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyCharacterMovement;
using SFRemastered.InputSystem;

namespace SFRemastered
{
    public class InputPoller : MonoBehaviour
    {
        public BlackBoard blackBoard;
        public PlayerMovement playerMovement;

        private void Update()
        {
            // Poll movement Input

            Vector2 movementInput = new Vector2
            {
                x = InputManager.instance.move.x,
                y = InputManager.instance.move.y
            };

            // Add movement input in world space

            Vector3 movementDirection = Vector3.zero;

            movementDirection += Vector3.right * movementInput.x;
            movementDirection += Vector3.forward * movementInput.y;

            // If Camera is assigned, add input movement relative to camera look direction

            if (blackBoard.camera != null)
            {
                movementDirection = movementDirection.relativeTo(blackBoard.camera.transform);
            }

            blackBoard.moveDirection = movementDirection;
            blackBoard.jump = InputManager.instance.jump.Pressing;
            blackBoard.sprint = InputManager.instance.sprint.Pressing;
            blackBoard.isGrounded = playerMovement.IsGrounded();
        }
    }
}
