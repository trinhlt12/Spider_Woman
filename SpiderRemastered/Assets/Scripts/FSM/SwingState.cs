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
        [SerializeField] private float _mulDirForce;
        [SerializeField] private float _velocity;
        [SerializeField] private float _distanceMaxSwing;
        [SerializeField] private float _angleRotate = 180;
        private Transform _webAttachPoint;
        private bool _webAttached;
        private LineRenderer _webLine;
        private bool isRotating = false;
        private float contrastAngle;

        public override void EnterState()
        {
            base.EnterState();
            // _webAttachPoint = new GameObject("WebAttachPoint").transform;
            // _webAttachPoint.position = _fsm.transform.position + _fsm.transform.forward * 5f + Vector3.up * 10f;
            _webAttachPoint = _blackBoard.webAttachPoint.transform;
            _blackBoard.webAttachPoint.canUpdatePos = false;
            isRotating = false;
            // ShootWeb();

            Vector3 velocity = _blackBoard.playerMovement.GetVelocity();
            _blackBoard.playerMovement.SetMovementMode(MovementMode.None);
            _blackBoard.rigidbody.useGravity = true;
            _blackBoard.rigidbody.isKinematic = false;
            _blackBoard.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            _blackBoard.rigidbody.velocity = Vector3.zero;
            _blackBoard.rigidbody.useGravity = false;

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
            _blackBoard.cameraController.enabled = true;
            _blackBoard.webAttachPoint.canUpdatePos = true;
            _webAttached = false;
            _webLine.positionCount = 0;
            _blackBoard.rigidbody.useGravity = true;_blackBoard.cameraController._cinemachineTargetYaw = _blackBoard.targetCam.eulerAngles.y;
            _blackBoard.cameraController._cinemachineTargetPitch = _blackBoard.targetCam.eulerAngles.x;


            // if (_webAttachPoint != null)
            // {
            //     Destroy(_webAttachPoint.gameObject);
            // }

            Vector3 velocity = _blackBoard.rigidbody.velocity.projectedOnPlane(Vector3.up);
            _fsm.transform.DORotate(Quaternion.
                LookRotation(Camera.main.transform.forward.projectedOnPlane(Vector3.up), 
                    Vector3.up).eulerAngles, 0.2f);
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

            // if (distanceToAttachPoint > _webSettings.maxWebLength)
            // {
            //     Debug.Log("check");
            //     // Clamp the position to maintain the rope length
            //     Vector3 clampedPosition = _webAttachPoint.position - webDirection * _webSettings.maxWebLength;
            //     _fsm.transform.position = Vector3.Lerp(_fsm.transform.position,clampedPosition, Time.deltaTime * 5);
            // }

            // Calculate the swing force considering gravity// swingForce += Vector3.up * _blackBoard.rigidbody.velocity.y; // Adjusting the vertical force component to fully counteract gravity
            // Vector3 moveDir = _blackBoard.moveDirection;
            // swingForce += moveDir * _mulDirForce;

            // Apply tension force
            // float tension = _webSettings.tensionForce * (distanceToAttachPoint / _webSettings.maxWebLength);
            // swingForce += webDirection * tension;
            Vector3 swingForce = webDirection * Mathf.Pow(_velocity, 2) /distanceToAttachPoint;
            
            swingForce = Vector3.ClampMagnitude(swingForce, _webSettings.maxSwingSpeed);
            
            Vector3 dir = Camera.main.transform.forward;
            dir = Vector3.ProjectOnPlane(dir, webDirection);
            _blackBoard.rigidbody.velocity = dir * _velocity;
            _blackBoard.rigidbody.AddForce(swingForce);
            if (!isRotating && (_webAttachPoint.position.y - _fsm.transform.position.y)/distanceToAttachPoint <= _distanceMaxSwing)
            {
                isRotating = true;
                _blackBoard.cameraController.enabled = false;
                contrastAngle = _blackBoard.targetCam.eulerAngles.y + _angleRotate;
                _blackBoard.targetCam
                    .DORotate(
                        new Vector3(_blackBoard.targetCam.eulerAngles.x, contrastAngle,
                            _blackBoard.targetCam.eulerAngles.z), 1f)
                    .OnComplete(() =>
                    {
                        isRotating = false;
                        _blackBoard.cameraController._cinemachineTargetYaw = _blackBoard.targetCam.eulerAngles.y;
                        _blackBoard.cameraController._cinemachineTargetPitch = _blackBoard.targetCam.eulerAngles.x;
                        _blackBoard.cameraController.enabled = true;
                    });
            }
            //
            // if (isRotating)
            // {
            //     _blackBoard.targetCam.eulerAngles = Vector3.Lerp(_blackBoard.targetCam.eulerAngles, new Vector3(_blackBoard.targetCam.eulerAngles.x, contrastAngle,_blackBoard.targetCam.eulerAngles.z) ,
            //         Time.deltaTime * 0.5f);
            //     if (Mathf.Abs(_blackBoard.targetCam.eulerAngles.y-contrastAngle)<= 0.01f) 
            //         isRotating = false;
            // }
            _webLine.SetPosition(0, _fsm.transform.position);
            _webLine.SetPosition(1, _webAttachPoint.position);
        }

        private void ReleaseWeb()
        {
            _webAttached = false;
            Vector3 releaseVelocity = _blackBoard.rigidbody.velocity + _fsm.transform.forward * _webSettings.releaseBoost;
            // _blackBoard.rigidbody.velocity = releaseVelocity;
        }
    }
}
