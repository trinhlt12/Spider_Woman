using Animancer;
using UnityEngine;

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
        [SerializeField] protected ClipTransition _mainAnimation;
        protected AnimancerState _state;

        [SerializeField] protected SwingState _swingState;

        public virtual void InitState(FSM fsm, BlackBoard blackBoard, bool isAIControlled)
        {
            _fsm = fsm;
            _blackBoard = blackBoard;
            _isAIControlled = isAIControlled;
            elapsedTime = 0;
        }

        public virtual void EnterState() 
        {
            Debug.Log("Entering State: " + this);
            elapsedTime = 0;

            if(_mainAnimation.Clip != null)
                _state = _blackBoard.animancer.Play(_mainAnimation);
        }

        public virtual void ConsistentUpdateState() 
        {
            elapsedTime += Time.deltaTime;
        }

        public virtual StateStatus UpdateState()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                return HandleSwingInput();
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