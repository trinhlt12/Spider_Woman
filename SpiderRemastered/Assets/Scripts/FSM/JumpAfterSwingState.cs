using EasyCharacterMovement;
using SFRemastered.InputSystem;
using UnityEngine;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/JumpAfterSwing")]
    public class JumpAfterSwingState : StateBase
    {
        [SerializeField] private IdleState _idleState;
        [SerializeField] private FallState _fallState;
        [SerializeField] private WebSO _webSettings;
        //[SerializeField] private AnimationClip _jumpAnimation;

        public override void EnterState()
        {
            base.EnterState();
            ReleaseWeb();
            Vector3 directionToFace = Camera.main.transform.position - _blackBoard.transform.position;
            directionToFace.y = 0;
            /*// Play animation
            if (_jumpAnimation != null)
            {
                _blackBoard.animancer.Play(_jumpAnimation).Events.OnEnd = () =>
                {
                    _fsm.ChangeState(_idleState);
                };
            }*/
        }

        public override StateStatus UpdateState()
        {
            base.UpdateState();
            if(_blackBoard.playerMovement.GetVelocity().y < 0 && elapsedTime > .2f)
            {
                _fsm.ChangeState(_fallState);
                return StateStatus.Success;
            }
            return StateStatus.Running;
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        private void ReleaseWeb()
        {
            _blackBoard._webAttached = false;

            _blackBoard.rigidbody.isKinematic = false;
            //_blackBoard.rigidbody.useGravity = true;

            Vector3 upwardForce = _fsm.transform.up * _webSettings.releaseBoost;
            Vector3 forwardForce = _fsm.transform.forward * _webSettings.releaseBoost;
            Vector3 releaseVelocity = _blackBoard.rigidbody.velocity + upwardForce + forwardForce;
           // _blackBoard.rigidbody.velocity = releaseVelocity;
            _blackBoard.playerMovement.SetVelocity(releaseVelocity);
            _blackBoard.rigidbody.isKinematic = true;
        }
    }
}