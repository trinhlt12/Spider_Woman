using DG.Tweening;
using UnityEngine;
using SFRemastered.InputSystem;
using SFRemastered.Ultils;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/ComboAttackState")]
    public class ComboAttackState : StateBase
    {
        [SerializeField] private ComboConfig _comboConfig;
        [SerializeField] private IdleState _idleState;

        private HitEffectManager _hitEffectManager;
        private ComboSystem _comboSystem;
        private bool _isAnimationEnding;

        public override void InitState(FSM fsm, BlackBoard blackBoard, bool isAIControlled)
        {
            base.InitState(fsm, blackBoard, isAIControlled);
            _hitEffectManager = fsm.GetComponent<HitEffectManager>();
            if (_hitEffectManager == null)
            {
                Debug.LogError("HitEffectManager not found on FSM GameObject");
            }
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

                // Play a random clip from the array
                if (attack.AnimationClip is { Length: > 0 })
                {
                    var randomIndex = Random.Range(0, attack.AnimationClip.Length);
                    _state = _blackBoard.animancer.Play(attack.AnimationClip[randomIndex]);
                }
                else
                {
                    Debug.LogError("No animation clips found in AttackData.");
                }

                _comboSystem.SetCurrentAnimancerState(_state);
                if (_state != null) _state.Events.OnEnd += OnAttackAnimationEnd;
                _isAnimationEnding = false;

                // Apply damage, effects, etc.
                float damage = attack.Damage * _comboSystem.GetComboDamageMultiplier();
                ApplyDamage(damage, attack);
            }
        }

        private void ApplyDamage(float damage, AttackData attack)
        {
            Vector3 attackOrigin =
                _blackBoard.transform.position + _blackBoard.transform.forward * attack.AttackRange / 2f;
            Vector3 attackDirection = _blackBoard.transform.forward;

            Collider[] hitColliders = Physics.OverlapBox(
                attackOrigin,
                attack.HitboxSize / 2f,
                _blackBoard.transform.rotation,
                LayerMask.GetMask("Enemy")
            );

            foreach (var hitCollider in hitColliders)
            {
                IHittable hittable = hitCollider.GetComponent<IHittable>();
                if (hittable != null)
                {
                    Vector3 hitPoint = hitCollider.ClosestPoint(attackOrigin);
                    Vector3 hitNormal = (hitPoint - attackOrigin).normalized;

                    Vector3 impactDirection = Vector3.Reflect(attackDirection, hitNormal).normalized;
                    hittable.TakeHit(damage, hitPoint, impactDirection);
                    
                    // Play hit effect
                    if (_hitEffectManager != null)
                    {
                        _hitEffectManager.PlayHitEffect(hitPoint, hitNormal);
                    }
                }
            }
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