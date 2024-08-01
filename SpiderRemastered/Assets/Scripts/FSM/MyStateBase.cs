using SFRemastered.InputSystem;
using UnityEngine;

namespace SFRemastered
{
    public class MyStateBase : StateBase
    {
        [SerializeField] protected SwingState _swingState;
        [SerializeField] protected WallRunIdleState _wallRunIdleState;
        [SerializeField] protected WebZipState _webZipState;
        [SerializeField] protected WallRunState _wallRunState;
        protected WallDetection _wallDetection;
        protected ZipPointerDetector _zipPointerDetector;

        public override void InitState(FSM fsm, BlackBoard blackBoard, bool isAIControlled)
        {
            base.InitState(fsm, blackBoard, isAIControlled);
            _wallDetection = _fsm.GetComponent<WallDetection>();
        }

        public override StateStatus UpdateState()
        {
            base.UpdateState();
            if (InputManager.instance.swing.Down)
            {
                return HandleSwingInput();
            }
            
            if (InputManager.instance.zip.Down)
            {
                return HandleZipInput();
            }

            if (_wallDetection != null && _wallDetection.IsWallDetected() && _blackBoard.PlayerVars.moveDirection.magnitude < .1f)
            {
                _fsm.ChangeState(_wallRunIdleState);
            }

            if (_wallDetection.IsWallDetected() && _blackBoard.PlayerVars.moveDirection.magnitude > .1f)
            {
                _fsm.ChangeState(_wallRunState);
            }
            
            return StateStatus.Running;
        }
        protected virtual StateStatus HandleZipInput()
        {
            if (_webZipState != null && _blackBoard.PlayerVars.zipPointDetected)
            {
                _fsm.ChangeState(_webZipState);
                return StateStatus.Success;
            }
            return StateStatus.Running;
        }
        
        protected virtual StateStatus HandleSwingInput()
        {
            if (_swingState != null)
            {
                _fsm.ChangeState(_swingState);
                return StateStatus.Success;
            }
            return StateStatus.Running;
        }
    }
}