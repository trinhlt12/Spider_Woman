using SFRemastered.InputSystem;
using UnityEngine;
using Cinemachine;
using System.Collections;

namespace SFRemastered
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float lockOnSmoothing = 5f;
        
        public BlackBoard BlackBoard;
        public GameObject CinemachineCameraTarget;
        public float TopClamp = 70.0f;
        public float BottomClamp = -30.0f;
        public float CameraAngleOverride = 0.0f;
        public bool LockCameraPosition = false;

        // Cinemachine
        public float _cinemachineTargetYaw;
        public float _cinemachineTargetPitch;
        private Vector2 look = Vector2.zero;
        private const float _threshold = 0.01f;

        // Target lock
        private Transform lockedTarget;
        private bool isLocked = false;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private float lockOnFOV = 40f;
        [SerializeField] private float fovTransitionDuration = 0.5f;
        private float defaultFOV;
        private Coroutine fovTransitionCoroutine;

        private void Start()
        {
            if (virtualCamera == null)
            {
                virtualCamera = playerTransform.GetComponent<CinemachineVirtualCamera>();
            }
            defaultFOV = virtualCamera.m_Lens.FieldOfView;
        }

        private void Update()
        {
            look.x = InputManager.instance.look.y * 70f;
            look.y = -InputManager.instance.look.x * 70f;
        }

        private void LateUpdate()
        {
            if (isLocked && lockedTarget != null)
            {
                LockOnTarget();
            }
            else
            {
                CameraRotation();
            }
        }

        private void CameraRotation()
        {
            if (TimeManager.instance.pause || InputManager.instance.disableInput)
                return;

            if (look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                float deltaTimeMultiplier = Time.deltaTime;

                _cinemachineTargetYaw += look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += look.y * deltaTimeMultiplier;
            }

            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void LockOnTarget()
        {
            Vector3 directionToTarget = (lockedTarget.position - playerTransform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // Calculate the desired yaw and pitch
            Vector3 targetAngles = targetRotation.eulerAngles;
            float targetYaw = targetAngles.y;
            float targetPitch = targetAngles.x;

            // Smoothly interpolate between current rotation and target rotation
            _cinemachineTargetYaw = Mathf.LerpAngle(_cinemachineTargetYaw, targetYaw, lockOnSmoothing * Time.deltaTime);
            _cinemachineTargetPitch = Mathf.LerpAngle(_cinemachineTargetPitch, targetPitch, lockOnSmoothing * Time.deltaTime);

            // Clamp the pitch
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Apply the rotation
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        public void SetTarget(Transform newTarget)
        {
            lockedTarget = newTarget;
        }

        public void ToggleLock(bool lockState)
        {
            isLocked = lockState;
            float targetFOV = isLocked ? lockOnFOV : defaultFOV;

            // Stop any ongoing FOV transition
            if (fovTransitionCoroutine != null)
            {
                StopCoroutine(fovTransitionCoroutine);
            }

            // Start a new FOV transition
            fovTransitionCoroutine = StartCoroutine(TransitionFOV(targetFOV));

            if (!isLocked)
            {
                lockedTarget = null;
            }
            
            BlackBoard.lockedTarget = lockedTarget;
        }

        private IEnumerator TransitionFOV(float targetFOV)
        {
            float startFOV = virtualCamera.m_Lens.FieldOfView;
            float elapsedTime = 0f;

            while (elapsedTime < fovTransitionDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / fovTransitionDuration);
                float newFOV = Mathf.Lerp(startFOV, targetFOV, t);
                virtualCamera.m_Lens.FieldOfView = newFOV;
                yield return null;
            }

            virtualCamera.m_Lens.FieldOfView = targetFOV;
            fovTransitionCoroutine = null;
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}