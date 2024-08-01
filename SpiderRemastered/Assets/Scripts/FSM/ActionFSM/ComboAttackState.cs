using DG.Tweening;
using EasyCharacterMovement;
using UnityEngine;
using SFRemastered.InputSystem;
using SFRemastered.Ultils;
using UnityEngine.Serialization;

// ReSharper disable All

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/ComboAttackState")]
    public class ComboAttackState : StateBase
    {
        [SerializeField] private ComboConfig _comboConfig;
        [SerializeField] private IdleState _idleState;
        [SerializeField] private SprintState _sprintState;

        private HitEffectManager _hitEffectManager;
        protected ComboSystem _comboSystem;
        private CameraController _cameraController;
        private bool _isAnimationEnding;
        private Transform _lockedTarget;

        public override void InitState(FSM fsm, BlackBoard blackBoard, bool isAIControlled)
        {
            base.InitState(fsm, blackBoard, isAIControlled);
            _hitEffectManager = fsm.GetComponent<HitEffectManager>();
            if (_hitEffectManager == null)
            {
                Debug.LogError("HitEffectManager not found on FSM GameObject");
            }
            _comboSystem = new ComboSystem(_comboConfig);
            _cameraController = Camera.main.GetComponent<CameraController>();
        }

        public override void EnterState()
        {
            base.EnterState();
            _comboSystem.StartCombo();
            _isAnimationEnding = false;
            _blackBoard.PlayerVars.playerMovement.useRootMotion = true;
            _blackBoard.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            _blackBoard.GetComponent<Rigidbody>().isKinematic = false;
            _blackBoard.GetComponent<Rigidbody>().velocity = Vector3.zero;
            RotateTowardsTarget();
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

        protected void ExecuteNextAttack()
        {
            AttackData attack = _comboSystem.GetCurrentAttack();
            if (attack != null)
            {
                if (_state != null)
                {
                    _state.Events.OnEnd -= OnAttackAnimationEnd;
                }

                // Play a random clip from the array
                if (attack.AnimationClip != null && attack.AnimationClip.Count > 0)
                {
                    int randomIndex = Random.Range(0, attack.AnimationClip.Count);
                    _state = _blackBoard.CommonVars.animancer.Play(attack.AnimationClip[randomIndex]);
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
        
        
        private void RotateTowardsTarget()
        {
            Transform target = NearestEnemyDetector.GetCurrentTarget();
            if (target != null)
            {
                Vector3 targetPosition = target.position;
                Vector3 playerPosition = _blackBoard.transform.position;
        
                // Calculate direction on the horizontal plane only
                Vector3 direction = targetPosition - playerPosition;
                direction.y = 0;

                if (direction != Vector3.zero)
                {
                    // Calculate the target rotation on the Y-axis only
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

                    // Use DOTween to smoothly rotate to the target rotation
                    _blackBoard.transform.DORotate(targetRotation.eulerAngles, 0.2f)
                        .SetEase(Ease.OutSine)
                        .OnComplete(() =>
                        {
                            _blackBoard.transform.rotation = targetRotation;

                            if (_blackBoard.GetComponent<Rigidbody>() != null)
                            {
                                _blackBoard.GetComponent<Rigidbody>().rotation = targetRotation;
                            }
                        });
                }
            }
        }

        protected void ApplyDamage(float damage, AttackData attack)
        {
            _blackBoard.PlayerVars.attackRange = attack.HitboxSize.magnitude / 2f;
            Vector3 attackOrigin =
                _blackBoard.transform.position + _blackBoard.transform.forward * _blackBoard.PlayerVars.attackRange / 2f;
            Vector3 attackDirection = _blackBoard.transform.forward;

            Collider[] hitColliders = Physics.OverlapBox(
                attackOrigin,
                new Vector3(attack.HitboxSize.x, attack.HitboxSize.y, attack.HitboxSize.z * 2),
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
            
            _blackBoard.PlayerVars.playerMovement.useRootMotion = false;

            if (!_comboSystem.CanContinueCombo())
            {
                EndCombo();
            }
        }

        private void EndCombo()
        {
            _comboSystem.EndCombo();
            _fsm.ChangeState(_idleState);
            _blackBoard.PlayerVars.playerMovement.SetVelocity(Vector3.zero);
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
            _blackBoard.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _blackBoard.GetComponent<Rigidbody>().isKinematic = true;
            _blackBoard.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            _blackBoard.PlayerVars.playerMovement.useRootMotion = false;
        }

        private void MoveTowardsTarget()
        {
            if(_lockedTarget == null) return;
            
            float distance = Vector3.Distance(_blackBoard.transform.position, _lockedTarget.position);
            
            _blackBoard.PlayerVars.mediumRange = _blackBoard.PlayerVars.attackRange * 2f;
            _blackBoard.PlayerVars.farRange = _blackBoard.PlayerVars.attackRange * 3f;
            
            Vector3 direction = (_lockedTarget.position - _blackBoard.transform.position).normalized;
            float maxSpeed = _blackBoard.PlayerVars.playerMovement.GetMaxSpeed();
        }
    }
}