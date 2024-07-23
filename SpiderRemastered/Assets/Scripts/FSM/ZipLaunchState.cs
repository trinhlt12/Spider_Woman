using DG.Tweening;
using EasyCharacterMovement;
using UnityEngine;
using UnityEngine.WSA;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/ZipLaunch")]
    public class ZipLaunchState : StateBase
    {
        [SerializeField] private FallState _fallState;
        private Tweener _launchTweener;
        public override void EnterState()
        {
            base.EnterState();
            Launch();
            _blackBoard.playerMovement.SetVelocity(Vector3.zero);
            Vector3 directionToFace = Camera.main.transform.position - _blackBoard.transform.position;
            directionToFace.y = 0;
            //_blackBoard.transform.rotation = Quaternion.LookRotation(directionToFace);

        }

        private void Launch()
        {
            _blackBoard.playerMovement.SetMovementMode(MovementMode.None);
            _blackBoard.rigidbody.isKinematic = false;

            // Correctly calculate the upward and forward forces
            Vector3 startPosition = _blackBoard.transform.position;
            Vector3 upwardForce = Vector3.up * _blackBoard.launchBoost;
            Vector3 forwardForce = _blackBoard.transform.forward * _blackBoard.launchBoost;

            Vector3 peakPosition = startPosition + forwardForce + upwardForce;
            Vector3 endPosition = startPosition + forwardForce * 2;

            // Define the path as a parabolic trajectory
            Vector3[] path = new Vector3[] { startPosition, peakPosition, endPosition };

            // Use DOTween to animate the player's position along the path
            _launchTweener = _blackBoard.transform.DOPath(path, 3f, PathType.CatmullRom)
                .SetEase(Ease.OutQuad).OnComplete(OnLaunchComplete);
        }

        private void OnLaunchComplete()
        {
            _blackBoard.playerMovement.SetMovementMode(MovementMode.Walking);
            _fsm.ChangeState(_fallState);
        }

        public override void ExitState()
        {
            base.ExitState();
            // Ensure any tweens are killed to avoid unintended behavior
            if (_launchTweener != null && _launchTweener.IsActive())
            {
                _launchTweener.Kill();
            }

            // Reset velocity to avoid unintended behavior
            _blackBoard.playerMovement.SetVelocity(Vector3.zero);
            _blackBoard.playerMovement.SetMovementMode(MovementMode.Walking);
        }
    }
}