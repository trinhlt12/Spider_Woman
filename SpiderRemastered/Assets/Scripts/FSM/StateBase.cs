using Animancer;
using SFRemastered.InputSystem;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace SFRemastered
{
    public enum StateStatus
    {
        None,
        Success,
        Failure,
        Running
    }

    public abstract class StateBase : ScriptableObject
    {
        protected FSM _fsm;
        protected BlackBoard _blackBoard;
        protected bool _isAIControlled;
        public bool canTransitionToSelf = false;
        public float elapsedTime { get; private set; }
        [SerializeField] protected ClipTransition[] _mainAnimations;
        protected AnimancerState _state;
        protected WallDetection _wallDetection;
        protected ZipPointerDetector _zipPointerDetector;
        [SerializeField] protected SwingState _swingState;
        [SerializeField] protected WallRunIdleState _wallRunIdleState;
        [SerializeField] protected WebZipState _webZipState;

        public virtual void InitState(FSM fsm, BlackBoard blackBoard, bool isAIControlled)
        {
            _fsm = fsm;
            _blackBoard = blackBoard;
            _isAIControlled = isAIControlled;
            _wallDetection = _fsm.GetComponent<WallDetection>();
            elapsedTime = 0;
        }

        public virtual void EnterState() 
        {
            Debug.Log("Entering State: " + this);
            elapsedTime = 0;

            if(_mainAnimations != null && _mainAnimations.Length > 0)
            {
                int randomIndex = Random.Range(0, _mainAnimations.Length);
                ClipTransition randomClipTransition = _mainAnimations[randomIndex];

                if(randomClipTransition.Clip != null)
                    _state = _blackBoard.animancer.Play(randomClipTransition);
            }
        }

        public virtual void ConsistentUpdateState() 
        {
            elapsedTime += Time.deltaTime;
        }

        public virtual StateStatus UpdateState()
        {
            if (InputManager.instance.swing.Down)
            {
                return HandleSwingInput();
            }
            
            if (InputManager.instance.zip.Down)
            {
                return HandleZipInput();
            }

            if (_wallDetection != null && _wallDetection.IsWallDetected())
            {
                _fsm.ChangeState(_wallRunIdleState);
            }else if (_wallDetection == null)
            {
                Debug.LogError("wall detection is null");
            }
            
            return StateStatus.Running;
        }

        protected virtual StateStatus HandleZipInput()
        {
            if (_webZipState != null && _blackBoard.zipPointDetected)
            {
                _fsm.ChangeState(_webZipState);
                return StateStatus.Success;
            }
            return StateStatus.Running;
        }

        public virtual void FixedUpdateState() { }
        public virtual void ExitState() { }

        protected virtual StateStatus HandleSwingInput()
        {
            if (_swingState != null)
            {
                _fsm.ChangeState(_swingState);
                return StateStatus.Success;
            }
            return StateStatus.Running;
        }
        protected void print(string msg)
        {
            Debug.Log(msg);
        }
    }
}