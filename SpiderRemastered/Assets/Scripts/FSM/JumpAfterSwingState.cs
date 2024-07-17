using EasyCharacterMovement;
using SFRemastered.InputSystem;
using UnityEngine;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/JumpAfterSwing")]
    public class JumpAfterSwingState : StateBase
    {
        [SerializeField] private IdleState _idleState;
        [SerializeField] private WebSO _webSettings;
        [SerializeField] private AnimationClip _jumpAnimation;

        public override void EnterState()
        {
            base.EnterState();
            ReleaseWeb();

            // Play animation
            if (_jumpAnimation != null)
            {
                _blackBoard.animancer.Play(_jumpAnimation).Events.OnEnd = () =>
                {
                    _fsm.ChangeState(_idleState);
                };
            }
        }

        public override StateStatus UpdateState()
        {
            return StateStatus.Running;
        }

        private void ReleaseWeb()
        {
            _blackBoard._webAttached = false;

            _blackBoard.rigidbody.isKinematic = false;
            _blackBoard.rigidbody.useGravity = true;

            Vector3 releaseVelocity = _blackBoard.rigidbody.velocity + _fsm.transform.up * _webSettings.releaseBoost;
            _blackBoard.rigidbody.velocity = releaseVelocity;
            _blackBoard.rigidbody.isKinematic = true;
        }
    }
}