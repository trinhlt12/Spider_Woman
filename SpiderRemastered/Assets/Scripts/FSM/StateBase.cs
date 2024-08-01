using System.Collections.Generic;
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
        [SerializeField] protected List<ClipTransition> _mainAnimations;
        protected AnimancerState _state;
        
        public virtual void InitState(FSM fsm, BlackBoard blackBoard, bool isAIControlled)
        {
            _fsm = fsm;
            _blackBoard = blackBoard;
            _isAIControlled = isAIControlled;
            elapsedTime = 0;
        }

        public virtual void EnterState() 
        {
            elapsedTime = 0;
            {
                if(_mainAnimations is { Count: > 0 })
                {
                    int randomIndex = Random.Range(0, _mainAnimations.Count);
                    ClipTransition randomClipTransition = _mainAnimations[randomIndex];

                    if(randomClipTransition.Clip != null)
                        _state = _blackBoard.CommonVars.animancer.Play(randomClipTransition);
                }
            }
        }

        public virtual void ConsistentUpdateState() 
        {
            elapsedTime += Time.deltaTime;
        }

        public virtual StateStatus UpdateState()
        {
            
            return StateStatus.Running;
        }

        public virtual void FixedUpdateState() { }
        public virtual void ExitState() { }
        protected void print(string msg)
        {
            Debug.Log(msg);
        }
    }
}