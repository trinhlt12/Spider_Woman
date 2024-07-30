using UnityEngine;
using DG.Tweening;
using Animancer;

namespace SFRemastered
{
    [CreateAssetMenu(menuName = "ScriptableObjects/States/ZipAttackState")]
    public class ZipAttackState : StateBase
    {
        [SerializeField] private ClipTransition zipAttackAnimation;
        [SerializeField] private float webShootDuration = 0.5f;
        [SerializeField] private float zipDuration = 1f;
        [SerializeField] private float webThickness = 0.1f;
        [SerializeField] private Ease zipEase = Ease.OutQuad;
        [SerializeField] private ComboAttackState comboAttackState;

        private LineRenderer webLineRenderer;
        private Transform shootPosition;
        private bool isWebShot = false;
        private bool isZipping = false;

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

            PlayZipAttackAnimation();
        }

        private void PlayZipAttackAnimation()
        {
            if (zipAttackAnimation != null)
            {
                _state = _blackBoard.animancer.Play(zipAttackAnimation);
                if (_state != null) _state.Events.OnEnd += OnZipAttackAnimationEnd;
            }
            else
            {
                Debug.LogError("Zip attack animation not assigned!");
            }
        }

        private void OnZipAttackAnimationEnd()
        {
            if (_state != null)
            {
                _state.Events.OnEnd -= OnZipAttackAnimationEnd;
            }
            ShootWeb();
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
                .SetEase(Ease.OutQuad)
                .OnComplete(() => 
                {
                    StartZipping();
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

                DOTween.Sequence()
                    .Append(_blackBoard.transform.DOMove(targetPosition, zipDuration).SetEase(zipEase))
                    .OnUpdate(() =>
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
                    })
                    .OnComplete(() =>
                    {
                        if (webLineRenderer != null)
                        {
                            webLineRenderer.enabled = false;
                        }
                        TransitionToComboAttack();
                    });
            }
        }

        private void TransitionToComboAttack()
        {
            if (comboAttackState != null)
            {
                _fsm.ChangeState(comboAttackState);
            }
            else
            {
                Debug.LogError("ComboAttackState not assigned!");
            }
        }

        public override StateStatus UpdateState()
        {
            // Most of the logic is handled by DOTween sequences
            return StateStatus.Running;
        }

        public override void ExitState()
        {
            base.ExitState();
            if (webLineRenderer != null)
            {
                webLineRenderer.enabled = false;
            }
            DOTween.Kill(_blackBoard.transform);
        }
    }
}