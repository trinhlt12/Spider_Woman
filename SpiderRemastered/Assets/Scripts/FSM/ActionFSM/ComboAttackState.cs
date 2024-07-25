using SFRemastered.InputSystem;
using UnityEngine;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ActionStates/ComboAttackState")]
    public class ComboAttackState : ActionStateBase
    {
        [SerializeField] private ComboConfig _comboConfig;
        [SerializeField] private ActionIdleState _actionIdleState;

        private ComboSystem _comboSystem;

        public override void InitState(FSM fsm, BlackBoard blackBoard, bool isAIControlled)
        {
            base.InitState(fsm, blackBoard, isAIControlled);
            _comboSystem = new ComboSystem(_comboConfig);
        }

        public override void EnterState()
        {
            base.EnterState();
            _comboSystem.StartCombo();
            ExecuteAttack();
        }

        public override StateStatus UpdateState()
        {
            StateStatus baseStatus = base.UpdateState();
            if (baseStatus != StateStatus.Running)
            {
                return baseStatus;
            }

            if (InputManager.instance.attack.Down && _comboSystem.CanContinueCombo())
            {
                _comboSystem.AdvanceCombo();
                ExecuteAttack();
            }

            if (_state.NormalizedTime >= 1 && !_comboSystem.CanContinueCombo())
            {
                _actionFSM.ChangeState(_actionIdleState);
                return StateStatus.Success;
            }

            return StateStatus.Running;
        }

        private void ExecuteAttack()
        {
            AttackData attack = _comboSystem.GetCurrentAttack();
            if (attack != null)
            {
                _state = _blackBoard.animancer.Play(attack.AnimationClip);
                _comboSystem.SetCurrentAnimancerState(_state);
                _state.Events.OnEnd += OnAttackAnimationEnd;

                // Apply damage
                float damage = attack.Damage * _comboSystem.GetComboDamageMultiplier();
                ApplyDamage(damage, attack);
            }
        }

        private void ApplyDamage(float damage, AttackData attack)
        {
            //TO DO:
        }

        private void OnAttackAnimationEnd()
        {
            if (!_comboSystem.CanContinueCombo())
            {
                _comboSystem.EndCombo();
                _actionFSM.ChangeState(_actionFSM.GetComponent<ActionIdleState>());
            }
        }

        public override void ExitState()
        {
            base.ExitState();
            if (_state != null)
            {
                _state.Events.OnEnd -= OnAttackAnimationEnd;
            }
            _comboSystem.EndCombo();
        }
    }
}
