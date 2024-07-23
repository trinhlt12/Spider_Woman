using NodeCanvas.BehaviourTrees;
using System.Collections;
using System.Collections.Generic;
using SFRemastered.SFRemastered;
using UnityEngine;

namespace SFRemastered
{
    public class FSM : MonoBehaviour
    {
        [SerializeField] private bool _isAIControlled = false;
        [SerializeField] private StateBase _startingState;
        [SerializeField] private StateBase _currentState;
        [SerializeField] private StateBase _previousState;
        private BlackBoard _blackBoard;

        [SerializeReference] private List<StateBase> _states;

        private void Awake()
        {
            // Ensure we have a SceneReferences component
            if (GetComponent<SceneReferences>() == null)
            {
                gameObject.AddComponent<SceneReferences>();
            }
        }

        public void InitFSM(bool isAIControlled)
        {
            _blackBoard = GetComponent<BlackBoard>();
            _currentState = _startingState;
            isAIControlled = GetComponent<BehaviourTreeOwner>() != null;
            foreach (var state in _states)
            {
                state.InitState(this, _blackBoard, isAIControlled);
            }
            _currentState.EnterState();
        }

        public bool ChangeState(StateBase newState, bool force = false)
        {
            if(_isAIControlled && !force)
            {
                return false;
            }

            if (newState == null)
            {
                return false;
            }

            if (_currentState is WebZipState && _blackBoard.isZipping)
            {
                return false;
            }

            if (_currentState == newState && !newState.canTransitionToSelf)
            {
                return false;
            }
            _currentState.ExitState();
            _previousState = _currentState;
            _currentState = newState;
            _currentState.EnterState();

            return true;
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
            InitFSM(_isAIControlled);
        }

        //update state
        private void Update()
        {
            if(!_isAIControlled)
                OnUpdate(); 
        }

        public StateStatus OnUpdate()
        {
            _currentState.ConsistentUpdateState();
            return _currentState.UpdateState();
        }

        //fixed update state
        private void FixedUpdate()
        {
            if (!_isAIControlled)
                OnFixedUpdate();
        }

        public void OnFixedUpdate()
        {
            _currentState.FixedUpdateState();
        }
    }
}
