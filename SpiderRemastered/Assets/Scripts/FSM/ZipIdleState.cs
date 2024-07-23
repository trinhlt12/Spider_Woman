using SFRemastered.InputSystem;
using UnityEngine;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/ZipIdle")]
    public class ZipIdleState : GroundState
    {
        [SerializeField] private WalkState _walkState;
        [SerializeField] private ZipLaunchState _zipLaunchState;

        public override void EnterState()
        {
            base.EnterState();
            _blackBoard.playerMovement.SetMovementDirection(Vector3.zero);
        }

        public override StateStatus UpdateState()
        {
            StateStatus baseStatus = base.UpdateState();
            if(baseStatus != StateStatus.Running)
            {
                return baseStatus;
            }
            if(_blackBoard.moveDirection.magnitude > 0f)
            {
                _fsm.ChangeState(_walkState);
                return StateStatus.Success;
            }

            if (InputManager.instance.launch.Down)
            {
                _fsm.ChangeState(_zipLaunchState);
                return StateStatus.Success;
            }

            return StateStatus.Running;
        }
    }
}