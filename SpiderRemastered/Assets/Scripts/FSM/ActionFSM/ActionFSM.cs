using UnityEngine;
using System.Collections.Generic;

namespace SFRemastered
{
    public class ActionFSM : FSM
    {
        /*[SerializeField] private ActionStateBase _actionStartingState;
        [SerializeReference] private List<ActionStateBase> _actionStates;

        private ActionStateBase _currentActionState;*/

        /*public new void InitFSM(bool isAIControlled)
        {
            base.InitFSM(isAIControlled);
            _currentActionState = _actionStartingState;
            foreach (var state in _actionStates)
            {
                state.InitState(this, _blackBoard, isAIControlled);
            }

            _currentActionState.EnterState();
        }*/

        /*public bool ChangeActionState(ActionStateBase newState, bool force = false)
        {
            if (_isAIControlled && !force)
            {
                return false;
            }

            if (newState == null)
            {
                return false;
            }

            if (_currentActionState == newState && !newState.canTransitionToSelf)
            {
                return false;
            }

            _currentActionState.ExitState();
            _currentActionState = newState;
            _currentActionState.EnterState();

            return true;
        }*/

        /*public new StateStatus OnUpdate()
        {
            StateStatus movementStatus = base.OnUpdate();
            if (movementStatus != StateStatus.Running)
            {
                return movementStatus;
            }

            _currentActionState.ConsistentUpdateState();
            return _currentActionState.UpdateState();
        }

        public new void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            _currentActionState.FixedUpdateState();
        }*/
    }
}