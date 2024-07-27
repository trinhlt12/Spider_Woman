using DG.Tweening;
using UnityEngine;
using Animancer;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ActionStates/ZipAttackState")]
    public class ZipAttackState : ComboAttackState
    {
        [SerializeField] private float webShootDuration = 0.5f;
        [SerializeField] private float zipDuration = 1f;
        [SerializeField] private float webThickness = 0.1f;
        [SerializeField] private Ease webShootEase = Ease.OutQuad;
        [SerializeField] private Ease zipEase = Ease.InOutQuad;
        [SerializeField] private ClipTransition zipAttackAnimation; 
        
        private LineRenderer webLineRenderer;
        private Transform shootPosition;
        private bool isWebShot = false;
        private bool isZipping = false;
        private Sequence zipSequence;

        public override void EnterState()
        {
            base.EnterState();
            shootPosition = _blackBoard.shootPosition;
            isWebShot = false;
            isZipping = false;
            
            webLineRenderer = _fsm.GetComponent<LineRenderer>();
            if (webLineRenderer == null)
            {
                Debug.LogError("LineRenderer component not found on FSM GameObject!");
                return;
            }

            webLineRenderer.startWidth = webThickness;
            webLineRenderer.endWidth = webThickness;
            webLineRenderer.enabled = false;

            // Start the zip attack as the first attack in the combo
            _comboSystem.StartCombo();
            ExecuteZipAttack();
        }

        public override StateStatus UpdateState()
        {
            if (!isWebShot)
            {
                ShootWeb();
            }
            else if (!isZipping)
            {
                StartZipping();
            }
            else
            {
                // Once zipping is complete, continue with normal combo logic
                return base.UpdateState();
            }

            return StateStatus.Running;
        }

        private void ExecuteZipAttack()
        {
            if (zipAttackAnimation != null)
            {
                _state = _blackBoard.animancer.Play(zipAttackAnimation);
                _comboSystem.SetCurrentAnimancerState(_state);
                if (_state != null) _state.Events.OnEnd += OnZipAttackAnimationEnd;
            }
            else
            {
                Debug.LogError("Zip attack animation not assigned!");
            }
        }

        private void ShootWeb()
        {
            if (_blackBoard != null && _blackBoard.lockedTarget != null)
            {
                isWebShot = true;
                Vector3 startPosition = shootPosition.position;
                Vector3 endPosition = _blackBoard.lockedTarget.position;

                webLineRenderer.enabled = true;
                webLineRenderer.SetPosition(0, startPosition);
                webLineRenderer.SetPosition(1, startPosition);

                DOTween.To(() => 0f, x => 
                {
                    webLineRenderer.SetPosition(1, Vector3.Lerp(startPosition, endPosition, x));
                }, 1f, webShootDuration)
                .SetEase(webShootEase)
                .OnComplete(() => 
                {
                    isWebShot = true;
                });
            }
        }

        private void StartZipping()
        {
            if (_blackBoard != null && _blackBoard.lockedTarget != null)
            {
                isZipping = true;
                Vector3 startPosition = _blackBoard.transform.position;
                Vector3 targetPosition = _blackBoard.lockedTarget.position;

                zipSequence = DOTween.Sequence();

                zipSequence.Append(_blackBoard.transform.DOMove(startPosition - _blackBoard.transform.forward * 0.5f, zipDuration * 0.2f).SetEase(Ease.OutQuad));
                zipSequence.Append(_blackBoard.transform.DOMove(targetPosition, zipDuration).SetEase(zipEase));

                zipSequence.OnUpdate(() =>
                {
                    // Update web line
                    if (webLineRenderer != null && webLineRenderer.enabled)
                    {
                        webLineRenderer.SetPosition(0, shootPosition.position);
                        webLineRenderer.SetPosition(1, _blackBoard.lockedTarget.position);
                    }

                    // Rotate character to face the target
                    Vector3 directionToTarget = (_blackBoard.lockedTarget.position - _blackBoard.transform.position).normalized;
                    if (directionToTarget != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                        _blackBoard.transform.rotation = Quaternion.Slerp(_blackBoard.transform.rotation, targetRotation, Time.deltaTime * 10f);
                    }
                });

                zipSequence.OnComplete(() =>
                {
                    if (webLineRenderer != null)
                    {
                        webLineRenderer.enabled = false;
                    }
                    OnZipComplete();
                });
            }
        }

        private void OnZipComplete()
        {
            isZipping = false;
            // Apply damage for the zip attack
            AttackData zipAttack = _comboSystem.GetCurrentAttack();
            if (zipAttack != null)
            {
                float damage = zipAttack.Damage * _comboSystem.GetComboDamageMultiplier();
                ApplyDamage(damage, zipAttack);
            }
            // Advance to the next attack in the combo
            _comboSystem.AdvanceCombo();
            ExecuteNextAttack();
        }

        private void OnZipAttackAnimationEnd()
        {
            if (_state != null)
            {
                _state.Events.OnEnd -= OnZipAttackAnimationEnd;
            }
            // The zip attack animation has ended, but we continue with the zipping motion
        }

        public override void ExitState()
        {
            base.ExitState();
            if (webLineRenderer != null)
            {
                webLineRenderer.enabled = false;
            }
            if (zipSequence != null && zipSequence.IsActive())
            {
                zipSequence.Kill();
            }
            DOTween.Kill(_blackBoard.transform);
        }
    }
}