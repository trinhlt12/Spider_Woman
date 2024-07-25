using UnityEngine;
using Animancer;
using SFRemastered.InputSystem;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/ComboAttackState")]
    public class ComboAttackState : StateBase
    {
        [SerializeField] private ComboConfig _comboConfig;
        [SerializeField] private ActionIdleState _actionIdleState; // Reference to ActionIdleState

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
            ExecuteNextAttack();
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
                ExecuteNextAttack();
            }

            if (_state?.NormalizedTime >= 1 && !_comboSystem.CanContinueCombo())
            {
                EndCombo();
                return StateStatus.Success;
            }

            return StateStatus.Running;
        }

        private void ExecuteNextAttack()
        {
            AttackData attack = _comboSystem.GetCurrentAttack();
            if (attack != null)
            {
                _state = _blackBoard.animancer.Play(attack.AnimationClip);
                _comboSystem.SetCurrentAnimancerState(_state);
                _state.Events.OnEnd += OnAttackAnimationEnd;

                // Apply damage, effects, etc.
                float damage = attack.Damage * _comboSystem.GetComboDamageMultiplier();
                ApplyDamage(damage, attack);
            }
        }

        private void ApplyDamage(float damage, AttackData attack)
        {
            // Implement your damage application logic here
        }

        private void OnAttackAnimationEnd()
        {
            if (!_comboSystem.CanContinueCombo())
            {
                EndCombo();
            }
        }

        private void EndCombo()
        {
            _comboSystem.EndCombo();
            if (_actionIdleState != null)
            {
                _fsm.ChangeState(_actionIdleState);
            }
            else
            {
                Debug.LogError("ActionIdleState is not set in ComboAttackState");
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