using UnityEngine;
using System.Linq;
using ParadoxNotion.Design;

namespace SFRemastered
{
    [Category("Custom")]
    public class NearestEnemyDetector : MonoBehaviour
    {
        [SerializeField] private BlackBoard _blackBoard;
        [SerializeField] private float detectionRadius = 20f;
        [SerializeField] private LayerMask enemyLayerMask;
        [SerializeField] private float updateInterval = 0.1f;
        [SerializeField] private GameObject targetIconPrefab;
        [SerializeField] private float iconOffset = 2f;

        private Transform currentTarget;
        private float lastUpdateTime;
        private GameObject currentTargetIcon;
        private CameraController cameraController;

        private void Start()
        {
            cameraController = _blackBoard.camera.GetComponent<CameraController>();
            if (cameraController == null)
            {
                Debug.LogError("CameraController not found on BlackBoard!");
            }
        }

        private void Update()
        {
            if (Time.time - lastUpdateTime > updateInterval)
            {
                DetectAndLockTarget();
                lastUpdateTime = Time.time;
            }

            UpdateTargetIconPosition();
        }

        private void DetectAndLockTarget()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayerMask);

            Transform nearestEnemy = hitColliders
                .OrderBy(c => Vector3.Distance(transform.position, c.transform.position))
                .FirstOrDefault()?.transform;

            if (nearestEnemy != currentTarget)
            {
                UpdateTarget(nearestEnemy);
            }
        }

        private void UpdateTarget(Transform newTarget)
        {
            DestroyTargetIcon();
            currentTarget = newTarget;

            if (currentTarget != null)
            {
                CreateTargetIcon();
                cameraController.SetTarget(currentTarget);
                cameraController.ToggleLock(true);
            }
            else
            {
                cameraController.ToggleLock(false);
            }
        }

        private void CreateTargetIcon()
        {
            if (targetIconPrefab != null && currentTarget != null)
            {
                currentTargetIcon = Instantiate(targetIconPrefab, GetIconPosition(), Quaternion.identity, currentTarget);
            }
        }

        private void DestroyTargetIcon()
        {
            if (currentTargetIcon != null)
            {
                Destroy(currentTargetIcon);
                currentTargetIcon = null;
            }
        }

        private void UpdateTargetIconPosition()
        {
            if (currentTargetIcon != null && currentTarget != null)
            {
                currentTargetIcon.transform.position = GetIconPosition();
                currentTargetIcon.transform.LookAt(Camera.main.transform);
            }
        }

        private Vector3 GetIconPosition()
        {
            return currentTarget.position + Vector3.up * iconOffset;
        }

        public Transform GetCurrentTarget() => currentTarget;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }

        private void OnDisable()
        {
            DestroyTargetIcon();
            if (cameraController != null)
            {
                cameraController.ToggleLock(false);
            }
        }
    }
}