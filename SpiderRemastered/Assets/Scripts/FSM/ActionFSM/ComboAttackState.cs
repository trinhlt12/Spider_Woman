using UnityEngine;
using Animancer;
using SFRemastered.InputSystem;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/ComboAttackState")]
    public class ComboAttackState : StateBase
    {
        [SerializeField] private ComboConfig _comboConfig;
        [SerializeField] private IdleState _idleState;

        private ComboSystem _comboSystem;
        private bool _isAnimationEnding = false;

        public override void InitState(FSM fsm, BlackBoard blackBoard, bool isAIControlled)
        {
            base.InitState(fsm, blackBoard, isAIControlled);
            _comboSystem = new ComboSystem(_comboConfig);
        }

        public override void EnterState()
        {
            base.EnterState();
            _comboSystem.StartCombo();
            _isAnimationEnding = false;
            ExecuteNextAttack();
        }

        public override StateStatus UpdateState()
        {
            StateStatus baseStatus = base.UpdateState();
            if (baseStatus != StateStatus.Running)
            {
                return baseStatus;
            }

            if (!_isAnimationEnding && InputManager.instance.attack.Down && _comboSystem.CanContinueCombo())
            {
                _comboSystem.AdvanceCombo();
                ExecuteNextAttack();
            }

            if (_state?.NormalizedTime >= 1 && !_isAnimationEnding)
            {
                OnAttackAnimationEnd();
            }

            return StateStatus.Running;
        }

        private void ExecuteNextAttack()
        {
            AttackData attack = _comboSystem.GetCurrentAttack();
            if (attack != null)
            {
                if (_state != null)
                {
                    _state.Events.OnEnd -= OnAttackAnimationEnd;
                }

                _state = _blackBoard.animancer.Play(attack.AnimationClip);
                _comboSystem.SetCurrentAnimancerState(_state);
                _state.Events.OnEnd += OnAttackAnimationEnd;
                _isAnimationEnding = false;

                // Apply damage, effects, etc.
                float damage = attack.Damage * _comboSystem.GetComboDamageMultiplier();
                ApplyDamage(damage, attack);
            }
        }

        private void ApplyDamage(float damage, AttackData attack)
        {
            // TO DO
        }

        private void OnAttackAnimationEnd()
        {
            if (_isAnimationEnding) return;
            _isAnimationEnding = true;

            if (_state != null)
            {
                _state.Events.OnEnd -= OnAttackAnimationEnd;
            }

            if (!_comboSystem.CanContinueCombo())
            {
                EndCombo();
            }
        }

        private void EndCombo()
        {
            _comboSystem.EndCombo();
            _fsm.ChangeState(_idleState);
        }

        public override void ExitState()
        {
            base.ExitState();
            if (_state != null)
            {
                _state.Events.OnEnd -= OnAttackAnimationEnd;
            }
            _comboSystem.EndCombo();
            _isAnimationEnding = false;
        }
    }
}