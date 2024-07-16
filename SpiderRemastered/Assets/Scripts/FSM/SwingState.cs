using DG.Tweening;
using EasyCharacterMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/Swing")]
    public class SwingState : StateBase
    {
        [SerializeField] private IdleState _idleState;
        [SerializeField] private WebSO _webSettings;
        private Transform _webAttachPoint;
        private bool _webAttached;
        private LineRenderer _webLine;

        public override void EnterState()
        {
            base.EnterState();
            _webAttachPoint = new GameObject("WebAttachPoint").transform;
            _webAttachPoint.position = _fsm.transform.position + _fsm.transform.forward * 5f + Vector3.up * 10f;

            ShootWeb();

            Vector3 velocity = _blackBoard.playerMovement.GetVelocity();
            _blackBoard.playerMovement.SetMovementMode(MovementMode.None);
            _blackBoard.rigidbody.useGravity = true;
            _blackBoard.rigidbody.isKinematic = false;
            _blackBoard.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            _blackBoard.rigidbody.velocity = velocity;

            _webAttached = true;
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

            if (_webAttached)
            {
                Swing();
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                ReleaseWeb();
                _fsm.ChangeState(_idleState);
                return StateStatus.Success;
            }

            return StateStatus.Running;
        }

        public override void ExitState()
        {
            base.ExitState();
            _webAttached = false;
            _webLine.positionCount = 0;

            if (_webAttachPoint != null)
            {
                Destroy(_webAttachPoint.gameObject);
            }

            Vector3 velocity = _blackBoard.rigidbody.velocity.projectedOnPlane(Vector3.up);
            _fsm.transform.DORotate(Quaternion.LookRotation(_fsm.transform.forward.projectedOnPlane(Vector3.up), Vector3.up).eulerAngles, 0.2f);
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

        private void ShootWeb()
        {
            _webAttachPoint.position = _fsm.transform.position + _fsm.transform.forward * 5f + Vector3.up * 10f;
        }

        private void Swing()
        {
            Vector3 webDirection = (_webAttachPoint.position - _fsm.transform.position).normalized;
            float distanceToAttachPoint = Vector3.Distance(_fsm.transform.position, _webAttachPoint.position);

            if (distanceToAttachPoint > _webSettings.maxWebLength)
            {
                // Clamp the position to maintain the rope length
                Vector3 clampedPosition = _webAttachPoint.position - webDirection * _webSettings.maxWebLength;
                _fsm.transform.position = clampedPosition;
            }

            // Calculate the swing force considering gravity
            Vector3 swingForce = webDirection * _webSettings.swingForce;
            swingForce += Vector3.up * _blackBoard.rigidbody.velocity.y; // Adjusting the vertical force component to fully counteract gravity
            swingForce = Vector3.ClampMagnitude(swingForce, _webSettings.maxSwingSpeed);

            // Apply tension force
            float tension = _webSettings.tensionForce * (distanceToAttachPoint / _webSettings.maxWebLength);
            swingForce += webDirection * tension;

            _blackBoard.rigidbody.AddForce(swingForce);

            _webLine.SetPosition(0, _fsm.transform.position);
            _webLine.SetPosition(1, _webAttachPoint.position);
        }

        private void ReleaseWeb()
        {
            _webAttached = false;
            Vector3 releaseVelocity = _blackBoard.rigidbody.velocity + _fsm.transform.forward * _webSettings.releaseBoost;
            _blackBoard.rigidbody.velocity = releaseVelocity;
        }
    }
}
