using DG.Tweening;
using EasyCharacterMovement;
using SFRemastered.InputSystem;
using UnityEngine;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/Swing")]
    public class SwingState : StateBase
    {
        [SerializeField] private IdleState _idleState;
        [SerializeField] private JumpAfterSwingState _jumpAfterSwingState;
        [SerializeField] private WebSO _webSettings;
        [SerializeField] private float _mulDirForce;
        [SerializeField] private float _velocity;
        [SerializeField] private float _distanceMaxSwing;
        [SerializeField] private float _angleRotate = 180;
        private Transform _webAttachPoint;
        private LineRenderer _webLine;
        private bool isRotating = false;

        public override void EnterState()
        {
            base.EnterState();
            _webAttachPoint = _blackBoard.webAttachPoint.transform;
            _blackBoard.webAttachPoint.canUpdatePos = false;

            Vector3 velocity = _blackBoard.playerMovement.GetVelocity();
            _blackBoard.playerMovement.SetMovementMode(MovementMode.None);
            _blackBoard.rigidbody.useGravity = true;
            _blackBoard.rigidbody.isKinematic = false;
            _blackBoard.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            _blackBoard.rigidbody.velocity = Vector3.zero;
            _blackBoard.rigidbody.useGravity = false;

            _blackBoard._webAttached = true;
            _webLine = _fsm.GetComponent<LineRenderer>();
            _webLine.positionCount = 2;
        }

        public override StateStatus UpdateState()
        {
            if (GroundCheck())
            {
                _fsm.ChangeState(_idleState);
                return StateStatus.Success;
            }

            if (_blackBoard._webAttached)
            {
                Swing();
            }

            if (InputManager.instance.swing.Up)
            {
                _fsm.ChangeState(_idleState);
                return StateStatus.Success;
            }

            return StateStatus.Running;
        }

        public override void ExitState()
        {
            base.ExitState();
            _blackBoard.cameraController.enabled = true;
            _blackBoard.webAttachPoint.canUpdatePos = true;
            _blackBoard._webAttached = false;
            _webLine.positionCount = 0;

            Vector3 velocity = _blackBoard.rigidbody.velocity.projectedOnPlane(Vector3.up);
            _fsm.transform.DORotate(Quaternion.LookRotation(Camera.main.transform.forward.projectedOnPlane(Vector3.up), Vector3.up).eulerAngles, 0.2f);
            _blackBoard.playerMovement.SetMovementMode(MovementMode.Walking);
            _blackBoard.rigidbody.useGravity = false;
            _blackBoard.rigidbody.isKinematic = true;
            _blackBoard.rigidbody.constraints = RigidbodyConstraints.None;
            _blackBoard.playerMovement.SetVelocity(velocity);
        }

        private bool GroundCheck()
        {
            return Physics.Raycast(_fsm.transform.position, Vector3.down, 0.3f, _blackBoard.groundLayers);
        }

        private void Swing()
        {
            Vector3 webDirection = (_webAttachPoint.position - _fsm.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(webDirection);
            float distanceToAttachPoint = Vector3.Distance(_fsm.transform.position, _webAttachPoint.position);

            Vector3 swingForce = webDirection * Mathf.Pow(_velocity, 2) / distanceToAttachPoint;
            swingForce = Vector3.ClampMagnitude(swingForce, _webSettings.maxSwingSpeed);
            
            Vector3 dir = Camera.main.transform.forward;
            dir = Vector3.ProjectOnPlane(dir, webDirection);
            _blackBoard.rigidbody.velocity = dir * _velocity;
            _blackBoard.rigidbody.AddForce(swingForce);

            _webLine.SetPosition(0, _blackBoard.shootPosition.position);
            _webLine.SetPosition(1, _webAttachPoint.position);
        }
    }
}
